namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    public sealed class UserEditFields
    {
        public UserEditFields(
                string userId, 
                string userFirstName, 
                string userLastName)
        {
            this.UserLastName = userLastName;
            this.UserFirstName = userFirstName;
            this.UserId = userId;
        }

        public string UserId { get; private set; }
        
        public string UserFirstName { get; private set; }
        
        public string UserLastName { get; private set; } 
    }
}