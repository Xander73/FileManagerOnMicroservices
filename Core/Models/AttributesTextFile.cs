

namespace Core.Models
{
    [Serializable]
    public class AttributesTextFile
    {        
        public int Paragraphes { get; set; }
        public int Words { get; set; }
        public int Chars { get; set; }
        public int CharsWithoutSpace { get; set; }
    }
}
