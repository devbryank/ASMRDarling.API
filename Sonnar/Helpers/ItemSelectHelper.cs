using System.Collections.Generic;

namespace Sonnar.Helpers
{
    class ItemSelectHelper
    {
        public static T SelectItem<T>(List<T> itemList, string targetProp, string targetString)
        {
            return itemList.Find(i => i.GetType().GetProperty(targetProp).GetValue(i, null).ToString().Equals(targetString));
        }


        public static List<T> SelectItems<T>(List<T> itemList, string targetProp, string targetString)
        {
            return itemList.FindAll(i => i.GetType().GetProperty(targetProp).GetValue(i, null).ToString().Contains(targetString));
        }
    }
}
