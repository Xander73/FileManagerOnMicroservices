using System;

namespace Core.Models.Responses
{
    public class FolderAddInformationResponse
    {
        public string AddInformation { get; set; }

        public Exception ex;


        public FolderAddInformationResponse()
        {
            ex = new Exception();
        }
    }
}
