using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public class ContactOverview
    {
        public ContactOverview(string id, string name, string phone, string eMail)
        {
            Id = id;
            Name = name;
            Phone = phone;
            EMail = eMail;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Phone { get; private set; }
        public string EMail { get; private set; }

    }
}
