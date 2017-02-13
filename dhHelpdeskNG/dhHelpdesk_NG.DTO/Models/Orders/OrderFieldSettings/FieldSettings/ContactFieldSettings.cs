using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    public class ContactFieldSettings
    {
        public ContactFieldSettings()
        {
        }

        public ContactFieldSettings(
            TextFieldSettings id,
            TextFieldSettings name,
            TextFieldSettings phone,
            TextFieldSettings email)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Email = email;
        }

        public TextFieldSettings Id { get; private set; }

        public TextFieldSettings Name { get; private set; }

        public TextFieldSettings Phone { get; private set; }

        public TextFieldSettings Email { get; private set; }
    }
}
