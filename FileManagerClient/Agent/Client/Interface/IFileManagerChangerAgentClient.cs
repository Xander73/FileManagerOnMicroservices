using Core.Models.Requests;

namespace FileManagerClient.Agent.Client.Interface
{
    public interface IFileManagerChangerAgentClient
    {
        void CreateItem(CreateRequest request);


        void DeleteItem(DeleteRequest request);


        void RenameItem(RenameItemRequest request);


        void CopyItem(CopyItemRequest request);
    }
}
