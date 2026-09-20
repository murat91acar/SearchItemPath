namespace SearchForItem.Interfaces.Files.Entities
{
    public class DataFile
    {
        public ItemStep ItemStep { get; set; }

        public Dictionary<Item, List<string>> ItemPaths { get; set; }

        public List<Item> Items { get; set; }

        public string ErrMessage { get; set; }
    }
}
