namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public sealed class UserOverview
    {
        public UserOverview(string userId, string userFirstName, string userLastName, string userPhone, string userEMail)
        {
            UserId = userId;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            UserPhone = userPhone;
            UserEMail = userEMail;
        }

        public string UserId { get; private set; }
        
        public string UserFirstName { get; private set; }
        
        public string UserLastName { get; private set; }

        public string UserPhone { get; private set; }

        public string UserEMail { get; private set; }
    }
}