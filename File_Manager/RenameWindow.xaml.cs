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
            NewNameTextBox.SelectAll(); // Zaznacza tekst w polu tekstowym
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            NewName = NewNameTextBox.Text;
            DialogResult = true; // Ustawiamy wynik dialogu na "true", co oznacza, że OK zostało kliknięte
        }
    }
}
