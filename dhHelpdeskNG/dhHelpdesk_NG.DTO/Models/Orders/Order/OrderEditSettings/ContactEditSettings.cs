using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    public class ContactEditSettings
    {
        public ContactEditSettings(TextFieldEditSettings id, TextFieldEditSettings name, TextFieldEditSettings phone, TextFieldEditSettings eMail)
        {
            Id = id;
            Name = name;
            Phone = phone;
            EMail = eMail;
        }

        public TextFieldEditSettings Id { get; private set; }
        public TextFieldEditSettings Name { get; private set; }
        public TextFieldEditSettings Phone { get; private set; }
        public TextFieldEditSettings EMail { get; private set; }
    }
}
