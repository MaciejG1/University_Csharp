using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// <summary>
    /// Logika interakcji dla klasy FilesManager.xaml
    /// </summary>
    public partial class FilesManager : UserControl
    {
        private string filePath = "C:\\Users";
        private string currentlySelectedItemName = "";
        private bool isFile = false;
        public FilesManager()
        {
            InitializeComponent();
            filePathTextBox.Text = filePath;
            LoadFilesAndDirectories();
        }

        private void LoadFilesAndDirectories()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            fileListView.Items.Clear();

            try
            {
                FileSystemInfo[] items = directoryInfo.GetFileSystemInfos();
                foreach (FileSystemInfo item in items)
                {
                    fileListView.Items.Add(item.Name);
                }
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
                filePath= filePath + "\\" + fileListView.SelectedItem.ToString();
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
                string selectedItem = fileListView.SelectedItem.ToString();
                string newPath = System.IO.Path.Combine(filePath, selectedItem);

                if (Directory.Exists(newPath))
                {
                    filePath = newPath;
                    filePathTextBox.Text = filePath;
                    LoadFilesAndDirectories();
                }
                else if (File.Exists(newPath))
                {
                    // System.Diagnostics.Process.Start(newPath); // Otwórz plik
                    try
                    {
                        // Użyj ProcessStartInfo do otwierania plików
                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = newPath,
                            UseShellExecute = true  // Pozwala na otwarcie pliku w domyślnej aplikacji
                        };
                        Process.Start(psi);  // Otwórz plik
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Cannot open file: " + ex.Message);
                    }
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (fileListView.SelectedItem != null)
            {
                string selectedItem = fileListView.SelectedItem.ToString();
                string fullPath = System.IO.Path.Combine(filePath, selectedItem);

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
                    LoadFilesAndDirectories(); // Odśwież widok
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (fileListView.SelectedItem != null)
            {
                string selectedItem = fileListView.SelectedItem.ToString();
                string fullPath = System.IO.Path.Combine(filePath, selectedItem);

                if (File.Exists(fullPath))
                {
                    // Implementacja kopiowania plików
                    string destPath = "C:\\SomeDestinationPath\\" + selectedItem; //docelowo dopisac sciezke do ktorej ma byc kopiowany
                    File.Copy(fullPath, destPath);
                }
            }
        }
    }
}
