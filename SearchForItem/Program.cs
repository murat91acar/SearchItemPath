using SearchForItem;
using SearchForItem.Files;
using SearchForItem.Interfaces.Files;
using SearchForItem.Interfaces.Files.Entities;
using SearchForItem.Interfaces.View;

internal class Program
{
    private static IMainScreen _view;
    private static IFileReader _fileReader;
    private static IFindPathScreen _findPathScreen;

    private static void Main(string[] args)
    {
        _fileReader = new FileReader();
        _findPathScreen = new FindPathScreen();
        _view = new MainScreen(_fileReader, _findPathScreen);
        _view.Execute();
    }
}