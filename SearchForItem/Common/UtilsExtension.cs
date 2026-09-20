namespace SearchForItem.Common
{
    public static class UtilsExtension
    {
        public static List<string> StringReverse(this List<string> strList)
        {
            var listCount = strList.Count();
            if (listCount <= 1) return strList;

            for (int i = 0; i < listCount; i++)
            {
                if (i >= listCount - i - 1) break;
                var last = strList[listCount - i - 1];
                strList[listCount - i - 1] = strList[i];
                strList[i] = last;
            }

            return strList;
        }
    }
}
