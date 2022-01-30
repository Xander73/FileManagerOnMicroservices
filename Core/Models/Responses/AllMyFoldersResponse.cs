using Core.Models.DTO;

namespace Core.Models.Responses
{
    public class AllMyFoldersResponse
    {
        public List<MyFolderDTO> Items { get; set; }
        public Exception ex = null;


        public AllMyFoldersResponse()
        {
            Items = new List<MyFolderDTO>();
            Exception ex = null;
        }
    }
    
}
