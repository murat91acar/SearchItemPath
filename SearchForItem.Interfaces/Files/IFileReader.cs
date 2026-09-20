using SearchForItem.Interfaces.Files.Entities;

namespace SearchForItem.Interfaces.Files
{
    public interface IFileReader
    {
        public DataFile GetDataFromFile(bool doesUsePathDic, string filePath);
    }
}
