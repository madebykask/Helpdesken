namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class UserFieldSettingsModel
    {
        public UserFieldSettingsModel()
        {            
        }


        public UserFieldSettingsModel(TextFieldSettingsModel userId,
            TextFieldSettingsModel userFirstName,
            TextFieldSettingsModel userLastName,
            TextFieldSettingsModel userPhone,
            TextFieldSettingsModel userEMail)
        {
            UserId = userId;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            UserPhone = userPhone;
            UserEMail = userEMail;
        }

        [NotNull]
        [LocalizedDisplay("Användar ID")]
        public TextFieldSettingsModel UserId { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Förnamn")]
        public TextFieldSettingsModel UserFirstName { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Efternamn")]
        public TextFieldSettingsModel UserLastName { get; set; }

        [NotNull]
        [LocalizedDisplay("Telefon")]
        public TextFieldSettingsModel UserPhone { get; set; }
        
        [NotNull]
        [LocalizedDisplay("E-post")]
        public TextFieldSettingsModel UserEMail { get; set; }
    }
}