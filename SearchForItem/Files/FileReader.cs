using SearchForItem.Common;
using SearchForItem.Interfaces.Files;
using SearchForItem.Interfaces.Files.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SearchForItem.Files
{
    internal class FileReader : IFileReader
    {
        public DataFile GetDataFromFile(bool doesUsePathDic, string filePath)
        {
            var data = new DataFile() { ItemStep = new ItemStep() { StepDescription = "Start"} };
            var stack = new Stack<(ItemStep ItemStep, int StepIndex)>();
            stack.Push((data.ItemStep, -1));

            var itemId = 0;
            try
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    int indentLevel = line.TakeWhile(c => !Char.IsLetter(c)).Count();

                    if (line.Substring(indentLevel - 2, 1) != "+" && !string.Equals(line.Substring(indentLevel, 5), "Item:", StringComparison.OrdinalIgnoreCase))
                    {
                        data.ErrMessage = $"Unexpected line. Lines should start with + for paths and Item: for items";
                        break;
                    }

                    var stepDescription = line.Remove(0, indentLevel).Replace("Item: ", "");

                    var newStep = new ItemStep() { StepDescription = stepDescription };

                    while (stack.Count > 0 && stack.Peek().StepIndex >= indentLevel)
                    {
                        stack.Pop();
                    }

                    if (stack.Count != 0)
                    {
                        var firstStep = stack.Peek().ItemStep;

                        if (firstStep.NextItemSteps == null)
                        {
                            firstStep.NextItemSteps = new List<ItemStep>();
                        }

                        firstStep.NextItemSteps.Add(newStep);
                    }

                    var isItem = line.Contains("Item: ", StringComparison.OrdinalIgnoreCase);

                    if (!isItem)
                    {
                        stack.Push((newStep, indentLevel));
                    }
                    else
                    {
                        itemId = AddNewItem(newStep, data, itemId);

                        if (!doesUsePathDic)
                        {
                            continue;
                        }

                        var paths = stack.Select(x => x.ItemStep.StepDescription).ToList().StringReverse();

                        CreatePathDict(newStep, data, paths);
                    }
                }
            }
            catch (Exception ex)
            {

                //TODO: LogExceptionFunction(ex.Message). For internal investigation
                data.ErrMessage = $"Unexpected error occured. Please get in touch with your admin";
            }

            return data;
        }

        private void CreatePathDict(ItemStep newStep, DataFile data, List<string> paths)
        {
            if (data.ItemPaths == null)
            {
                data.ItemPaths = new Dictionary<Item, List<string>>();
            }

            var itemPath = data.ItemPaths?.Keys.FirstOrDefault(x => x.ItemId == newStep.ItemId);

            if (itemPath == null)
            {
                data.ItemPaths.Add(new Item() { ItemId = newStep.ItemId, ItemName = newStep.StepDescription }, paths);
            }
            else
            {
                data.ItemPaths[itemPath].AddRange(paths);
            }
        }

        private int AddNewItem(ItemStep newStep, DataFile data, int itemId)
        {
            if (data.Items == null)
            {
                data.Items = new List<Item>();
            }

            var newItem = data.Items.FirstOrDefault(x => string.Equals(x.ItemName, newStep.StepDescription, StringComparison.OrdinalIgnoreCase));

            if (newItem == null)
            {
                itemId++;
                data.Items.Add(new Item() { ItemId = itemId, ItemName = newStep.StepDescription });
                newStep.ItemId = itemId;
            }
            else
            {
                newStep.ItemId = newItem.ItemId;
            }

            return itemId;
        }
    }
}
