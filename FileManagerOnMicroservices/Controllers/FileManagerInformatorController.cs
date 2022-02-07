using Core;
using Core.Interfaces;
using Core.Models.DTO;
using Core.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FileManagerInformator.Controllers
{
    [ApiController]
    [Route("api/filemanagerinformator")]
    public class FileManagerInformatorController : Controller
    {
        private ILogger<FileManagerInformatorController> _logger;

        public FileManagerInformatorController(ILogger<FileManagerInformatorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return Ok("Colls FileManagerInformator");
        }


        [HttpGet("drives")]
        public async Task<IActionResult> GetDrivers()
        {
            _logger.LogInformation("FileManagerInformator::GetDrivers() was start");
            AllDrivesResponse drives = new();
            try
            {
                DriveInfo[] drivesInfo = await Task.Run(() => DriveInfo.GetDrives());
                foreach (DriveInfo d in drivesInfo)
                {
                    drives.Drives.Add(d.Name);
                }
                return Ok(drives);
            }
            catch (Exception e)
            {
                drives.ex = e;
                _logger?.LogError(nameof(GetDrivers));
            }
            return Ok(new AllDrivesResponse());
        }


        [HttpPost("directories")]
        public async Task<IActionResult> PostFolders(string requiredDirectory)
        {
            _logger.LogInformation("FileManagerInformator::PostFolders(string) was start");
            AllMyFoldersResponse objectsCurrentDirectory = new();

            try
            {
                string[] folders = await Task.Run(() => Directory.GetDirectories(@requiredDirectory));
                foreach (string folder in folders)
                {
                    objectsCurrentDirectory.Items.Add(BuildFolderDTO(folder));
                }
                objectsCurrentDirectory.ex = new DirectoryNotFoundException();
                return Ok(objectsCurrentDirectory);
            }
            catch (Exception e)
            {
                objectsCurrentDirectory.ex = e;
                _logger.LogError(nameof(PostFolders));
            }
            return Ok(new AllMyFoldersResponse());
        }


        private MyFolderDTO BuildFolderDTO(string pathFolder)
        {
            MyFolderDTO itemFolder = new MyFolderDTO();
            MyFolder myFolder = new MyFolder(pathFolder);
            itemFolder.Name = myFolder.Name;
            itemFolder.FullPath = myFolder.FullPath;
            itemFolder.FolderOrFile = TypeItem.Folder;
            itemFolder.SizeFolders = myFolder.SizeFolders;
            itemFolder.FilesInFolder = myFolder.FilesInFolder;
            itemFolder.FoldersInFolder = myFolder.FoldersInFolder;

            DirectoryInfo dirInfo = new DirectoryInfo(pathFolder);
            var attributes = dirInfo.Attributes;
            itemFolder.AttributesFolderProp.Hidden = attributes.HasFlag(FileAttributes.Hidden);
            itemFolder.AttributesFolderProp.ReadOnly = attributes.HasFlag(FileAttributes.ReadOnly);

            return itemFolder;
        }


        [HttpPost("files")]
        public async Task<IActionResult> PostFiles(string requiredDirectory)
        {
            _logger.LogInformation("FileManagerInformator::PostFiles(string) was start");
            AllMyFilesResponse filesCurrentDirectory = new();
            try
            {
                string[] files = await Task.Run(() => Directory.GetFiles(requiredDirectory));
                foreach (string file in files)
                {
                    filesCurrentDirectory.Items.Add(BuildFileDTO(file));
                }
                return Ok(filesCurrentDirectory);
            }
            catch (Exception e)
            {
                filesCurrentDirectory.ex = e;
                _logger.LogError(nameof(PostFiles));
            }
            return Ok(new AllMyFilesResponse());

        }


        private MyFileDTO BuildFileDTO(string pathFile)
        {
            MyFileDTO itemFile = new MyFileDTO();
            MyFile myFile = new MyFile(pathFile);
            itemFile.Name = myFile.Name;
            itemFile.FullPath = myFile.FullPath;
            itemFile.FolderOrFile = TypeItem.File;
            itemFile.AttributesFile.Size = myFile.SizeFile();

            FileInfo fileInfo = new FileInfo(pathFile);
            var attributes = fileInfo.Attributes;
            itemFile.AttributesFile.Hidden = attributes.HasFlag(FileAttributes.Hidden);
            itemFile.AttributesFile.ReadOnly = attributes.HasFlag(FileAttributes.ReadOnly);

            if (myFile.IsTextFile())
            {
                Dictionary<string, int> textAttributes = myFile.TextFileInformmation();

                itemFile.AttributesFile.AttributesText.Words = textAttributes["Words"];
                itemFile.AttributesFile.AttributesText.Paragraphes = textAttributes["Paragraphes"];
                itemFile.AttributesFile.AttributesText.Words = textAttributes["Words"];
                itemFile.AttributesFile.AttributesText.Chars = textAttributes["Chars"];
                itemFile.AttributesFile.AttributesText.CharsWithoutSpace = textAttributes["CharsWithoutSpace"];
            }

            return itemFile;
        }


        [HttpPost("size")]
        public SizeResponse Size(string pathItem)
        {
            _logger.LogInformation("FileManagerInformator::PostSize(string) was start");
            MyFolder myFolder = new MyFolder(pathItem);
            SizeResponse sr = new SizeResponse();
            try
            {
                myFolder.SizeFolder(pathItem);
                sr.Size = $"FoldersInFolder - {myFolder.FoldersInFolder}\n"
                    + $"FilesInFolder - {myFolder.FilesInFolder}\n"
                    + $"Size folder - {myFolder.SizeFolders}\n";
            }
            catch (Exception e)
            {
                sr.ex = e;
                _logger.LogError(nameof(Size), myFolder);
            }
            
            return sr;
        }


        [HttpPost("search")]
        public async Task<IActionResult> PostSearchItems(string pathFolder, string searchNameItem)
        {
            _logger.LogInformation("FileManagerInformator::PostSearchItems(string, string) was start");
            ALLItemsResponse response = new ALLItemsResponse();

            try
            {
                await Task.Run(() => response.Items.AddRange(
                   MyFolder.Search(pathFolder, searchNameItem)));
            }
            catch (Exception e)
            {
                response.ex = e;
                _logger.LogError(nameof(PostSearchItems), response);
            }

            return Ok(response);
        }
    }
}
