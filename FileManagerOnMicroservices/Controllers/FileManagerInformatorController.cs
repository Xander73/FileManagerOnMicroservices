

using Core;
using Core.Interfaces;
using FileManagerClient.Agent.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.IO;

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
            _logger.LogInformation("FileManagerInformator::Index() was start");
            string drives = "";
            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                drives += d.Name + " - " + d.DriveType + '\n';
            }
                        
            return Ok(drives);
        }


        [HttpGet("drives")]
        public async Task<IActionResult> GetDrivers()
        {
            _logger.LogInformation("FileManagerInformator::GetDrivers() was start");
            AllDrivesResponse drives = new();
            DriveInfo[] drivesInfo = await Task.Run(() => DriveInfo.GetDrives());

            foreach (DriveInfo d in drivesInfo)
            {
                drives.Drives.Add(d.Name + " - " + d.DriveType + '\n');
            }
            return Ok(drives);
        }


        [HttpPost("directory")]
        public async Task<IActionResult> PostFoldersFiles(string newPath)
        {
            _logger.LogInformation("FileManagerInformator::GetFiles() was start");
            List<IItem> objectsCurrentDirrectory = new();

            string[] folders = await Task.Run(() => Directory.GetDirectories(newPath));
            foreach (string folder in folders)
            {
                IItem itemFolder = new Item(); 
                itemFolder.Path = folder + '\n';
                itemFolder.IsFolderOrFile = TypeItem.Folder;
                objectsCurrentDirrectory.Add(itemFolder);
            }

            string[] files = await Task.Run(() => Directory.GetFiles(newPath));
            foreach (string file in files)
            {
                IItem itemFile = new Item();
                itemFile.Path = file + '\n';
                itemFile.IsFolderOrFile = TypeItem.Folder;
                objectsCurrentDirrectory.Add(itemFile);
            }

            return Ok(objectsCurrentDirrectory);
        }


        [HttpPost("size")]
        public async Task<IActionResult> PostSize(IItem pathItem)
        {
            switch(pathItem.IsFolderOrFile)
            {
                case TypeItem.Folder:
                    {
                        MyFolder myFolder = new MyFolder(pathItem.Path);
                        return Ok(myFolder.SizeFolder);
                    }

                case TypeItem.File:
                    {
                        MyFile myFile = new MyFile(pathItem.Path);                        
                        return Ok(myFile.SizeFile());
                    }

                default: throw new ArgumentException(nameof(pathItem));
            }
        }
    }
}
