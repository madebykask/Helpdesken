using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.BusinessData.Models.Shared
{
    public sealed class CustomSelectList
    {
        public CustomSelectList()
        {
            Items = new ListItems();
            SelectedItems = new SelectedItems();
        }

        public ListItems Items { get; }

        public SelectedItems SelectedItems { get; }
    }

    public sealed class ListItem
    {
        public ListItem(string id, string value, bool isActive)
        {
            Id = id;
            Value = value;
            IsActive = isActive;
        }

        public string Id { get; }

        public string Value { get; }

        public bool IsActive { get; }

        public static ListItem CreateEmpty()
        {
            return new ListItem(string.Empty, string.Empty, true);
        }
    }

    public sealed class ListItems : List<ListItem>
    {
        public ListItems()
        {
        }

        public ListItems(List<ListItem> listItems)
        {
            ClearItems();
            AddItems(listItems);
        }

        public void AddItem(ListItem item)
        {
            Add(item);
        }

        public void AddItem(string id, string value, bool isActive = true)
        {
            Add(new ListItem(id, value, isActive));
        }

        public void AddItems(IList<ListItem> items)
        {
            AddRange(items);
        }

        public void ClearItems()
        {
            Clear();
        }
    }

    public sealed class SelectedItems : List<int>
    {
        public SelectedItems()
        {
        }

        public SelectedItems(List<int> listSelectedItems)
        {
            ClearItems();
            AddRange(listSelectedItems);
        }

        public SelectedItems(string strItems, bool ignoreNegativeItems = true, string separator = ",")
        {
            AddItems(strItems, ignoreNegativeItems, separator);
        }

        public void AddItem(int item)
        {
            Add(item);
        }

        public void AddItems(IList<int> items)
        {
            AddRange(items);
        }

        public void AddItems(string strItems, bool ignoreNegativeItems = true, string separator = ",")
        {
            if (!string.IsNullOrEmpty(strItems))
            {
                var items = strItems.Split(separator.ToCharArray());
                var o = 0;
                foreach (var item in items)
                {
                    if (int.TryParse(item, out o))
                    {
                        if (ignoreNegativeItems)
                        {
                            if (o > 0)
                                Add(o);
                        }
                        else
                        {
                            Add(o);
                        }
                    }
                }
            }
        }

        public int? GetFirstOrDefaultSelected()
        {
            return this.FirstOrDefault();
        }

        public string GetSelectedStr()
        {
            return string.Join(",", ToArray());
        }

        public string GetSelectedStrOrNull()
        {
            var ret = string.Join(",", ToArray());
            if (string.IsNullOrWhiteSpace(ret))
                return null;

            return ret;
        }

        // required for reflection logic
        public string ToStringValue()
        {
            return GetSelectedStrOrNull();
        }

        public void ClearItems()
        {
            Clear();
        }
    }
}