namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserEditSettings
    {
        public UserEditSettings(TextFieldEditSettings userId,
            TextFieldEditSettings userFirstName,
            TextFieldEditSettings userLastName,
            TextFieldEditSettings userPhone,
            TextFieldEditSettings userEMail)
        {
            UserId = userId;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            UserPhone = userPhone;
            UserEMail = userEMail;
        }

        [NotNull]
        public TextFieldEditSettings UserId { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings UserFirstName { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings UserLastName { get; private set; }

        [NotNull]
        public TextFieldEditSettings UserPhone { get; private set; }

        [NotNull]
        public TextFieldEditSettings UserEMail { get; private set; }
    }
}