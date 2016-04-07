namespace DH.Helpdesk.SelfService.Infrastructure.Extensions.HtmlHelperExtensions.Content
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class DropDownWithSubmenusContent
    {
        private bool DropDownItemContainsValue(DropDownWithSubmenusItem item, string value)
        {
            if (item.Value == value)
            {
                return true;
            }

            return item.Subitems.Any() && item.Subitems.Any(i => this.DropDownItemContainsValue(i, value));
        }

        public DropDownWithSubmenusContent(List<DropDownWithSubmenusItem> items, string selectedValue)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items", "Value cannot be null.");
            }

            if (selectedValue != null)
            {
                if (!items.Any(i => this.DropDownItemContainsValue(i, selectedValue)))
                {
                    throw new ArgumentOutOfRangeException(
                        "selectedValue", @"The given value was not present in ""items"".");
                }
            }

            this.Items = items;
            this.SelectedValue = selectedValue;
        }

        public List<DropDownWithSubmenusItem> Items { get; private set; }

        public string SelectedValue { get; private set; }
    }
}