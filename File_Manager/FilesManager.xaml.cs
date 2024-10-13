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
      
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        public FilesManager()
        {
            InitializeComponent();
            filePathTextBox.Text = filePath;
            folderPath = filePathTextBox.Text;
            LoadFilesAndDirectories();
            fileListView.Drop += FileListView_Drop;
            fileListView.DragEnter += FileListView_DragEnter;
       
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
                collectionView.Refresh();
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
                    
                    string sourcePath = System.IO.Path.Combine(folderPath, selectedItem.Name);

                    
                    if (mainWindow.RightSide != null)
                    {
                        
                        string destinationPath = System.IO.Path.Combine(mainWindow.RightSide.folderPath, selectedItem.Name);

                        try
                        {
                            
                            MessageBoxResult result = MessageBox.Show(
                                $"Are you sure you want to copy the file or folder '{selectedItem.Name}' from \n'{sourcePath}' \nto \n'{destinationPath}'?",
                                "Confirm Copy",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);

                            if (result == MessageBoxResult.Yes)
                            {
                                
                                if (File.Exists(sourcePath) && !File.Exists(destinationPath)) //copy file
                                {
                                    File.Copy(sourcePath, destinationPath, overwrite: true); 
                                    MessageBox.Show($"File {selectedItem.Name} copied to {destinationPath}");
                                }
                                else if (Directory.Exists(sourcePath) && sourcePath != mainWindow.RightSide.folderPath) // copy directory
                                {
                                    CopyDirectory(sourcePath, destinationPath); 
                                    MessageBox.Show($"Directory {selectedItem.Name} copied to {mainWindow.RightSide.folderPath}");
                                }
                                else if (File.Exists(destinationPath))
                                {
                                    MessageBox.Show("A file with this name already exists.");
                                }
                                else if (sourcePath == mainWindow.RightSide.folderPath)
                                {
                                    MessageBox.Show("You cannot copy folder to itself");
                                }
                                else
                                {
                                    MessageBox.Show("Selected item does not exist as a file or directory.");
                                }

                                // refresh the view of the files
                                mainWindow.LeftSide.LoadFilesAndDirectories();
                                mainWindow.RightSide.LoadFilesAndDirectories();
                            }
                            else
                            {
                                // cancellation of copy
                                MessageBox.Show("Copy operation cancelled.");
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

        private void CopyDirectory(string sourceDir, string destinationDir)
        {
            try
            {
                
                if (!Directory.Exists(sourceDir))
                {
                    MessageBox.Show("Source directory does not exist: " + sourceDir);
                    return;
                }

                // if directory doesnt exist - create new one
                if (!Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }

                // copy all of the files 
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
                            continue; // if u dont want to overwrite it, just skip this
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

                // Recursion copy of subdirectories
                foreach (string directory in Directory.GetDirectories(sourceDir))
                {
                    string destDir = System.IO.Path.Combine(destinationDir, System.IO.Path.GetFileName(directory));

                    // checking if source directory is equal to destionation directory
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
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete '{selectedItem.Name}'?",
                     "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            if (File.Exists(fullPath))
                            {
                                File.Delete(fullPath); // Delete Files      
                            }
                            else if (Directory.Exists(fullPath))
                            {
                                Directory.Delete(fullPath, true); // Delete Directories
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
        }
        //-------------------------newFolderFunctions-------------------------------------------------------//
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.Key == Key.F7) //create new folder
            {
                
                NewFolder_Click(sender, e);
            } 
            else if (e.Key == Key.F8) // delete file
            {
               
                DeleteMenuItem_Click(sender,e);
            }
            else if (e.Key == Key.F5) //refresh function
            {
                mainWindow.LeftSide.LoadFilesAndDirectories();
                mainWindow.RightSide.LoadFilesAndDirectories();
            }
        }
        private void NewFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(folderPath))
                {
                    // Generate unique name of directory
                    string newFolderName = "New folder";
                    string newFolderPath = System.IO.Path.Combine(folderPath, newFolderName);
                    int folderIndex = 1;

                    while (Directory.Exists(newFolderPath))
                    {
                        newFolderName = $"New folder ({folderIndex++})";
                        newFolderPath = System.IO.Path.Combine(folderPath, newFolderName);
                    }

                    // Create New Folder
                    Directory.CreateDirectory(newFolderPath);

                    
                    LoadFilesAndDirectories();

                    // searching new item on the list and make focus on it
                    FileItem newFolderItem = fileListView.Items
                        .Cast<FileItem>()
                        .FirstOrDefault(item => item.Name == newFolderName);

                    if (newFolderItem != null)
                    {
                        
                        fileListView.ScrollIntoView(newFolderItem);
                        fileListView.SelectedItem = newFolderItem;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"The folder cannot be created. {exception.Message}");
            }
        }
        private void ChangeName_Click(object sender, RoutedEventArgs e)
        {
            if (fileListView.SelectedItem is FileItem selectedItem)
            {
                // open new window to change a name of the folder
                RenameWindow renameWindow = new RenameWindow(selectedItem.Name);
                bool? result = renameWindow.ShowDialog();

                if (result == true)
                {
                    string newName = renameWindow.NewName;
                    CompleteRename(selectedItem, newName); 
                }
            }
        }
        private void CompleteRename(FileItem selectedItem, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("The name cannot be empty.");
                return;
            }

            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            if (newName.Any(c => invalidChars.Contains(c)))
            {
                MessageBox.Show("The name contains invalid characters.");
                return;
            }

            string oldPath = System.IO.Path.Combine(folderPath, selectedItem.Name);
            string newPath = System.IO.Path.Combine(folderPath, newName);

            try
            {
                if (Directory.Exists(oldPath))
                {
                    Directory.Move(oldPath, newPath);
                }
                else if (File.Exists(oldPath))
                {
                    File.Move(oldPath, newPath);
                }

                selectedItem.Name = newName; // Update name in object FileItem
                fileListView.Items.Refresh(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error renaming item: {ex.Message}");
            }
        }
        //--------------------------EndNewFolderFunctions---------------------------------------------------//

        //------------------------code to drag&drop function-----------------------------------------------//
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed && fileListView.SelectedItem != null)
            {
                FileItem selectedItem = fileListView.SelectedItem as FileItem;
                if (selectedItem != null)
                {
                    DataObject data = new DataObject();
                    data.SetData("FileItem", selectedItem); //Set right type of object. In this situation it is "FileItem"

                    DragDrop.DoDragDrop(fileListView, data, DragDropEffects.Move);
                }
            }
        }
        private void FileListView_DragEnter(object sender, DragEventArgs e)
        {
            // checking if it's any data to move
            if (e.Data.GetDataPresent("FileItem"))
            {
                e.Effects = DragDropEffects.Move; 
            }
            else
            {
                e.Effects = DragDropEffects.None; 
            }
        }
        private void FileListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FileItem"))
            {
                // Receive Data
                FileItem selectedItem = e.Data.GetData("FileItem") as FileItem;

                if (selectedItem != null)
                {
                    
                    string sourcePath = System.IO.Path.Combine(mainWindow.LeftSide.folderPath, selectedItem.Name);
                    string destinationPath = System.IO.Path.Combine(mainWindow.RightSide.folderPath, selectedItem.Name);

                    

                    try
                    {
                        // Validate of the same path
                        if (sourcePath == destinationPath)
                        {
                            MessageBox.Show("Cannot move to the same location.");
                            return;
                        }

                        // Authorization 
                        MessageBoxResult result = MessageBox.Show(
                            $"Are you sure you want to copy the item '{selectedItem.Name}' from \n'{sourcePath}' \nto \n'{destinationPath}'?",
                            "Confirm Copy",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            
                            if (File.Exists(sourcePath))
                            {
                                File.Copy(sourcePath, destinationPath, overwrite: true); // Copy file
                                MessageBox.Show($"File {selectedItem.Name} copied to {destinationPath}");
                            }
                            else if (Directory.Exists(sourcePath))
                            {
                                CopyDirectory(sourcePath, destinationPath); // Copy directory
                                MessageBox.Show($"Directory {selectedItem.Name} copied to {destinationPath}");
                            }

                           //refresh of the view after copy operation
                            LoadFilesAndDirectories();
                        }
                        else
                        {
                            
                            MessageBox.Show("Copy operation cancelled.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error moving item: {ex.Message}");
                    }
                }
            }
        }
        //-----------------------end code to drag&drop function--------------------------------------------//
        private void Sorting(object sender, RoutedEventArgs e)
        {
            Button headerClicked = sender as Button;
            string sortBy = headerClicked.Tag.ToString();

            // if column has been clicked, change the way of sorting
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
