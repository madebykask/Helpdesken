namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public static class WebMvcHelper
    {
        public static SelectList CreateListField(IEnumerable<ItemOverview> items)
        {
            return CreateListField(items, null, true);
        }

        public static SelectList CreateListField(
                    IEnumerable<ItemOverview> items,
                    int? selectedValue)
        {
            return CreateListField(items, selectedValue, true);
        }

        public static SelectList CreateListField(
            IEnumerable<ItemOverview> items,
            int? selectedValue,
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

        public static SelectListItem[] GetListItems(ItemOverview[] overviews)
        {
            return overviews.Select(o => new SelectListItem
                                             {
                                                 Value = o.Value,
                                                 Text = o.Name
                                             }).ToArray();
        }

        public static ItemOverview[] GetOverviews(SelectListItem[] items)
        {
            return items.Select(i => new ItemOverview(i.Text, i.Value)).ToArray();
        }
    }
}