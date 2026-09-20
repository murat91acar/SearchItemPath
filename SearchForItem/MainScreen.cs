using SearchForItem.Interfaces.Files;
using SearchForItem.Interfaces.Files.Entities;
using SearchForItem.Interfaces.View;
using System.Reflection;
using System.Text;

namespace SearchForItem
{
    public class MainScreen : IMainScreen
    {
        private readonly IFileReader _reader;

        private readonly IFindPathScreen _findPathScreen;

        private DataFile _readDataFile;

        // If file is too big this can be set as false and search can be done in the tree
        // If file is small this can be set as true so during tree creation paths will be created and the search will be faster
        // Doesn't make sense to have both options under one process but this decision would need double check and be sure about file size 
        private const bool _doesUsePathDic = true;

        public MainScreen(IFileReader reader, IFindPathScreen findPathScreen)
        {
            _reader = reader;
            _findPathScreen = findPathScreen;
        }
        public void Execute()
        {
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestDataFile/TestFile.txt");
            _readDataFile = _reader.GetDataFromFile(_doesUsePathDic, filePath);
            _findPathScreen.Execute(_readDataFile, _doesUsePathDic);
        }
    }
}
