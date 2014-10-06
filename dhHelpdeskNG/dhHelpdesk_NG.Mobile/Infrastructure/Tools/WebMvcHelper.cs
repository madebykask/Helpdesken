namespace DH.Helpdesk.Mobile.Infrastructure.Tools
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public static class WebMvcHelper
    {
        public static SelectList CreateListField(
                    IEnumerable<ItemOverview> items,
                    int selectedValue)
        {
            return CreateListField(items, selectedValue, false);
        }

        public static SelectList CreateListField(
            IEnumerable<ItemOverview> items,
            int selectedValue,
            bool needEmpty)
        {
            var list = new List<ItemOverview>();
            if (needEmpty)
            {
                list.Add(ItemOverview.CreateEmpty());
            }

            list.AddRange(items);

            return new SelectList(list, "Value", "Name", selectedValue);
        }

        public static MultiSelectList CreateMultiSelectField(
            IEnumerable<ItemOverview> items,
            IEnumerable<int> selectedValues)
        {
            return new MultiSelectList(items, "Value", "Name", selectedValues);
        }         
    }
}