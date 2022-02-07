using Core.Models.Requests;
using Core.Models.Responses;

namespace FileManagerClient.Agent.Client.Interface
{
    public interface IFileManagerChangerAgentClient
    {
        ExceptionResponse CreateItem(CreateRequest request);


        ExceptionResponse DeleteItem(DeleteRequest request);


        ExceptionResponse RenameItem(RenameItemRequest request);


        ExceptionResponse CopyItem(CopyItemRequest request);
    }
}
