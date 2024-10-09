using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace File_Explorer
{
    public partial class MainWindow : Window
    {
      

        public MainWindow()
        {
            InitializeComponent();
            // Inicjalizacja strony i wyświetlenie jej w kontrolce Frame
            FilesManager LeftSidefilesManager = new FilesManager();
            FilesManager RightSidefilesManager = new FilesManager();
          

        }
      
        
    }
}
