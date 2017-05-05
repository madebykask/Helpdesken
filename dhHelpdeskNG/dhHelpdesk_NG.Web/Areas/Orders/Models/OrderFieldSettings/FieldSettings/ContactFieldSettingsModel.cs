using DH.Helpdesk.BusinessData.Enums.Orders.Fields;
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

        [LocalizedDisplay(OrderLabels.ContactId)]
        public TextFieldSettingsModel Id { get; set; }

        [LocalizedDisplay(OrderLabels.ContactName)]
        public TextFieldSettingsModel Name { get; set; }

        [LocalizedDisplay(OrderLabels.ContactPhone)]
        public TextFieldSettingsModel Phone { get; set; }

        [LocalizedDisplay(OrderLabels.ContactEMail)]
        public TextFieldSettingsModel Email { get; set; }
    }
}