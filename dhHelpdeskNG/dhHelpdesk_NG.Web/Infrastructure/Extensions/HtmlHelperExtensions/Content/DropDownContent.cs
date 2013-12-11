namespace dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content
{
    using System;
    using System.Collections.Generic;

    public sealed class DropDownContent
    {
        public DropDownContent(List<DropDownItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items", "Value cannot be null.");
            }

            this.Items = items;
        }

        public DropDownContent(List<DropDownItem> items, string selectedValue)
            : this(items)
        {
            this.SelectedValue = selectedValue;
        }

        public List<DropDownItem> Items { get; private set; }

        public string SelectedValue { get; private set; }
    }
}