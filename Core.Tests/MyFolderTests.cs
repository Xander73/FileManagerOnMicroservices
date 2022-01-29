using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Core.Tests
{
    public class MyFolderTests
    {
        MyFolder myFolder;
        string pathTestFolder = Directory.GetCurrentDirectory() + "\\TestDirectory";


        public MyFolderTests()
        {
            myFolder = new MyFolder(pathTestFolder);
            Directory.CreateDirectory(myFolder.FullPath);
        }


        [Fact]
        public void CreateFolder_NewFolder_TrueReturned()
        {
            myFolder.CreateFolder();

            Assert.True(Directory.Exists(myFolder.FullPath + "\\New Folder"));
        }


        [Fact]
        public void CreateFolder_nullPath_ExceptionReturned()
        {
            try
            {
                myFolder.CreateFolder(null);
            }
            catch (ArgumentNullException e)
            {
                Assert.IsType<ArgumentNullException> (e); 
            }
        }


        [Fact]
        public void CreateFolder_NewFolderWithPath_TrueReturned()
        {
            myFolder.CreateFolder(pathTestFolder + "\\testCreateDirectory");

            Assert.True(Directory.Exists(pathTestFolder + "\\testCreateDirectory"));
        }


        [Fact]
        public void DeleteFolder_FolderToDelete_FelseReturned()
        {
            string nameFolder = pathTestFolder + "\\FolderToDelete";
            myFolder.CreateFolder(nameFolder);

            myFolder.DeleteFolder(nameFolder);

            Assert.False(Directory.Exists(nameFolder));
        }


        [Fact]
        public void DeleteFolder_NotExistFolder_DirectoryNotFoundExceptionReturned()
        {
            string nameFolder = pathTestFolder + "\\FolderToDelete";

            try
            {
                myFolder.DeleteFolder(nameFolder);
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
                myFolder.DeleteFolder(null);
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
            myFolder.CreateFolder(oldFolder);
            if (File.Exists(newFolder))
            {
                Directory.Delete(newFolder);
            }
            myFolder.RenameFolder(oldFolder, newFolder);


            bool isResult = Directory.Exists(newFolder);

            Assert.True(isResult);
        }


        [Fact]
        public void RenameFolder_NullNewFolderToRename_ArgumentNullExceptionReturned()
        {
            string oldFolder = pathTestFolder + "\\OldFolder";
            string newFolder = pathTestFolder + "\\RenamedFolder";
            myFolder.CreateFolder(oldFolder); if (File.Exists(newFolder))
                if (File.Exists(newFolder))
                {
                    Directory.Delete(newFolder);
                }
            try
            {
                myFolder.RenameFolder(oldFolder, null);
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
            myFolder.CreateFolder(oldFolder); 
            if (File.Exists(newFolder))
            {
                Directory.Delete(newFolder);
            }
            try
            {
                myFolder.RenameFolder(null, newFolder);
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
            myFolder.CreateFolder(oldFolder);
            if (File.Exists(newFolder))
            {
                Directory.Delete(newFolder);
            }
            try
            {
                myFolder.RenameFolder(null, null);
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
            myFolder.CreateFolder(oldFolder);
            if (Directory.Exists(newFolder))
            {
                Directory.Delete(newFolder);
            }
            try
            {
                myFolder.RenameFolder(wrongOldFolder, newFolder);
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
            myFolder.CreateFolder(oldFolder);
            try
            {
                myFolder.RenameFolder(oldFolder, newFolder);
            }
            catch (Exception e)
            {
                Assert.IsType<IOException>(e);
            }
        }


        [Fact]
        public void CopyFolder_CopyFolder_FalseReturned()
        {
            string oldFolder = pathTestFolder + "\\CopyFolder";
            myFolder.CreateFolder(oldFolder);
            using (File.Create(oldFolder + "\\copyFile.txt")) ; 
            string newFolder = pathTestFolder + "\\CopyedFolder\\CopyFolder";
            myFolder.CreateFolder(newFolder);

            myFolder.CopyFolder(oldFolder, newFolder);

            string excpected = "CopyedFolder";
            string[] str = Directory.GetDirectories(pathTestFolder);
            bool actual = false;

            foreach (var item in str)
            {
                if (Path.GetFileName(item) == excpected)
                {
                    actual = true;
                }
            }
            Assert.True(actual);
        }


        [Fact]
        public void CopyFolder_OldFolderNull_ArgumentNullExceptionReturned()
        {
            string oldFolder = pathTestFolder + "\\CopyFolder";
            myFolder.CreateFolder(oldFolder);
            using (File.Create(oldFolder + "\\copyFile.txt")) ;
            string newFolder = pathTestFolder + "\\CopyedFolder";
            myFolder.CreateFolder(newFolder);

            try
            {
                myFolder.CopyFolder(null, newFolder);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void CopyFolder_NewFolderNull_ArgumentNullExceptionReturned()
        {
            string oldFolder = pathTestFolder + "\\CopyFolder";
            myFolder.CreateFolder(oldFolder);
            using (File.Create(oldFolder + "\\copyFile.txt")) ;
            string newFolder = pathTestFolder + "\\CopyedFolder";
            myFolder.CreateFolder(newFolder);

            try
            {
                myFolder.CopyFolder(oldFolder, null);
            }
            catch (Exception e)
            {
                Assert.IsType<ArgumentNullException>(e);
            }
        }


        [Fact]
        public void CopyFolder_OldFolderWrongPath_FalsseReturned()
        {
            string oldFolder = pathTestFolder + "\\CopyFolder";
            string wrongOldFolder = oldFolder + "mistake";
            myFolder.CreateFolder(oldFolder);
            using (File.Create(oldFolder + "\\copyFile.txt")) ;
            string newFolder = pathTestFolder + "\\CopyedFolder";
            myFolder.CreateFolder(newFolder);

            try
            {
                myFolder.CopyFolder(wrongOldFolder, newFolder);
            }
            catch (Exception e)
            {
                Assert.IsType<DirectoryNotFoundException>(e);
            }
        }


        [Fact]
        public void SizeFolder_CurrentFolder_TrueReturned()
        {
            string textToSizeFile = "Текст для размера файла";
            MyFolder myFolder = new MyFolder(pathTestFolder + "\\sizeFolder");
            myFolder.CreateFolder(pathTestFolder + "\\sizeFolder");
            MyFile myFile = new MyFile(myFolder.FullPath + "\\SizeFile.txt");
            myFile.CreateFile(myFile.FullPath);
            using(StreamWriter sw = new StreamWriter(pathTestFolder + "\\sizeFolder" + "\\SizeFile.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine(textToSizeFile);
            }

            long expected = 45;
            
            myFolder.SizeFolder(pathTestFolder + "\\sizeFolder");

            long actual = myFolder.SizeFolders;

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Search_CurrentFolderSearchFolder_TrueReturned()
        {
            string searchFolderName = pathTestFolder + "\\SearchedDirrectory";
            myFolder.CreateFolder(searchFolderName);
            List<Item> expected = new List<Item>();
            expected.Add(
                new Item()
                {
                    FullPath = searchFolderName,
                    Name = Path.GetFileName(searchFolderName),
                    FolderOrFile = TypeItem.Folder
                }
            );
            List<Item> actual = MyFolder.Search(myFolder.FullPath, "SearchedDirrectory");

            Assert.True(expected[0].FullPath == actual[0].FullPath);
        }


        [Fact]
        public void Search_CurrentFolderSearchFile_TrueReturned()
        {
            string searchFolderName = pathTestFolder + "\\SearchedDirrectory";
            myFolder.CreateFolder(searchFolderName);
            File.Create(searchFolderName + "\\test.txt");
            List<Item> expected = new List<Item>()
            {
                new Item()
                {
                    FullPath = searchFolderName + "\\test.txt",
                    Name = Path.GetFileName("test.txt"),
                    FolderOrFile = TypeItem.File
                }
            };
            
            List<Item> actual = MyFolder.Search(searchFolderName, "test");

            Assert.True(expected[0].FullPath == actual[0].FullPath);
        }
    }
}
