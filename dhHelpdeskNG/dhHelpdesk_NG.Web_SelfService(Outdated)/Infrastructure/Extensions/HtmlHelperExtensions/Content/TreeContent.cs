namespace DH.Helpdesk.SelfService.Infrastructure.Extensions.HtmlHelperExtensions.Content
{
    using System;
    using System.Collections.Generic;

    public sealed class TreeContent
    {
        public TreeContent(List<TreeItem> items, string selectedValue)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items", "Value cannot be null.");
            }

            this.Items = items;
            this.SelectedValue = selectedValue;
        }

        public string SelectedValue { get; private set; }

        public List<TreeItem> Items { get; private set; }
    }
}