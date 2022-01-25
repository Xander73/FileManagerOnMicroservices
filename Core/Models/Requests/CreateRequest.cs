

using Core.Interfaces;

namespace Core.Models.Requests
{
    public class CreateRequest
    {
        public string ClientBaseAddres { get; set; }

        public string PathNewItem { get; set; }

        public TypeItem TypeItem { get; set; }
    }
}
