namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    using System.Collections.Generic;

    public sealed class Contact
    {
        public Contact(List<string> ids, string name, string phone, string email)
        {
            this.Ids = ids;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
        }

        public List<string> Ids { get; private set; }

        public string Name { get; private set; }

        public string Phone { get; private set; }

        public string Email { get; private set; }
    }
}