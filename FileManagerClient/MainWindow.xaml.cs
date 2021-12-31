using FileManagerClient.Agent;
using FileManagerClient.Agent.Client;
using FileManagerClient.Agent.Client.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileManagerClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //ILogger<FileManagerAgentClient> _logger = LoggerExtensions;
        public MainWindow()
        {
            InitializeComponent();
            string[] arg = { };
            //CreateHostBuilder(arg).Build().Run();
            SetDrive();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            }).UseNLog();


        public void SetDrive()
        {
            var drives = GetDrives();
            cmbDrives.ItemsSource = drives;
            cmbDrives.SelectedItem = drives[0];
        }


        public List<string> GetDrives()
        {
            var drives = AgentConnect.GetAllDrivers(new FileManagerInformatorAgentClient(
                new System.Net.Http.HttpClient()));

            return drives.Drives;
        }

        public List<string> GetDrives(object sender, MouseEventArgs e)
        {
            var drives = AgentConnect.GetAllDrivers(new FileManagerInformatorAgentClient(
                new System.Net.Http.HttpClient()));

            return drives.Drives;
        }



        //private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        //{
        //    int firstcharindex = FileFolderArea.GetFirstCharIndexOfCurrentLine();

        //    int currentline = FileFolderArea.GetLineFromCharIndex(firstcharindex);

        //    string currentlinetext = FileFolderArea.Lines[currentline];

        //    FileFolderArea.Select(firstcharindex, currentlinetext.Length + 1);

        //}




        private void FileFolderArea_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                //FileFolderArea.Document.Blocks.Clear();

                string text = "D:/";



                //var start = FileFolderArea.Document.ContentStart;
                //var end = FileFolderArea.Document.ContentEnd;
                //FileFolderArea.Selection.Select(start, end);
            }
        }


        private void ListBox_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ListBox listBoxTemp = new ListBox();
                Image image = new Image();
            }
        }



        private void Login_Click(object sender, RoutedEventArgs e)
        {
            RenameWindow renameWindow = new RenameWindow();

            if (renameWindow.ShowDialog() == true)
            {
                if (renameWindow.NewName == "12345678")
                    MessageBox.Show("Авторизация пройдена");
                else
                    MessageBox.Show("Неверный пароль");
            }
            else
            {
                MessageBox.Show("Авторизация не пройдена");
            }
        }
    }
}
