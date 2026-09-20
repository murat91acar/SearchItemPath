namespace SearchForItem.Interfaces.Files.Entities
{
    public class ItemStep
    {
        public string StepDescription { get; set; }

        public List<ItemStep> NextItemSteps { get; set; }

        public int ItemId { get; set; }

        public List<string> FindItemPath(int itemId)
        {
            var result = new List<string>();
            var stack = new Stack<(ItemStep ItemStep, List<string> Paths)>();

            stack.Push((this, new List<string>()));

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                var itemStep = current.ItemStep;

                var nextItemPath = new List<string> (current.Paths);

                if (itemStep.ItemId != itemId)
                {
                    nextItemPath.Add(itemStep.StepDescription);
                }

                if (itemStep.ItemId == itemId)
                {
                    result.AddRange(nextItemPath);
                }

                if (itemStep.NextItemSteps?.Count == null)
                {
                    continue;
                }

                for (int i = itemStep.NextItemSteps.Count - 1 ; i >= 0; i--)
                {
                    stack.Push((itemStep.NextItemSteps[i], nextItemPath));
                }
            }

            return result;
        }
    }
}
