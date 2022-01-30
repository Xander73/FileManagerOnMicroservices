using Core.Models.Responses;
using FileManagerInformator.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileManagerInformator.Tests.Controllers
{
    public class FileManagerInformatorControllerTests
    {
        private FileManagerInformatorController fmi;
        private Mock<ILogger<FileManagerInformatorController>> mock;
        string pathTestFolder = Directory.GetCurrentDirectory() + "\\TestDirectory";

        public FileManagerInformatorControllerTests()
        {
            mock = new Mock<ILogger<FileManagerInformatorController>>();
            fmi = new FileManagerInformatorController(mock.Object);
        }


        [Fact]
        public void GetDrivers_ExistDirectoriesReturned()
        {
            var expected = Directory.GetLogicalDrives();
            Task<IActionResult> action = fmi.GetDrivers();
            OkObjectResult result = action.Result as OkObjectResult;
            AllDrivesResponse value = result.Value as AllDrivesResponse;
            
            string[] actual = value.Drives.ToArray();

            Assert.True(Enumerable.SequenceEqual(expected, actual));
        }


        [Fact]
        public void PostFolders_ListOfPathsReturned()
        {
            string path = pathTestFolder + "\\PostFolder";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path + "\\testPostFolder");
            }            
            string expected = path +  "\\testPostFolder";

            var directoryes = fmi.PostFolders(path).Result as OkObjectResult;
            AllMyFoldersResponse actual = directoryes.Value as AllMyFoldersResponse;

            Assert.Equal(expected, actual.Items[0].FullPath);
        }


        [Fact]
        public void PostFolders_nullPath_ArgumentNullExceptionReturned()
        {
            try
            {
                var directoryes = fmi.PostFolders(null).Result as OkObjectResult;
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void PostFolders_WrongPathPath_DirectoryNotFoundExceptionReturned()
        {
            try
            {
                var directoryes = fmi.PostFolders("\\wrongPath!(@*").Result as OkObjectResult;
            }
            catch (Exception e)
            {
                Assert.IsType<DirectoryNotFoundException>(e);
            }
        }


        [Fact]
        public void PostFiles_ListOfPathsReturned()
        {
            string path = pathTestFolder + "\\PostFolder";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path + "\\testPostFolder");
            }
            string expected = path + "\\testPostFile.txt";
            if (!File.Exists(expected))
            {
                File.Create(expected);
            }            

            var files = fmi.PostFiles(path).Result as OkObjectResult;
            AllMyFilesResponse actual = files.Value as AllMyFilesResponse;

            Assert.Equal(expected, actual.Items[0].FullPath);
        }


        [Fact]
        public void PostFiles_nullArgument_NullReferenceExceptionReturned()
        {
            try
            {
                var files = fmi.PostFiles(null).Result as OkObjectResult;
            }
            catch (Exception e)
            {
                Assert.IsType<NullReferenceException>(e);
            }
        }


        [Fact]
        public void PostFiles_WrongPath_FileNotFoundExceptionReturned()
        {
            try
            {
                var files = fmi.PostFiles("\\wrongPath!)(%#").Result as OkObjectResult;
            }
            catch (Exception e)
            {
                Assert.IsType<FileNotFoundException>(e);
            }
        }


        [Fact]
        public void Size_Folder_NumberFoldersFilesSizeInByteshReturned()
        {
            string path = pathTestFolder + "\\SizeFolder";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string pathFile = path + "\\testPostFile.txt";
            if (!File.Exists(pathFile))
            {
                using (File.Create(pathFile)) { }
            }
            using (StreamWriter sw = new StreamWriter(pathFile, false, Encoding.Default))
            {
                sw.WriteLine("Текст для размера файла");
            }
            string expected = $"FoldersInFolder - 0\n"
                    + $"FilesInFolder - 1\n"
                    + $"Size folder - 45\n";
            string actual = fmi.Size(path).Size;

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Size_nullArgument_ArgumentNullExceptionReturned()
        {
            try
            {
                string actual = fmi.Size(null).Size;
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void Size_WrongPathArgument_SizeInByteshReturned()
        {
            string path = pathTestFolder + "\\SizeFolder";
            string pathFile = path + "\\testPostFile.txt";

            if (!File.Exists(pathFile))
            {
                using (File.Create(pathFile)) { }
            }
            try
            {
                string actual = fmi.Size("wrongPath)(*&^%").Size;
            }
            catch (Exception e)
            {
                Assert.IsType<DirectoryNotFoundException>(e);
            }
        }


        [Fact]
        public void Size_FilePathArgument_DirectoryNotFoundExceptionReturned()
        {
            try
            {
                string actual = fmi.Size("wrongPath)(*&^%").Size;
            }
            catch (Exception e)
            {
                Assert.IsType<DirectoryNotFoundException>(e);
            }
        }


        [Fact]
        public void PostSearchItems_serchFile_ArrayDirectoriesReturned()
        {
            string path = pathTestFolder + "\\SearchFolder";
            string pathFile = path + "\\testSearchFile.txt";
            string expected = pathFile;
            ;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(pathFile))
            {
                using (File.Create(pathFile)) { }
            }
            var paths = fmi.PostSearchItems(pathTestFolder, "testSearchFile").Result as OkObjectResult;

            ALLItemsResponse actual = paths.Value as ALLItemsResponse;

            Assert.Equal(expected, actual.Items[0].FullPath);
        }


        [Fact]
        public void PostSearchItems_serchFolder_ArrayDirectoriesReturned()
        {
            string path = pathTestFolder + "\\SearchFolder";
            string expected = path;
            ;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var paths = fmi.PostSearchItems(pathTestFolder, "SearchFolder").Result as OkObjectResult;

            ALLItemsResponse actual = paths.Value as ALLItemsResponse;

            Assert.Equal(expected, actual.Items[0].FullPath);
        }


        [Fact]
        public void PostSearchItems_notFoundItem_ArrayCount0Returned()
        {
            string path = pathTestFolder + "\\SearchFolder";
            string expected = path;
            ;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var paths = fmi.PostSearchItems(pathTestFolder, "notFounditem)(*").Result as OkObjectResult;

            ALLItemsResponse actual = paths.Value as ALLItemsResponse;

            Assert.Equal(0, actual.Items.Count);
        }


        [Fact]
        public void PostSearchItems_nullArgument_ArgumentNullExceptionReturned()
        {
            try
            {
                var paths = fmi.PostSearchItems(pathTestFolder, "SearchFolder");
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void PostSearchItems_nullArgument_DirectoryNotFoundExceptionReturned()
        {
            try
            {
                var paths = fmi.PostSearchItems("WrongPatn)(*&", "SearchFolder");
            }
            catch (Exception e)
            {
                Assert.IsType<DirectoryNotFoundException>(e);
            }
        }
    }
}
