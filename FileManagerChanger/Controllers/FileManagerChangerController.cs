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
                try
                {
                    myFolder.CopyFolder(pathOldItem, pathNewItem);
                }
                catch (Exception)
                {
                    _logger.LogError(nameof(PostCopy), myFolder);
                }
            }
            else
            {
                MyFile myFile = new MyFile(pathOldItem);
                try
                {
                    myFile.CopyFile(pathOldItem, pathNewItem);
                }
                catch (Exception)
                {
                    _logger.LogError(nameof(PostCopy), myFile);
                }
            }
        }


        [HttpPost("delete")]
        public void PostDelete(string pathDelete)
        {
            _logger.LogInformation("FileManagerChanger::PostDelete(string) was start");
            if (Directory.Exists(pathDelete))
            {
                MyFolder myFolder = new MyFolder(pathDelete);
                try
                {
                    myFolder.DeleteFolder(pathDelete);
                }
                catch (Exception)
                {
                    _logger.LogError(nameof(pathDelete), myFolder);
                }
                
            }
            else
            {
                MyFile myFile = new MyFile(pathDelete);
                try
                {
                    myFile.DeleteFile(pathDelete);
                }
                catch (Exception)
                {
                    _logger.LogError(nameof(pathDelete), myFile);
                }
            }
        }


        [HttpPost("rename")]
        public void PostRename(string pathOldItem, string newNameItem)
        {
            _logger.LogInformation("FileManagerChanger::PostRename(PostRename(string, string)) was start");
            if (Directory.Exists(pathOldItem))
            {
                MyFolder myFolder = new MyFolder(pathOldItem);
                try
                {

                    myFolder.RenameFolder(pathOldItem, newNameItem);
                }
                catch (Exception)
                {

                    _logger.LogError(nameof(PostRename), myFolder);
                }
            }
            else
            {
                MyFile myFile = new MyFile(pathOldItem);
                try
                {
                    myFile.RenameFile(pathOldItem, newNameItem);
                }
                catch (Exception)
                {
                    _logger.LogError(nameof(PostRename), myFile);
                }
                
            }
        }


        [HttpPost("create")]
        public void PostCreate(string pathNewItem, TypeItem typeItem)
        {
            _logger.LogInformation("FileManagerChanger::PostCreate(string, string) was start");

            if (typeItem == TypeItem.Folder)
            {
                MyFolder myFolder = new MyFolder(pathNewItem);
                try
                {
                    myFolder.CreateFolder(pathNewItem);
                }
                catch (Exception)
                {

                    _logger.LogError(nameof(PostCreate), myFolder);
                }
                
            }
            else
            {
                MyFile myFile = new MyFile(pathNewItem);
                try
                {
                    myFile.CreateFile(pathNewItem);
                }
                catch (Exception)
                {
                    _logger.LogError(nameof(PostCreate), myFile);
                }
            }
        }
    }
}
