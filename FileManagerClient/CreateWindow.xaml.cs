using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace FileManagerClient
{
    public partial class CreateWindow : Window
    {
        public CreateWindow()
        {
            InitializeComponent();
            tbCreateNewName.Focus();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string NewName
        {
            get { return tbCreateNewName.Text; }
        }

    }
}