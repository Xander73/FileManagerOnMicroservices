using Core.Interfaces;
using FileManagerChanger.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using Xunit;

namespace FileManagerChanger.Tests.Controllers
{
    public class FileManagerChangerControllerTests
    {
        private FileManagerChangerController fmc;
        private Mock<ILogger<FileManagerChangerController>> mock;
        string pathTestFolder = Directory.GetCurrentDirectory() + "\\TestDirectory";


        public FileManagerChangerControllerTests()
        {
            mock = new Mock<ILogger<FileManagerChangerController>>();
            fmc = new FileManagerChangerController(mock.Object);
        }


        [Fact]
        public void CopyFolder_CopyFolder_TrueReturned()
        {
            string oldFolder = pathTestFolder + "\\CopyFolderController";
            if (!Directory.Exists(oldFolder))
            {
                Directory.CreateDirectory(oldFolder);
                Directory.CreateDirectory(oldFolder + "\\DirectoryToCopy");
            }
            if (!Directory.Exists(oldFolder + "\\DirectoryToCopy"))
            {
                Directory.CreateDirectory(oldFolder + "\\DirectoryToCopy");
            }
            string newFolder = pathTestFolder + "\\CopyedFolder";
            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }

            fmc.PostCopy(oldFolder, newFolder);
                        
            Assert.True(Directory.Exists(newFolder + "\\DirectoryToCopy"));
        }


        [Fact]
        public void CopyFolder_OldFolderNull_ArgumentNullExceptionReturned()
        {
            string newFolder = pathTestFolder + "\\CopyedFolder";
            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }

            try
            {
                fmc.PostCopy(null, newFolder);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void CopyFolder_NewFolderNull_ArgumentNullExceptionReturned()
        {
            string oldFolder = pathTestFolder + "\\CopyFolderController";
            if (!Directory.Exists(oldFolder))
            {
                Directory.CreateDirectory(oldFolder);
            }

            try
            {
                fmc.PostCopy(oldFolder, null);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void CopyFolder_OldFolderWrongPath_DirectoryNotFoundExceptionReturned()
        {
            string newFolder = pathTestFolder + "\\CopyedFolder";
            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }

            try
            {
                fmc.PostCopy("wrongPath", newFolder);
            }
            catch (Exception e)
            {
                Assert.IsType<DirectoryNotFoundException>(e);
            }
        }


        [Fact]
        public void DeleteFolder_FolderToDelete_FelseReturned()
        {
            string nameFolder = pathTestFolder + "\\FolderToDelete";
            Directory.CreateDirectory(nameFolder);

            fmc.PostDelete(nameFolder);

            Assert.False(Directory.Exists(nameFolder));
        }


        [Fact]
        public void DeleteFolder_NotExistFolder_DirectoryNotFoundExceptionReturned()
        {
            string nameFolder = pathTestFolder + "\\FolderToDelete";

            try
            {
                fmc.PostDelete(nameFolder);
            }
            catch (Exception e)
            {
                Assert.IsType<DirectoryNotFoundException>(e);
            }

        }


        [Fact]
        public void DeleteFolder_nullFolderToDelete_ArgumentNullExceptionReturned()
        {
            string nameFolder = pathTestFolder + "\\FolderToDelete";
            try
            {
                fmc.PostDelete(null);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void RenameFolder_FolderToRename_FalseReturned()
        {
            string oldFolder = pathTestFolder + "\\OldFolder";
            string newFolder = pathTestFolder + "\\RenamedFolder";
            if (!Directory.Exists(oldFolder))
            {
                Directory.CreateDirectory(oldFolder);
            }
            if (Directory.Exists(newFolder))
            {
                Directory.Delete(newFolder);
            }
            fmc.PostRename(oldFolder, newFolder);

            Assert.True(Directory.Exists(newFolder));
        }


        [Fact]
        public void RenameFolder_NullNewFolderToRename_ArgumentNullExceptionReturned()
        {
            string oldFolder = pathTestFolder + "\\OldFolder";
            string newFolder = pathTestFolder + "\\RenamedFolder";
            if (!Directory.Exists(oldFolder))
            {
                Directory.CreateDirectory(oldFolder);
            }
            if (Directory.Exists(newFolder))
            {
                Directory.Delete(newFolder);
            }
            try
            {
                fmc.PostRename(oldFolder, null);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void RenameFolder_NullOldFolderToRename_ArgumentNullExceptionReturned()
        {
            string oldFolder = pathTestFolder + "\\OldFolder";
            string newFolder = pathTestFolder + "\\RenamedFolder";
            if (!Directory.Exists(oldFolder))
            {
                Directory.CreateDirectory(oldFolder);
            }
            if (Directory.Exists(newFolder))
            {
                Directory.Delete(newFolder);
            }
            try
            {
                fmc.PostRename(null, newFolder);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void RenameFolder_NullBouthFolderToRename_ArgumentNullExceptionReturned()
        {
            string oldFolder = pathTestFolder + "\\OldFolder";
            string newFolder = pathTestFolder + "\\RenamedFolder";
            if (!Directory.Exists(oldFolder))
            {
                Directory.CreateDirectory(oldFolder);
            }
            if (Directory.Exists(newFolder))
            {
                Directory.Delete(newFolder);
            }
            try
            {
                fmc.PostRename(null, null);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void RenameFolder_WrongPathFolderToRename_DirectoryNotFoundExceptionReturned()
        {
            string oldFolder = pathTestFolder + "\\OldFolder";
            string wrongOldFolder = oldFolder + "mistake";
            string newFolder = pathTestFolder + "\\RenamedFolder";
            if (!Directory.Exists(oldFolder))
            {
                Directory.CreateDirectory(oldFolder);
            }
            if (Directory.Exists(newFolder))
            {
                Directory.Delete(newFolder);
            }
            try
            {
                fmc.PostRename(wrongOldFolder, newFolder);
            }
            catch (Exception e)
            {
                Assert.IsType<DirectoryNotFoundException>(e);
            }
        }


        [Fact]
        public void RenameFolder_PathFolderToRenameExist_IOExceptionReturned()
        {
            string oldFolder = pathTestFolder + "\\OldFolder";
            string newFolder = pathTestFolder + "\\RenamedFolder";
            if (!Directory.Exists(oldFolder))
            {
                Directory.CreateDirectory(oldFolder);
            }
            try
            {
                fmc.PostRename(oldFolder, newFolder);
            }
            catch (Exception e)
            {
                Assert.IsType<IOException>(e);
            }
        }


        [Fact]
        public void CreateFolder_NewFolder_TrueReturned()
        {
            string pathCreateFolder = pathTestFolder + "\\CreateFolder";
            fmc.PostCreate(pathCreateFolder, TypeItem.Folder);

            Assert.True(Directory.Exists(pathCreateFolder));
        }


        [Fact]
        public void CreateFolder_NewFile_TrueReturned()
        {
            string pathCreateFolder = pathTestFolder + "\\CreateFolder\\newFile.txt";
            if (!File.Exists(pathCreateFolder))
            {
                fmc.PostCreate(pathCreateFolder, TypeItem.File);
            }
            Assert.True(File.Exists(pathCreateFolder));
        }


        [Fact]
        public void CreateFolder_nullPath_ArgumentNullExceptionReturned()
        {
            try
            {
                fmc.PostCreate(null, TypeItem.File);
            }
            catch (ArgumentNullException e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void CreateFolder_NewFolderWithWrongPath_TrueReturned()
        {
            string pathWrongFolder = pathTestFolder + "\\wrongPath\\CreateFolder\\newFile.txt";
            try
            {
                fmc.PostCreate(pathWrongFolder, TypeItem.File);
            }
            catch (ArgumentNullException e)
            {
                Assert.IsType<DirectoryNotFoundException>(e);
            }
        }


        [Fact]
        public void CreateFolder_0ItemType_TrueReturned()
        {
            string pathWrongFolder = pathTestFolder + "\\wrongPath\\CreateFolder\\newFile.txt";
            try
            {
                fmc.PostCreate(pathWrongFolder, 0);
            }
            catch (ArgumentNullException e)
            {
                Assert.IsType<DirectoryNotFoundException>(e);
            }
        }
    }
}
