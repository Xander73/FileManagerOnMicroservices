using Core.Models.Requests;
using Core.Models.Responses;

namespace FileManagerClient.Agent.Client.Interface
{
    public interface IFileManagerInformatorAgentClient
    {
        FolderAddInformationResponse AdditionalInformation(FolderAddInformationResponseRequest request);


        ALLItemsResponse PostSearch(SearchItemRequest request);


        ALLItemsResponse GetItemsInFolder(ALLItemsRequest request);


        AllMyFilesResponse PostMyFiles(ALLItemsRequest request);


        AllMyFoldersResponse PostMyFolders(ALLItemsRequest request);


        AllDrivesResponse GetDrives(AllDrivesRequest request);
    }
}
