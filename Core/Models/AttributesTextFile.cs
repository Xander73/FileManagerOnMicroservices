

namespace FileManagerClient.Agent.Models
{
    public class AttributesTextFile
    {        
        public int Size { get; set; }
        public bool Hidden { get; set; }
        public bool ReadOnly { get; set; }
        public int Paragraphes { get; set; }
        public int Words { get; set; }
        public int Chars { get; set; }
        public int CharsWithoutSpace { get; set; }
    }
}
