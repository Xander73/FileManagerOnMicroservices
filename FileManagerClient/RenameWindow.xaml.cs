using System.Windows;

namespace FileManagerClient
{
    public partial class RenameWindow : Window
    {
        public RenameWindow()
        {
            InitializeComponent();
            newName.Focus();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string NewName
        {
            get { return newName.Text; }
        }

    }
}