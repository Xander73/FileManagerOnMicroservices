using Core.Interfaces;

namespace Core
{
    [Serializable]
    public class Item : IItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }

        public TypeItem FolderOrFile { get; set; }


    }
}
