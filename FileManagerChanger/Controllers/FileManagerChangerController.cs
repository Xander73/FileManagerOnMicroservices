using Core;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileManagerChanger.Controllers
{
    [ApiController]
    [Route("api/filemanagerchanger")]
    public class FileManagerChangerController : ControllerBase
    {
        private ILogger<FileManagerChangerController> _logger;


        public FileManagerChangerController(ILogger<FileManagerChangerController> logger)
        {
            _logger = logger;
        }


        [HttpGet("index")]
        public IActionResult Index ()
        {
            _logger.LogInformation("FileManagerChanger::Index() was start");
            return Ok("Colls FileManagerChanger");
        }


        [HttpPost("copy")]
        public void PostCopy(string pathOldItem, string pathNewItem)
        {
            _logger.LogInformation("FileManagerChanger::PostCopy(string, string) was start");
            if (Directory.Exists(pathOldItem))
            {
                MyFolder myFolder = new MyFolder(pathOldItem);
                myFolder.CopyFolder(pathOldItem, pathNewItem);
            }
            else
            {
                MyFile myFile = new MyFile(pathOldItem);
                myFile.CopyFile(pathOldItem, pathNewItem);
            }
        }


        [HttpPost("delete")]
        public void PostDelete(string pathDelete)
        {
            _logger.LogInformation("FileManagerChanger::PostDelete(string) was start");
            if (Directory.Exists(pathDelete))
            {
                MyFolder myFolder = new MyFolder(pathDelete);
                myFolder.DeleteFolder(pathDelete);
            }
            else
            {
                MyFile myFile = new MyFile(pathDelete);
                myFile.DeleteFile(pathDelete);
            }
        }


        [HttpPost("rename")]
        public void PostRename(string pathOldItem, string newNameItem)
        {
            _logger.LogInformation("FileManagerChanger::PostRename(PostRename(string, string)) was start");
            if (Directory.Exists(pathOldItem))
            {
                MyFolder myFolder = new MyFolder(pathOldItem);
                myFolder.RenameFolder(pathOldItem, newNameItem);
            }
            else
            {
                MyFile myFile = new MyFile(pathOldItem);
                myFile.RenameFile(pathOldItem, newNameItem);
            }
        }


        [HttpPost("create")]
        public void PostCreate(string pathNewItem, TypeItem typeItem)
        {
            _logger.LogInformation("FileManagerChanger::PostCreate(string, string) was start");

            if (typeItem == TypeItem.Folder)
            {
                MyFolder myFolder = new MyFolder(pathNewItem);
                myFolder.CreateFolder(pathNewItem);
            }
            else
            {
                MyFile myFile = new MyFile(pathNewItem);
                myFile.CreateFile(pathNewItem);
            }
        }
    }
}
