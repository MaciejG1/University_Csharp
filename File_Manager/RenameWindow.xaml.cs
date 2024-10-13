using System.Windows;

namespace File_Explorer
{
    public partial class RenameWindow : Window
    {
        public string NewName { get; private set; }

        public RenameWindow(string currentName)
        {
            InitializeComponent();
            NewNameTextBox.Text = currentName;
            NewNameTextBox.SelectAll();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            NewName = NewNameTextBox.Text;
            DialogResult = true; // it means that button ok has been clicked
        }
    }
}
