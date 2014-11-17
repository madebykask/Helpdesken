namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public sealed class UserOverview
    {
        public UserOverview(
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