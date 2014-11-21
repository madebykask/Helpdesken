namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserFieldSettings
    {
        public UserFieldSettings(
                TextFieldSettings userId, 
                TextFieldSettings userFirstName, 
                TextFieldSettings userLastName)
        {
            this.UserLastName = userLastName;
            this.UserFirstName = userFirstName;
            this.UserId = userId;
        }

        [NotNull]
        public TextFieldSettings UserId { get; private set; }
         
        [NotNull]
        public TextFieldSettings UserFirstName { get; private set; }
         
        [NotNull]
        public TextFieldSettings UserLastName { get; private set; }                  
    }
}