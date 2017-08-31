namespace DH.Helpdesk.BusinessData.Models.Shared
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class CustomSelectList
    {
        public CustomSelectList()
        {
            this.Items = new ListItems();
            this.SelectedItems = new SelectedItems();
        }

        public ListItems Items { get; private set; }

        public SelectedItems SelectedItems { get; private set; }               
        
    }

    public sealed class ListItem
    {
        public ListItem(string id, string value, bool isActive)
        {
            this.Id = id;
            this.Value = value;
            this.IsActive = isActive;
        }

        public string Id { get; private set; }

        public string Value { get; private set; }

        public bool IsActive { get; private set; }

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
            this.ClearItems();
            this.AddItems(listItems);
        }

        public void AddItem(ListItem item)
        {
            this.Add(item);
        }

        public void AddItem(string id, string value, bool isActive = true)
        {
            this.Add(new ListItem(id, value, isActive));
        }

        public void AddItems(IList<ListItem> items)
        {
            this.AddRange(items);
        }

        public void ClearItems()
        {
            this.Clear();
        }
    }

    public sealed class SelectedItems : List<int>
    {
        public SelectedItems()
        {            
        }        

        public SelectedItems(List<int> listSelectedItems)
        {
            this.ClearItems();
            this.AddRange(listSelectedItems);
        }

        public SelectedItems(string strItems, bool ignoreNegativeItems = true, string separator = ",")
        {                       
            this.AddItems(strItems, ignoreNegativeItems, separator);
        }

        public void AddItem(int item)
        {
            this.Add(item);
        }

        public void AddItems(IList<int> items)
        {
            this.AddRange(items);
        }

        public void AddItems(string strItems, bool ignoreNegativeItems = true, string separator = ",")
        {
            if (!string.IsNullOrEmpty(strItems))
            {
                var items = strItems.Split(separator.ToCharArray());
                int o = 0;
                foreach (var item in items)
                    if (int.TryParse(item, out o))
                    {
                        if (ignoreNegativeItems)
                        {
                            if (o > 0)
                                this.Add(o);
                        }
                        else
                            this.Add(o);
                    }
            }
        }

        public string GetSelectedStr()
        {
            return string.Join(",", this.ToArray());            
        }

        public int? GetFirstOrDefaultSelected()
        {
            return this.FirstOrDefault();
        }

        public string GetSelectedStrOrNull()
        {
            var ret = string.Join(",", this.ToArray());
            if (string.IsNullOrWhiteSpace(ret))
                return null;
            else
                return ret;
        }

        public void ClearItems()
        {
            this.Clear();
        }

    }
}