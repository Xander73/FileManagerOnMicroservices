namespace Core.Models.Responses
{
    public class ALLItemsResponse
    {
        public List<Item> Items { get; set; }
        public Exception ex;


        public ALLItemsResponse()
        {
            Items = new List<Item>();
            ex = new Exception();
        }
    }
}
