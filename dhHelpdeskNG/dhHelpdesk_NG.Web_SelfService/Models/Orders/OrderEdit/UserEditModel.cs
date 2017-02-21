using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.SelfService.Models.Orders.FieldModels;

namespace DH.Helpdesk.SelfService.Models.Orders.OrderEdit
{
    public sealed class UserEditModel
    {
        public UserEditModel()
        {            
        }

        public UserEditModel(ConfigurableFieldModel<string> userId, ConfigurableFieldModel<string> userFirstName, ConfigurableFieldModel<string> userLastName, ConfigurableFieldModel<string> userPhone, ConfigurableFieldModel<string> userEMail)
        {
            UserId = userId;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            UserPhone = userPhone;
            UserEMail = userEMail;
        }

        public string Header { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> UserId { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> UserFirstName { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> UserLastName { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> UserPhone { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> UserEMail { get; set; }
        

        public static UserEditModel CreateEmpty()
        {
            return new UserEditModel(
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return UserId.Show ||
                UserFirstName.Show ||
                UserLastName.Show ||
                UserPhone.Show ||
                UserEMail.Show ;
        }
    }
}