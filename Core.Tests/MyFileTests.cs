using Core.Interfaces;
using System.IO;
using Xunit;

namespace Core.Tests
{
    public class MyFileTests
    {
        IMyFile myFile;


        public MyFileTests()
        {
            myFile = new MyFile(Directory.GetCurrentDirectory(), "test.txt");

            using (FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "/test.txt", FileMode.OpenOrCreate));

            string text = "1111 111\n\t22 222";
            using (StreamWriter sr = new StreamWriter(Directory.GetCurrentDirectory() + "/test.txt"))
            {
                sr.WriteLine(text);
            }
        }

        [Fact]
        public void CreateFile_CurrentDir_TrueReturned ()
        {
            myFile.CreateFile();

            Assert.True(File.Exists(myFile.FullPath  + myFile.NameToPath()));
        }


        [Fact]
        public void CreateFile_CurrentDirAndNewFileName_TrueReturned()
        {
            string fileName = "CreateFile";
            myFile.CreateFile(fileName);

            Assert.True(File.Exists(myFile.FullPath + '\\' + fileName));
        }


        [Fact]
        public void Delete_CurrentDir_TrueReturned()
        {
            IMyFile myFileTemp = new MyFile(myFile.FullPath, "NewFileToDelete");
            myFileTemp.CreateFile(myFileTemp.Name);
            myFileTemp.DeleteFile();

            Assert.False(File.Exists(myFileTemp.FullPath + myFileTemp.NameToPath()));
        }


        [Fact]
        public void RenameFile_CurrentDir_TrueReturned()
        {
            IMyFile myFileTemp = new MyFile(myFile.FullPath, "NewFileToRename");
            myFileTemp.CreateFile(myFileTemp.Name);
            myFileTemp.RenameFile("RenamedFile");

            Assert.True(File.Exists(myFileTemp.FullPath + myFileTemp.NameToPath()));
        }


        [Fact]
        public void CopyFile_CurrentDirAndCurrentDirTest_TrueReturned()
        {
            IMyFile myFileTemp = new MyFile(myFile.FullPath, "NewFileToCopy");
            myFileTemp.CreateFile(myFileTemp.Name);
            IMyFile actual = myFileTemp.CopyFile(myFile.FullPath, "CopiedFile");

            Assert.True(File.Exists(myFileTemp.FullPath + myFileTemp.NameToPath()));
        }


        [Fact]
        public void SizeFile_TestFile_18ByteReturned()
        {
            long expected = 18;
            long actual = myFile.SizeFile();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetFileAttributes_NotTxtFile_ArchiveReturned()
        {
            string testFile = "TestFile";
            IMyFile myFileTemp = new MyFile(myFile.FullPath, testFile);
            myFile.CreateFile("TestFile");

            string execute = "Archive";

            string actual = myFile.GetFileAttributes();

            Assert.Equal(execute, actual);
        }


        [Fact]
        public void GetFileAttributes_TxtFile_TextAttributesReturned()
        {
            string testFile = "test.txt";
            IMyFile myFileTemp = new MyFile(myFile.FullPath, testFile);

            string execute = "Paragraphes - 1\nWords - 4\nChars - 15\nChars without space - 13\nArchive";

            string actual = myFile.GetFileAttributes();

            Assert.Equal(execute, actual);
        }


        [Fact]
        public void SetFileAttributes_Hiden_HidenReturned()
        {
            string testFile = "tests.txt";
            IMyFile myFileTemp = new MyFile(myFile.FullPath, testFile);
            myFile.CreateFile();

            myFile.SetFileAttributes(FileAttributes.Hidden);

            string execute = "Hidden";

            string actual = myFile.GetFileAttributes();

            Assert.Equal(execute, actual);
        }
    }
}
