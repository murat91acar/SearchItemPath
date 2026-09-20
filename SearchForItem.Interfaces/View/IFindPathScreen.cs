using SearchForItem.Interfaces.Files.Entities;

namespace SearchForItem.Interfaces.View
{
    public interface IFindPathScreen
    {
        public void Execute(DataFile ReadDataFile, bool DoesUsePathDic);
    }
}
