using FileManagerClient.Agent.Models.Requests;
using FileManagerClient.Agent.Models.Responses;
using System;
using System.Collections.Generic;

namespace FileManagerClient.Agent.Client.Interface
{
    public interface IFileManagerInformatorClient
    {
        void CreateFolder();


        void CreateFolder(string nameFolder);


        void DeleteFolder(string fullName);


        void RenameFolder(string oldName, string newName);


        void CopyFolder(string oldName, string newName);


        int SizeFolder(string name);


        List<string> Search(string path, string name);


        IEnumerable<string> GetItemsInFolder();


        AllDrivesResponse GetDrives(AllDrivesRequest request);
    }
}
