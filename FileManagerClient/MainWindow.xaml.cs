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
using System.Threading.Tasks;
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
        private Dictionary<string, MyFileDTO> _files;
        private Dictionary<string, MyFolderDTO> _folders;
        private Dictionary<string, List<Item>> searchItems;
        private string _pathFromCopy = "";
        public MainWindow()
        {
            InitializeComponent();
            InitiliazeMainWindow();
            currentPath.Text = cmbDrives.Text;
        }


        public void InitiliazeMainWindow()
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


        public List<string> GetDrives()
        {
            var drives = AgentConnect.GetAllDrives(new FileManagerInformatorAgentClient(
                new HttpClient()));

            return drives.Drives;
        }


        public void SetDrive()
        {
            var drives = GetDrives();
            cmbDrives.ItemsSource = drives;
            cmbDrives.SelectedItem = drives[0];
        }


        public List<string> GetDrives(object sender, MouseEventArgs e)
        {
            var drives = AgentConnect.GetAllDrives(new FileManagerInformatorAgentClient(
                new HttpClient()));

            return drives.Drives;
        }


        public void PrintFilesFolders(string requiredFolder)
        {
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


        public void SetItemsInCurrentFolder(string pathFolder)
        {
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


        public List<MyFileDTO> GetFiles(string currentPath)
        {
            var items = AgentConnect.GetFilesCurrentDirrectory(
                new FileManagerInformatorAgentClient(new HttpClient()),
                currentPath);

            return items.Items;
        }


        public List<MyFolderDTO> GetFolders(string currentPath)
        {
            var items = AgentConnect.GetFoldersCurrentDirrectory(
                new FileManagerInformatorAgentClient(new HttpClient()),
                currentPath);

            return items.Items;
        }


        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            RenameWindow renameWindow = new RenameWindow();

            if (renameWindow.ShowDialog() == true)
            {
                string selectedItem = foldersFiles.SelectedItem.ToString();
                AgentConnect.RenameItem(
                    new FileManagerChangerAgentClient(new HttpClient()),
                    currentPath.Text + '\\' + GetNameSelectedItem(selectedItem),
                    currentPath.Text + '\\' + renameWindow.newName.Text);
                PrintFilesFolders(currentPath.Text);
            }
        }


        private void foldersFiles_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            if (foldersFiles.SelectedItem == null)
            {
                return;
            }
            string nameSelectedItem = GetNameSelectedItem(foldersFiles.SelectedItem.ToString());
            btnRename.IsEnabled = foldersFiles.SelectedItem != null;

            if (foldersFiles.SelectedItem.ToString().Contains("folder"))
            {
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

        
        public void PrintInformationFolder(MyFolderDTO folder)
        {
            SetInformationFolder(GetInformationFolder(folder));
            SetAttributesFolder(folder);
        }


        public void SetInformationFolder(string informationFolder)
        {
            tbItemInfo.Text = informationFolder;
        }


        public string GetInformationFolder(MyFolderDTO folder)
        {
            string informationFolder = "Свойства текущей папки\n";
            informationFolder += $"Name - {folder.Name}\n";

            return informationFolder;
        }


        public void SetAttributesFolder(MyFolderDTO folder)
        {
            chbHidden.IsChecked = folder.AttributesFolderProp.Hidden;
            chbReadOnly.IsChecked = folder.AttributesFolderProp.ReadOnly;
        }


        public void PrintInformationFile(MyFileDTO file)
        {
            SetInformationFile(GetInformationFile(file));
            SetAttributesFile(file);
        }


        public void SetInformationFile(string informationFile)
        {
            tbItemInfo.Clear();
            tbItemInfo.Text = informationFile;
        }


        public string GetInformationFile(MyFileDTO file)
        {
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


        public void SetAttributesFile(MyFileDTO file)
        {
            chbHidden.IsChecked = file.AttributesFile.Hidden;
            chbReadOnly.IsChecked = file.AttributesFile.ReadOnly;
        }


        public string GetNameSelectedItem(string selectedItem)
        {
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
            PrintFilesFolders(Path.GetFullPath(currentPath.Text + "\\.."));
        }


        private void cmbDrives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPath.Text = cmbDrives.SelectedItem.ToString();
            PrintFilesFolders(currentPath.Text);
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            btnPaste.IsEnabled = foldersFiles.SelectedItem != null;
            _pathFromCopy = currentPath.Text + '\\'
                + GetNameSelectedItem(foldersFiles.SelectedItem.ToString());
        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            AgentConnect.PostItemCopy(
                new FileManagerChangerAgentClient(new HttpClient()),
                _pathFromCopy,
                currentPath.Text + '\\' + Path.GetFileName(_pathFromCopy)
                );

            PrintFilesFolders(Path.GetFullPath(currentPath.Text));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            AgentConnect.PostItemDelete(
                new FileManagerChangerAgentClient(new HttpClient()),
                currentPath.Text + '\\'
                + GetNameSelectedItem(foldersFiles.SelectedItem.ToString()));
            PrintFilesFolders(currentPath.Text);
            btnDelete.IsEnabled = foldersFiles.SelectedItem != null;
        }


        private void txtbFinde_GotFocus(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = "";
        }


        private void btnFinde_Click(object sender, RoutedEventArgs e)
        {
            ALLItemsResponse items = AgentConnect.GetSearchItem(
                new FileManagerInformatorAgentClient(new HttpClient()),
                currentPath.Text,
                tbSearch.Text);

            if (items.Items.Count != 0)
            {
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
            CreateWindow createWindow = new CreateWindow();

            if (createWindow.ShowDialog() == true)
            {
                if (string.IsNullOrEmpty(createWindow.NewName))
                {
                    MessageBox.Show("Ничего не введено");
                    return;
                }
                TypeItem typeNewItem = (bool)createWindow.rbFolder.IsChecked ? TypeItem.Folder : TypeItem.File;
                AgentConnect.PostItemCreate(
                new FileManagerChangerAgentClient(new HttpClient()),
                currentPath.Text + '\\'
                + createWindow.NewName,
                typeNewItem);
                PrintFilesFolders(currentPath.Text);
                btnDelete.IsEnabled = foldersFiles.SelectedItem != null;
            }
        }


        private void Size_Click(object sender, RoutedEventArgs e)
        {
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
