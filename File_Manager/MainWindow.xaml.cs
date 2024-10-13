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
        private FilesManager leftSideManager;
        private FilesManager rightSideManager;

        public MainWindow()
        {
            InitializeComponent();
            leftSideManager = new FilesManager();
            rightSideManager = new FilesManager();

   
        }


    }
}
