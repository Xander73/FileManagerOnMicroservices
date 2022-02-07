

namespace Core.Models.DTO
{
    [Serializable]
    public class MyFileDTO : Item
    {
        public AttributesFile AttributesFile { get; set; }


        public MyFileDTO()
        {
            AttributesFile = new AttributesFile();
        }

    }
}
