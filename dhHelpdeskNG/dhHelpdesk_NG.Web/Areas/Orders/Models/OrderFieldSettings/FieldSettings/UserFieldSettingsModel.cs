namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class UserFieldSettingsModel
    {
        public UserFieldSettingsModel()
        {            
        }

        public UserFieldSettingsModel(
                TextFieldSettingsModel userId, 
                TextFieldSettingsModel userFirstName, 
                TextFieldSettingsModel userLastName)
        {
            this.UserLastName = userLastName;
            this.UserFirstName = userFirstName;
            this.UserId = userId;
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
    }
}