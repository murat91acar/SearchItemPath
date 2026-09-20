using SearchForItem.Interfaces.Files.Entities;
using SearchForItem.Interfaces.View;
using System.Text;

namespace SearchForItem
{
    public class FindPathScreen : IFindPathScreen
    {
        private DataFile _readDataFile;

        private bool _doesUsePathDic;

        public void Execute(DataFile ReadDataFile, bool DoesUsePathDic)
        {
            _readDataFile = ReadDataFile;
            _doesUsePathDic = DoesUsePathDic;
            if (_readDataFile.ErrMessage != null && _readDataFile.ErrMessage != string.Empty) 
            {
                DisplayError(_readDataFile.ErrMessage);
            }
            else if (_readDataFile.ItemStep.NextItemSteps == null || _readDataFile.ItemStep.NextItemSteps.Count() == 0)
            {
                DisplayError("The file is empty please update the file with paths and items");
            }
            else
            {
                DisplayPaths();
            }
        }

        private void DisplayError(string? errMessage)
        {
            Console.WriteLine(errMessage);
            Console.Read();
        }

        private void DisplayPaths()
        {
            var response = string.Empty;
            var isExit = false;
            var isItemListNeeded = true;
            do
            {
                var sb = new StringBuilder();
                if (isItemListNeeded)
                {
                    DisplayItems(sb);
                    isItemListNeeded = false;
                }

                sb.Append("What item would you like to search for?");
                Console.WriteLine(sb.ToString());

                response = Console.ReadLine();

                if (response != string.Empty)
                {
                    isExit = DisplaySelectedPath(response, out isItemListNeeded);
                }
            }
            while (!isExit);
        }

        private bool DisplaySelectedPath(string response, out bool isItemListNeeded)
        {
            var convertedResponse = ResponseCheck(response, out var result);
            var sb = new StringBuilder();

            isItemListNeeded = false;

            if (convertedResponse.Item1 != string.Empty)
            {
                sb.AppendLine(convertedResponse.Item1);
                isItemListNeeded = false;
            }
            else if (!result)
            {
                var selectedItem = _readDataFile.Items.FirstOrDefault(x => x.ItemId == convertedResponse.Item2);
                sb.AppendLine($"Path for {selectedItem.ItemId} - {selectedItem.ItemName}");

                var itemPaths = new List<string>();
                if (!_doesUsePathDic)
                {
                    itemPaths = _readDataFile.ItemStep.FindItemPath(convertedResponse.Item2);
                }
                else
                {
                    itemPaths = _readDataFile.ItemPaths.FirstOrDefault(x => x.Key.ItemId == convertedResponse.Item2).Value;
                }

                var itemCount = 0;
                var firstPathDesc = _readDataFile.ItemStep.StepDescription;
                foreach (var itemPath in itemPaths)
                {
                    if (itemCount == 0 && string.Equals(itemPath, firstPathDesc, StringComparison.OrdinalIgnoreCase))
                    {
                        itemCount++;
                    }
                    else if (string.Equals(itemPath, firstPathDesc, StringComparison.OrdinalIgnoreCase))
                    {
                        sb.AppendLine("AND/OR");
                    }
                    else
                    {
                        sb.AppendLine(itemPath);
                    }
                }

                isItemListNeeded = true;
            }

            Console.WriteLine(sb.ToString());
            return result;
        }

        private void DisplayItems(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("Available items:");
            foreach (var item in _readDataFile.Items)
            {
                stringBuilder.AppendLine($"[{item.ItemId}] - {item.ItemName}");
            }
            stringBuilder.AppendLine("[0] - Exit");
        }

        private (string, int) ResponseCheck(string response, out bool isExit)
        {
            var errMessage = string.Empty;
            isExit = false;

            if (int.TryParse(response, out var selectedItem))
            {
                if (!_readDataFile.Items.Any(x => x.ItemId == selectedItem) && selectedItem != 0)
                {
                    errMessage = $"Given itemId: {selectedItem} couldn't be found in Item list. Could you please pick an item Id from the given list.";
                }

                if (selectedItem == 0)
                {
                    isExit = true;
                }
            }
            else
            {
                errMessage = $"{response} cannot be converted to an itemId.";
            }

            return (errMessage, selectedItem);
        }
    }
}
