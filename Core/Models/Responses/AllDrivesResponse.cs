

namespace Core.Models.Responses
{
    public class AllDrivesResponse
    {
        public List<string> Drives { get; set; }
        public Exception ex = null;


        public AllDrivesResponse()
        {
            Drives = new List<string>();
        }
    }
}
