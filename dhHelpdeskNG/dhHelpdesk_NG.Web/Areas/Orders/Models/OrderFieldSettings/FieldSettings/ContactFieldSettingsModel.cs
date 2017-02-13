using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    public class ContactFieldSettingsModel
    {
        public ContactFieldSettingsModel()
        {
        }

        public ContactFieldSettingsModel(
            TextFieldSettingsModel id,
            TextFieldSettingsModel name,
            TextFieldSettingsModel phone,
            TextFieldSettingsModel email)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Email = email;
        }

        [LocalizedDisplay("Id")]
        public TextFieldSettingsModel Id { get; set; }

        [LocalizedDisplay("Namn")]
        public TextFieldSettingsModel Name { get; set; }

        [LocalizedDisplay("Telefon")]
        public TextFieldSettingsModel Phone { get; set; }

        [LocalizedDisplay("E-post")]
        public TextFieldSettingsModel Email { get; set; }
    }
}