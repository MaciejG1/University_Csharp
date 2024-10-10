using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace File_Explorer
{
    public class FileItem
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }

    /// <summary>
    /// Logika interakcji dla klasy FilesManager.xaml
    /// </summary>
    public partial class FilesManager : UserControl
    {
        public string folderPath="";
        protected string filePath = "C:\\Users";
        private string currentlySelectedItemName = "";
        private bool isFile = false;
        //to sorting function
        private ICollectionView collectionView;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;
        private string _lastSortedColumn = string.Empty;
        public FilesManager OtherManager { get; set; }

        public FilesManager()
        {
            InitializeComponent();
            filePathTextBox.Text = filePath;
            folderPath = filePathTextBox.Text;
            LoadFilesAndDirectories();
        }

        protected void LoadFilesAndDirectories()
        {
            FileSystemInfo[] items;
            List<FileItem> fileItems;
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            

            try
            {
                items = directoryInfo.GetFileSystemInfos();
                fileItems = new List<FileItem>();
                foreach (FileSystemInfo item in items)
                {
                    fileItems.Add(new FileItem
                    {
                        Name = item.Name,
                        CreationDate = item.CreationTime
                    });
                }
                fileListView.ItemsSource = fileItems;
                //initialization the view of files for sorting functions
                collectionView = CollectionViewSource.GetDefaultView(fileListView.ItemsSource);
                collectionView.Refresh(); // Odświeżenie widoku
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            filePath = filePathTextBox.Text;

            if (fileListView.SelectedItem != null)
            {
                FileItem selectedItem = fileListView.SelectedItem as FileItem;
                if (selectedItem != null)
                {
                    filePath = System.IO.Path.Combine(filePath, selectedItem.Name);
                    filePathTextBox.Text = filePath;
                    folderPath = filePathTextBox.Text;
                }
            }
            
            LoadFilesAndDirectories();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DirectoryInfo parentDir = Directory.GetParent(filePath);
                if (parentDir != null)
                {
                    filePath = parentDir.FullName;
                    filePathTextBox.Text = filePath;
                    folderPath = filePathTextBox.Text;
                    LoadFilesAndDirectories();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FileListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (fileListView.SelectedItem != null)
            {
                FileItem selectedItem = fileListView.SelectedItem as FileItem;
                if (selectedItem != null)
                {
                    string newPath = System.IO.Path.Combine(filePath, selectedItem.Name);

                    if (Directory.Exists(newPath))
                    {
                        filePath = newPath;
                        filePathTextBox.Text = filePath;
                        folderPath = filePathTextBox.Text;
                        LoadFilesAndDirectories();
                    }
                    else if (File.Exists(newPath))
                    {
                        try
                        {
                            ProcessStartInfo psi = new ProcessStartInfo
                            {
                                FileName = newPath,
                                UseShellExecute = true
                            };
                            Process.Start(psi);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Cannot open file: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (fileListView.SelectedItem != null)
            {
                FileItem selectedItem = fileListView.SelectedItem as FileItem;
                if (selectedItem != null)
                {
                    // Źródłowa ścieżka pliku lub folderu
                    string sourcePath = System.IO.Path.Combine(folderPath, selectedItem.Name);
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                    // Sprawdzamy, czy obiekt FilesManager dla prawej strony jest dostępny
                    if (mainWindow.RightSide != null)
                    {
                        // Ścieżka docelowa w prawej instancji FilesManager
                        string destinationPath = System.IO.Path.Combine(mainWindow.RightSide.folderPath, selectedItem.Name);

                        try
                        {
                            if (File.Exists(sourcePath) && !File.Exists(destinationPath)) // Jeśli wybrano plik
                            {

                                File.Copy(sourcePath, destinationPath, overwrite: true); // Kopiowanie pliku
                                MessageBox.Show($"File {selectedItem.Name} copied to {destinationPath}");
                            }
                            else if (Directory.Exists(sourcePath) && sourcePath != mainWindow.RightSide.folderPath) // Jeśli wybrano folder
                            {
                                CopyDirectory(sourcePath, destinationPath); // Kopiowanie katalogu
                                MessageBox.Show($"Directory {selectedItem.Name} copied from {sourcePath} to {mainWindow.RightSide.folderPath}");
                            }
                            else if (File.Exists(destinationPath))
                            {
                                MessageBox.Show("A file with this name already exist.");
                            }
                            else if (sourcePath == mainWindow.RightSide.folderPath)
                            {
                                MessageBox.Show("You cannot copy folder to itself");
                            }
                            else
                            {
                                MessageBox.Show("Selected item does not exist as a file or directory.");
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error copying: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Destination explorer (RightSide) is not available.");
                    }
                }
            }
            else
            {
                MessageBox.Show("No item selected.");
            }
        }

        // Funkcja do rekurencyjnego kopiowania katalogów
        // Funkcja do bezpiecznego rekurencyjnego kopiowania katalogów
        private void CopyDirectory(string sourceDir, string destinationDir)
        {
            try
            {
                // Upewnij się, że katalog źródłowy istnieje
                if (!Directory.Exists(sourceDir))
                {
                    MessageBox.Show("Source directory does not exist: " + sourceDir);
                    return;
                }

                // Jeśli katalog docelowy nie istnieje, utwórz go
                if (!Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }

                // Kopiuj wszystkie pliki w bieżącym katalogu
                foreach (string file in Directory.GetFiles(sourceDir))
                {
                    string destFile = System.IO.Path.Combine(destinationDir, System.IO.Path.GetFileName(file));

                    // Sprawdź, czy plik o tej samej nazwie już istnieje
                    if (File.Exists(destFile))
                    {
                        MessageBoxResult result = MessageBox.Show(
                            $"File {System.IO.Path.GetFileName(file)} already exists in the destination. Do you want to overwrite it?",
                            "File exists",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.No)
                        {
                            continue; // Jeśli nie chcesz nadpisywać, przejdź do następnego pliku
                        }
                    }

                    try
                    {
                        File.Copy(file, destFile, overwrite: true);
                    }
                    catch (Exception fileEx)
                    {
                        MessageBox.Show("Error copying file: " + file + "\n" + fileEx.Message);
                    }
                }

                // Rekurencyjnie kopiuj podkatalogi
                foreach (string directory in Directory.GetDirectories(sourceDir))
                {
                    string destDir = System.IO.Path.Combine(destinationDir, System.IO.Path.GetFileName(directory));

                    // Sprawdź, czy katalog źródłowy i docelowy nie są takie same, aby uniknąć pętli
                    if (!sourceDir.Equals(destDir, StringComparison.OrdinalIgnoreCase))
                    {
                        CopyDirectory(directory, destDir);
                    }
                    else
                    {
                        MessageBox.Show("Cannot copy the folder into itself: " + directory);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error copying directory: " + ex.Message);
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (fileListView.SelectedItem != null)
            {
                FileItem selectedItem = fileListView.SelectedItem as FileItem;
                if (selectedItem != null)
                {
                    string fullPath = System.IO.Path.Combine(filePath, selectedItem.Name);

                    try
                    {
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath); // Usuń plik
                        }
                        else if (Directory.Exists(fullPath))
                        {
                            Directory.Delete(fullPath, true); // Usuń katalog
                        }
                        LoadFilesAndDirectories();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void Sorting(object sender, RoutedEventArgs e)
        {
            Button headerClicked = sender as Button;
            string sortBy = headerClicked.Tag.ToString();

            // Jeśli kliknięto tę samą kolumnę, zmień kierunek sortowania
            if (_lastSortedColumn == sortBy)
            {
                _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                                 ListSortDirection.Descending : ListSortDirection.Ascending;
            }
            else
            {
                _sortDirection = ListSortDirection.Ascending;
            }

            _lastSortedColumn = sortBy;
            Sort(sortBy, _sortDirection);
        }
        private void Sort(string sortBy, ListSortDirection direction)
        {
            if (collectionView == null)
            {
                collectionView = CollectionViewSource.GetDefaultView(fileListView.ItemsSource);
            }

            collectionView.SortDescriptions.Clear();
            collectionView.SortDescriptions.Add(new SortDescription(sortBy, direction));

            collectionView.Refresh();
        }
    }
}
