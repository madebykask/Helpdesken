namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserFieldSettings
    {

        public UserFieldSettings(TextFieldSettings userId,
            TextFieldSettings userFirstName,
            TextFieldSettings userLastName,
            TextFieldSettings userPhone,
            TextFieldSettings userEMail)
        {
            UserId = userId;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            UserPhone = userPhone;
            UserEMail = userEMail;
        }

        [NotNull]
        public TextFieldSettings UserId { get; private set; }
         
        [NotNull]
        public TextFieldSettings UserFirstName { get; private set; }
         
        [NotNull]
        public TextFieldSettings UserLastName { get; private set; }
        [NotNull]
        public TextFieldSettings UserPhone { get; private set; }

        [NotNull]
        public TextFieldSettings UserEMail { get; private set; }

    }
}