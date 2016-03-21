namespace DH.Helpdesk.SelfService.Infrastructure.Extensions.HtmlHelperExtensions.Content
{
    using System;

    public sealed class DropDownItem
    {
        public DropDownItem(string name, string value)
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
        }

        public string Name { get; private set; }

        public string Value { get; private set; }
    }
}