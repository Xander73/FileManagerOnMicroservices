

namespace Core.Models.DTO
{
    [Serializable]
    public class MyTextFileDTO : Item
    {
        public AttributesTextFile AttributesTextFile { get; set; }


        public MyTextFileDTO()
        {
            AttributesTextFile = new AttributesTextFile();
        }
    }
}
