namespace Core.Models.Responses
{
    public class ALLItemsResponse
    {
        public List<Item> Items { get; set; }


        public ALLItemsResponse()
        {
            Items = new List<Item>();
        }
    }
}
