using Core.Models.DTO;

namespace Core.Models.Responses
{
    public class AllMyFilesResponse
    {
        public List<MyFileDTO> Items { get; set; }
        public Exception ex = null;


        public AllMyFilesResponse()
        {
            Items = new List<MyFileDTO>();
        }
    }
}
