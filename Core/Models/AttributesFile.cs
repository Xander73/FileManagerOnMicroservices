

namespace Core.Models
{
    [Serializable]
    public class AttributesFile
    {
        public long Size { get; set; }
        public bool Hidden { get; set; }
        public bool ReadOnly { get; set; }
        public AttributesTextFile AttributesText { get; set; }


        public AttributesFile()
        {
            AttributesText = new AttributesTextFile();
        }
    }
}
