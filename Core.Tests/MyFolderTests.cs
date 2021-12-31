using System;
using System.Collections.Generic;
using System.IO;
using Xunit ;

namespace Core.Tests
{
    public class MyFolderTests
    {
        MyFolder myFolder = new MyFolder(Directory.GetCurrentDirectory() + "\\TestDirectory");


        public MyFolderTests()
        {
            myFolder = new MyFolder(Directory.GetCurrentDirectory() + "\\TestDirectory");
            Directory.CreateDirectory(myFolder.CurrentPath);
        }


        [Fact]
        public void CreateFolder_NewFolder_TrueReturned()
       {
            myFolder.CreateFolder();

            Assert.True(Directory.Exists(myFolder.CurrentPath + "\\New Folder"));
        }


        [Fact]
        public void CreateFolder_NameFolder_TrueReturned()
        {
            myFolder.CreateFolder("NameFolder");

            Assert.True(Directory.Exists(myFolder.CurrentPath + "\\NameFolder"));
        }


        [Fact]
        public void DeleteFolder_FolderToDelete_FalsseReturned()
        {
            string nameFolder = "FolderToDelete";
            myFolder.CreateFolder(nameFolder);

            myFolder.DeleteFolder(nameFolder);

            Assert.False(Directory.Exists(myFolder.CurrentPath + "\\" + nameFolder));
        }


        [Fact]
        public void RenameFolder_FolderToDelete_FalsseReturned()
        {
            string oldFolder = "OldFolder";
            string newFolder = "RenamedFolder";
            myFolder.CreateFolder(oldFolder);
            Directory.Delete(myFolder.CurrentPath + "\\" + newFolder);
            myFolder.RenameFolder(oldFolder, newFolder);

            bool isResult = Directory.Exists(myFolder.CurrentPath + "\\" + newFolder);
            

            Assert.True(isResult);
        }


        [Fact]
        public void CopyFolder_FolderToDelete_FalsseReturned()
        {
            string oldFolder = "CopyFolder";
            using (File.Create(myFolder.CurrentPath + '\\' + oldFolder + "\\copyFolder")) { } ;
            string newFolder = "CopyedFolder";
            myFolder.CreateFolder("\\" + newFolder);

            myFolder.CopyFolder(oldFolder, myFolder.CurrentPath + '\\' + newFolder) ;

            string[] excpected = { "copyFolder" };
            string[] str = Directory.GetFiles(myFolder.CurrentPath + '\\' + newFolder);


            List<string> actual = new List<string>();
            
            foreach (var item in str)
            {
                FileInfo fileInfo = new FileInfo(item);
                actual.Add(fileInfo.Name);
            }
            Assert.Equal(excpected[0], actual[0]);
        }


        [Fact]
        public void SizeFolder_CurrentFolder_TrueReturned()
        {
            int expected = 9;

            int actual = myFolder.SizeFolder(myFolder.CurrentPath);

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Search_DiskDCopyFolder_TrueReturned()
        {
            myFolder.CreateFolder("SearchedDirrectory");
            List <string> expected = new List<string>() { @"D:\0_work\C#\FileManagerOnMicroservices\Core.Tests\bin\Debug\net6.0\TestDirectory\SearchedDirrectory" };

            var actual = myFolder.Search(myFolder.CurrentPath, "SearchedDirrectory");

            Assert.Equal(expected, actual);
        }
    }
}
