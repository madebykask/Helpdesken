namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserFieldSettings
    {
        public UserFieldSettings(
                FieldSettings userId, 
                FieldSettings userFirstName, 
                FieldSettings userLastName)
        {
            this.UserLastName = userLastName;
            this.UserFirstName = userFirstName;
            this.UserId = userId;
        }

        [NotNull]
        public FieldSettings UserId { get; private set; }
         
        [NotNull]
        public FieldSettings UserFirstName { get; private set; }
         
        [NotNull]
        public FieldSettings UserLastName { get; private set; }                  
    }
}