using FileManagerClient.Agent.Client.Interface;
using Core.Models.Responses;
using Core.Models.Requests;
using Core.Interfaces;
using System.Threading.Tasks;

namespace FileManagerClient.Agent
{
    public class AgentConnect
    {
        public static AllDrivesResponse GetAllDrives (
            IFileManagerInformatorAgentClient fileManagerInformatorClient)
        {
            var requestDrivers = new AllDrivesRequest()
            {
                ClientBaseAddres = @"https://localhost:44396"
            };

            return fileManagerInformatorClient.GetDrives(requestDrivers);
        }


        public static AllMyFoldersResponse GetFoldersCurrentDirrectory(
            IFileManagerInformatorAgentClient fileManagerInformatorClient, 
            string currentFolder)
        {
            var requestItems = new ALLItemsRequest()
            {
                ClientBaseAddres = @"https://localhost:44396",
                PathRequiredFolder = currentFolder
            };

            return fileManagerInformatorClient.PostMyFolders(requestItems);
        }


        public static AllMyFilesResponse GetFilesCurrentDirrectory(
            IFileManagerInformatorAgentClient fileManagerInformatorClient, 
            string currentFolder)
        {
            var requestItems = new ALLItemsRequest()
            {
                ClientBaseAddres = @"https://localhost:44396",
                PathRequiredFolder = currentFolder
            };

            return fileManagerInformatorClient.PostMyFiles(requestItems);
        }


        public static ExceptionResponse PostItemCopy(
            IFileManagerChangerAgentClient fileManagerChangerAgentClient, 
            string pathFromCopy, 
            string pathToCopy)
        {
            var copyRequest = new CopyItemRequest()
            {
                ClientBaseAddres = @"https://localhost:44372",
                OldPath = pathFromCopy,
                NewPath = pathToCopy
            };

            return fileManagerChangerAgentClient.CopyItem(copyRequest);
        }


        public static ExceptionResponse PostItemDelete(
            IFileManagerChangerAgentClient fileManagerChangerAgentClient,
            string pathDelete)
        {
            var deleteRequest = new DeleteRequest()
            {
                ClientBaseAddres = @"https://localhost:44372",
                PathDelete = pathDelete
            };
            
            return fileManagerChangerAgentClient.DeleteItem(deleteRequest);
        }


        public static ExceptionResponse RenameItem(
            IFileManagerChangerAgentClient fileManagerChangerAgentClient, 
            string pathOldItem, 
            string newName)
        {
            var renameRequest = new RenameItemRequest()
            {
                ClientBaseAddres = @"https://localhost:44372",
                PathOldItem = pathOldItem,
                NewNameItem = newName
            };

            return fileManagerChangerAgentClient.RenameItem(renameRequest);
        }


        public static ALLItemsResponse GetSearchItem(
            IFileManagerInformatorAgentClient fileManagerInformatorAgentClient,
            string pathFolder,
            string searchNameItem)
        {
            var request = new SearchItemRequest()
            {
                ClientBaseAddres = @"https://localhost:44396",
                PathFolder = pathFolder,
                SearchNameItem = searchNameItem
            };

            return fileManagerInformatorAgentClient.PostSearch(request);
        }


        public static ExceptionResponse PostItemCreate (
            IFileManagerChangerAgentClient fileManagerChangerAgentClient,
            string newName,
            TypeItem typeNewItem
            )
        {
            var request = new CreateRequest()
            {
                ClientBaseAddres = @"https://localhost:44372",
                PathNewItem = newName,
                TypeItem = typeNewItem
            };

            return fileManagerChangerAgentClient.CreateItem(request);
        }


        public static async Task<FolderAddInformationResponse> GetAddInformationAsync(
                IFileManagerInformatorAgentClient fileManagerInformatorAgentClient,
                string pathFolder
                )
            {
            var request = new FolderAddInformationResponseRequest()
            {
                ClientBaseAddres = @"https://localhost:44396",
                PathItem = pathFolder
            };
                return await Task.Run(() =>  fileManagerInformatorAgentClient.AdditionalInformation(request));
            }
    }
}
