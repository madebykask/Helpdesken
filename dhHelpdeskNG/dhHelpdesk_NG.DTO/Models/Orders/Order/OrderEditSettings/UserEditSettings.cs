namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserEditSettings
    {
        public UserEditSettings(
                TextFieldEditSettings userId, 
                TextFieldEditSettings userFirstName, 
                TextFieldEditSettings userLastName)
        {
            this.UserLastName = userLastName;
            this.UserFirstName = userFirstName;
            this.UserId = userId;
        }

        [NotNull]
        public TextFieldEditSettings UserId { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings UserFirstName { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings UserLastName { get; private set; }     
    }
}