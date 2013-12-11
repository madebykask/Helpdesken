namespace dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content
{
    using System;
    using System.Collections.Generic;

    public sealed class DropDownWithSubmenusItem
    {
        public DropDownWithSubmenusItem(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Value cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value", "Value cannot be null or empty.");
            }

            this.Name = name;
            this.Value = value;
            this.Subitems = new List<DropDownWithSubmenusItem>();
        }

        public string Value { get; private set; }

        public string Name { get; private set; }

        public List<DropDownWithSubmenusItem> Subitems { get; private set; }
    }
}