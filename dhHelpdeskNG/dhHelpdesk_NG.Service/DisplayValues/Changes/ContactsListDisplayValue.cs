namespace DH.Helpdesk.Services.DisplayValues.Changes
{
    using System.Collections.Generic;
    using System.Text;

    using DH.Helpdesk.BusinessData.Models.Changes;

    public sealed class ContactsListDisplayValue : DisplayValue<List<Contact>>
    {
        public ContactsListDisplayValue(List<Contact> value)
            : base(value)
        {
        }

        public override string GetDisplayValue()
        {
            if (this.Value == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach (var contact in this.Value)
            {
                sb.AppendLine(string.Format(
                                "{0};{1};{2};{3};",
                                contact.Name,
                                contact.Phone,
                                contact.Email,
                                contact.Company));
            }

            return sb.ToString();            
        }
    }
}