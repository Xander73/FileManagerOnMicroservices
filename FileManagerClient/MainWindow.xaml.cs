using Core;
using Core.Interfaces;
using Core.Models;
using Core.Models.DTO;
using Core.Models.Responses;
using FileManagerClient.Agent;
using FileManagerClient.Agent.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NLog.Web;
using NLog;

namespace FileManagerClient
{
    public partial class MainWindow : Window
    {
        private static Logger _logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        private Dictionary<string, MyFileDTO> _files;
        private Dictionary<string, MyFolderDTO> _folders;
        private Dictionary<string, List<Item>> searchItems;
        private string _pathFromCopy = "";


        public MainWindow()
        {
            InitializeComponent();
            InitiliazeMainWindow();
            currentPath.Text = cmbDrives.Text;
            _logger.Info("MainWindow initial");

        }


        private void InitiliazeMainWindow()
        {
            _files = new Dictionary<string, MyFileDTO>();
            _folders = new Dictionary<string, MyFolderDTO>();
            searchItems = new Dictionary<string, List<Item>>();
            tbItemInfo.IsReadOnly = true;
            btnPaste.IsEnabled = !string.IsNullOrEmpty(_pathFromCopy);
            btnRename.IsEnabled = foldersFiles.SelectedItem != null;
            btnCopy.IsEnabled = foldersFiles.SelectedItem != null;
            btnDelete.IsEnabled = foldersFiles.SelectedItem != null;

            SetDrive();
            currentPath.Text = cmbDrives.Items.CurrentItem.ToString();
            PrintFilesFolders(currentPath.Text);
        }


        private List<string> GetDrives()
        {
            _logger.Info("Init MainWindow::GetDrives()");
            var drives = AgentConnect.GetAllDrives(new FileManagerInformatorAgentClient(
                new HttpClient()));
            if (drives.ex != null)
            {
                MessageBox.Show($"Возникла ошибка - {drives.ex.Message}", "Ошибка");
                return new List<string>();
            }

            return drives.Drives;
        }


        private void SetDrive()
        {
            _logger.Info("Init MainWindow::SetDrive()");
            var drives = GetDrives();
            cmbDrives.ItemsSource = drives;
            cmbDrives.SelectedItem = drives[0];
        }


        public void PrintFilesFolders(string requiredFolder)
        {
            _logger.Info("Init MainWindow::PrintFilesFolders(string)");
            currentPath.Text = requiredFolder;
            foldersFiles.Items.Clear();
            SetItemsInCurrentFolder(requiredFolder);

            foreach (var item in _folders.Keys)
            {
                foldersFiles.Items.Add("folder | " + item);
            }

            foreach (var item in _files.Keys)
            {
                foldersFiles.Items.Add(" file  | " + item);
            }
            btnCopy.IsEnabled = foldersFiles.SelectedItem != null;
            btnDelete.IsEnabled = foldersFiles.SelectedItem != null;
        }


        private void SetItemsInCurrentFolder(string pathFolder)
        {
            _logger.Info("Init MainWindow::SetItemsInCurrentFolder(string)");
            _folders.Clear();
            _files.Clear();

            foreach (var folder in GetFolders(pathFolder))
            {
                _folders[folder.Name] = folder;
            }

            foreach (var file in GetFiles(pathFolder))
            {
                _files[file.Name] = file;
            }
        }


        private List<MyFileDTO> GetFiles(string currentPath)
        {
            _logger.Info("Init MainWindow::GetFiles(string)");
            var items = AgentConnect.GetFilesCurrentDirrectory(
                new FileManagerInformatorAgentClient(new HttpClient()),
                currentPath);

            if (items.ex != null)
            {
                MessageBox.Show($"Ошибка при получении файлов - {items.ex.Message}", "Ошибка");
            }

            return items.Items;
        }


        private List<MyFolderDTO> GetFolders(string currentPath)
        {
            _logger.Info("Init MainWindow::GetFolders(string)");
            var items = new AllMyFoldersResponse();
            items = AgentConnect.GetFoldersCurrentDirrectory(
                new FileManagerInformatorAgentClient(new HttpClient()),
                currentPath);

            if (items.ex != null)
            {
                MessageBox.Show($"Ошибка при получении папок - {items.ex.Message}", "Error");
            }

            return items.Items;
        }


        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Init MainWindow::Rename_Click(object sender, RoutedEventArgs)");
            RenameWindow renameWindow = new RenameWindow();

            if (renameWindow.ShowDialog() == true)
            {
                string selectedItem = foldersFiles.SelectedItem.ToString();
                var result = AgentConnect.RenameItem(
                    new FileManagerChangerAgentClient(new HttpClient()),
                    currentPath.Text + '\\' + GetNameSelectedItem(selectedItem),
                    currentPath.Text + '\\' + renameWindow.newName.Text);
                if (result.ex != null)
                {
                    MessageBox.Show($"Ошибка периименования - {result.ex.Message}", "Ошибка");
                }
                PrintFilesFolders(currentPath.Text);
            }
        }


        private void foldersFiles_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            _logger.Info("Init MainWindow::foldersFiles_SelectionChanged_1(object sender, SelectionChangedEventArgs)");
            if (foldersFiles.SelectedItem == null)
            {
                return;
            }
            string nameSelectedItem = GetNameSelectedItem(foldersFiles.SelectedItem.ToString());
            btnRename.IsEnabled = foldersFiles.SelectedItem != null;

            if (foldersFiles.SelectedItem.ToString().Contains("folder"))
            {
                btnSize.IsEnabled = true;
                if (_folders.ContainsKey(nameSelectedItem))
                {
                    PrintInformationFolder(_folders[nameSelectedItem]);
                }
                else
                {
                    Item item = searchItems[nameSelectedItem][foldersFiles.SelectedIndex];
                    var attributes = new DirectoryInfo(item.FullPath).Attributes;
                    MyFolderDTO itemDto = new MyFolderDTO();
                    itemDto.FilesInFolder = 0;
                    itemDto.FoldersInFolder = 0;
                    itemDto.SizeFolders = 0;
                    itemDto.AttributesFolderProp.Hidden = attributes.HasFlag(FileAttributes.Hidden);
                    itemDto.AttributesFolderProp.ReadOnly = attributes.HasFlag(FileAttributes.ReadOnly);

                    PrintInformationFolder(itemDto);                    
                }
            }
            else
            {
                btnSize.IsEnabled = false;
                if (_files.ContainsKey(nameSelectedItem))
                {
                    PrintInformationFile(_files[nameSelectedItem]);
                }
                else
                {
                    Item item = searchItems[nameSelectedItem][foldersFiles.SelectedIndex];
                    var attributes = new DirectoryInfo(item.FullPath).Attributes;
                    MyFileDTO itemDto = new MyFileDTO();
                    itemDto.Name = item.Name;
                    itemDto.FullPath = item.FullPath;
                    itemDto.FolderOrFile = item.FolderOrFile;
                    itemDto.AttributesFile.Hidden = attributes.HasFlag(FileAttributes.Hidden);
                    itemDto.AttributesFile.ReadOnly = attributes.HasFlag(FileAttributes.ReadOnly);
                    itemDto.AttributesFile.Size = new FileInfo(item.FullPath).Length / 1000;

                    PrintInformationFile(itemDto);
                }
            }
            btnCopy.IsEnabled = foldersFiles.SelectedItem != null;
            btnDelete.IsEnabled = foldersFiles.SelectedItem != null;
        }


        private void PrintInformationFolder(MyFolderDTO folder)
        {
            _logger.Info("Init MainWindow::PrintInformationFolder(MyFolderDTO)");
            SetInformationFolder(GetInformationFolder(folder));
            SetAttributesFolder(folder);
        }


        private void SetInformationFolder(string informationFolder)
        {
            _logger.Info("Init MainWindow::SetInformationFolder(string)");
            tbItemInfo.Text = informationFolder;
        }


        private string GetInformationFolder(MyFolderDTO folder)
        {
            _logger.Info("Init MainWindow::GetInformationFolder(MyFolderDTO)");
            string informationFolder = "Свойства текущей папки\n";
            informationFolder += $"Name - {folder.Name}\n";

            return informationFolder;
        }


        private void SetAttributesFolder(MyFolderDTO folder)
        {
            _logger.Info("Init MainWindow::SetAttributesFolder(MyFolderDTO)");
            chbHidden.IsChecked = folder.AttributesFolderProp.Hidden;
            chbReadOnly.IsChecked = folder.AttributesFolderProp.ReadOnly;
        }


        private void PrintInformationFile(MyFileDTO file)
        {
            _logger.Info("Init MainWindow::PrintInformationFile(MyFileDTO)");
            SetInformationFile(GetInformationFile(file));
            SetAttributesFile(file);
        }


        private void SetInformationFile(string informationFile)
        {
            _logger.Info("Init MainWindow::SetInformationFile(string)");
            tbItemInfo.Clear();
            tbItemInfo.Text = informationFile;
        }


        private string GetInformationFile(MyFileDTO file)
        {
            _logger.Info("Init MainWindow::GetInformationFile(MyFileDTO)");
            string informationFile = "Свойства текущего файла\n";
            informationFile += $"Name - {file.Name}\n";
            informationFile += $"Size - {file.AttributesFile.Size} kb\n";

            if (file.AttributesFile.AttributesText.Chars > 0)
            {
                AttributesTextFile attr = file.AttributesFile.AttributesText;
                informationFile += $"Paragraphes - {attr.Paragraphes}\n" +
                    $"Words - {attr.Words}\n" +
                    $"Chars without spases - {attr.CharsWithoutSpace}\n" +
                    $"Chars - {attr.Chars}\n";
            }

            return informationFile;
        }


        private void SetAttributesFile(MyFileDTO file)
        {
            _logger.Info("Init MainWindow::SetAttributesFile(MyFileDTO)");
            chbHidden.IsChecked = file.AttributesFile.Hidden;
            chbReadOnly.IsChecked = file.AttributesFile.ReadOnly;
        }


        private string GetNameSelectedItem(string selectedItem)
        {
            _logger.Info("Init MainWindow::GetNameSelectedItem(string)");
            if (selectedItem.Contains("folder"))
            {
                return selectedItem.Remove(0, "folder | ".Length);
            }
            else
            {
                return selectedItem.Remove(0, " file  | ".Length);
            }
        }


        private void chbHidden_Checked(object sender, RoutedEventArgs e)
        {
            _logger.Info("Init MainWindow::chbHidden_Checked(object sender, RoutedEventArgs)");
            CheckBox chb = (CheckBox)sender;
            string nameSelectedItem = GetNameSelectedItem(foldersFiles.SelectedItem.ToString());
            if (foldersFiles.SelectedItem.ToString().Contains("folder"))
            {
                _folders[nameSelectedItem].AttributesFolderProp.Hidden = (bool)chb.IsChecked;
            }
            else
            {
                _files[nameSelectedItem].AttributesFile.Hidden = (bool)chb.IsChecked;
            }
        }

        private void chbReadOnly_Checked(object sender, RoutedEventArgs e)
        {
            _logger.Info("Init MainWindow::chbReadOnly_Checked(object sender, RoutedEventArgs)");
            CheckBox chb = (CheckBox)sender;
            string nameSelectedItem = GetNameSelectedItem(foldersFiles.SelectedItem.ToString());
            if (foldersFiles.SelectedItem.ToString().Contains("folder"))
            {
                _folders[nameSelectedItem].AttributesFolderProp.ReadOnly = (bool)chb.IsChecked;
            }
            else
            {
                _files[nameSelectedItem].AttributesFile.ReadOnly = (bool)chb.IsChecked;
            }
        }


        private void OnFoldersFiles_mouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _logger.Info("Init MainWindow::OnFoldersFiles_mouseDoubleClick(object sender, MouseButtonEventArgs)");
            string currentItemName = GetNameSelectedItem(((ListBox)sender).SelectedItem.ToString());
            if (_folders.ContainsKey(currentItemName))
            {
                currentPath.Text = _folders[currentItemName].FullPath;
                PrintFilesFolders(_folders[currentItemName].FullPath);
            }
            else if (searchItems.Count != 0 && searchItems.ContainsKey(currentItemName))
            {
                currentPath.Text = searchItems[currentItemName][foldersFiles.SelectedIndex].FullPath;
                PrintFilesFolders(searchItems[currentItemName][foldersFiles.SelectedIndex].FullPath);
                searchItems.Clear();
            }
            else
            {
                try
                {
                    Process proc = new Process();
                    proc.StartInfo.UseShellExecute = true;
                    proc.StartInfo.RedirectStandardOutput = false;
                    proc.StartInfo.FileName = _files.ContainsKey(currentItemName)
                        ? _files[currentItemName].FullPath
                        : searchItems[currentItemName][foldersFiles.SelectedIndex].FullPath;
                    proc.StartInfo.Arguments = "";
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    proc.Start();
                    proc.WaitForExit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка - " + ex.Message);
                }
            }
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Init MainWindow::btnBack_Click(object sender, RoutedEventArgs)");
            PrintFilesFolders(Path.GetFullPath(currentPath.Text + "\\.."));
        }


        private void cmbDrives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _logger.Info("Init MainWindow::cmbDrives_SelectionChanged(object sender, SelectionChangedEventArgs)");
            currentPath.Text = cmbDrives.SelectedItem.ToString();
            PrintFilesFolders(currentPath.Text);
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Init MainWindow::btnCopy_Click(object sender, RoutedEventArgs)");
            btnPaste.IsEnabled = foldersFiles.SelectedItem != null;
            _pathFromCopy = currentPath.Text + '\\'
                + GetNameSelectedItem(foldersFiles.SelectedItem.ToString());
        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Init MainWindow::btnPaste_Click(object sender, RoutedEventArgs)");
            var result = AgentConnect.PostItemCopy(
                new FileManagerChangerAgentClient(new HttpClient()),
                _pathFromCopy,
                currentPath.Text + '\\' + Path.GetFileName(_pathFromCopy)
                );
            if (result.ex != null)
            {
                MessageBox.Show($"Ошибка вставки - {result.ex.Message}", "Ошибка");
            }

            PrintFilesFolders(Path.GetFullPath(currentPath.Text));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Init MainWindow::btnDelete_Click(object sender, RoutedEventArgs)");
            var result = AgentConnect.PostItemDelete(
                new FileManagerChangerAgentClient(new HttpClient()),
                currentPath.Text + '\\'
                + GetNameSelectedItem(foldersFiles.SelectedItem.ToString()));
            if (result.ex != null)
            {
                MessageBox.Show($"Ошибка удаления - {result.ex.Message}", "Ошибка");
            }
            PrintFilesFolders(currentPath.Text);
            btnDelete.IsEnabled = foldersFiles.SelectedItem != null;
        }


        private void txtbFinde_GotFocus(object sender, RoutedEventArgs e)
        {
            _logger.Info("Init MainWindow::txtbFinde_GotFocus(object sender, RoutedEventArgs)");
            tbSearch.Text = "";
        }


        private void btnFinde_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Init MainWindow::btnFinde_Click(object sender, RoutedEventArgs)");
            ALLItemsResponse items = AgentConnect.GetSearchItem(
                new FileManagerInformatorAgentClient(new HttpClient()),
                currentPath.Text,
                tbSearch.Text);

            if (items.Items.Count != 0)
            {_logger.Info("Init MainWindow::SetDrive()");
                foldersFiles.Items.Clear();
                searchItems.Clear();
                foreach (var item in items.Items)
                {
                    if (!searchItems.ContainsKey(item.Name))
                    {
                        searchItems[item.Name] = new List<Item>();
                    }
                    searchItems[item.Name].Add(item);
                }

                foreach (string name in searchItems.Keys)
                {
                    foreach (var item in searchItems[name])
                    {
                        string addItem = item.FolderOrFile == TypeItem.Folder ?
                        "folder | " + name : " file  | " + name;
                        foldersFiles.Items.Add(addItem);
                    }
                }

                btnCopy.IsEnabled = foldersFiles.SelectedItem != null;
                btnDelete.IsEnabled = foldersFiles.SelectedItem != null;
            }
            else
            {
                MessageBox.Show("Ничего не найдено");
            }
        }


        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Init MainWindow::btnCreate_Click(object sender, RoutedEventArgs)");
            CreateWindow createWindow = new CreateWindow();

            if (createWindow.ShowDialog() == true)
            {
                if (string.IsNullOrEmpty(createWindow.NewName))
                {
                    MessageBox.Show("Ничего не введено");
                    return;
                }
                TypeItem typeNewItem = (bool)createWindow.rbFolder.IsChecked ? TypeItem.Folder : TypeItem.File;
                var result = AgentConnect.PostItemCreate(
                new FileManagerChangerAgentClient(new HttpClient()),
                currentPath.Text + '\\'
                + createWindow.NewName,
                typeNewItem);
                if (result.ex != null)
                {
                    MessageBox.Show($"Ошибка создания - {result.ex.Message}", "Ошибка");
                }
                PrintFilesFolders(currentPath.Text);
                btnDelete.IsEnabled = foldersFiles.SelectedItem != null;
            }
        }


        private void Size_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Init MainWindow::Size_Click(object sender, RoutedEventArgs)");
            string pathSelectedItem = currentPath.Text
                + GetNameSelectedItem(foldersFiles.SelectedItem.ToString());
            MyFolder folder = new MyFolder(pathSelectedItem);
            folder.SizeFolder(pathSelectedItem);

            MessageBox.Show(
                $"Всего папок - {folder.FoldersInFolder}\n\n"
                + $"Всего файлов - {folder.FilesInFolder}\n\n"
                + $"Размер - {(folder.SizeFolders / 1000).ToString("### ### ### ### ###")} kb",
                $"Свойства папки - {folder.Name}"
                ); 
        }
    }
}
