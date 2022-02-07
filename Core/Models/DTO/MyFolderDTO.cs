

namespace Core.Models.DTO
{
    [Serializable]
    public class MyFolderDTO : Item
    {
        public int FilesInFolder { get; set; }
        public int FoldersInFolder { get; set; }
        public long SizeFolders { get; set; }
        public AttributesFolder AttributesFolderProp { get; set; }


        public MyFolderDTO()
        {
            AttributesFolderProp = new AttributesFolder();
        }
    }
}
