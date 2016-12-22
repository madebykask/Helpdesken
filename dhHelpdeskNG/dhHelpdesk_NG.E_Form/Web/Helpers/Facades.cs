using System.Web;
using ECT.Model.Entities;

namespace ECT.Web
{
    public static class SessionFacade
    {
        private const string CurrentUser = "CURRENT_USER";

        public static User User
        {
            get
            {
                return (User)HttpContext.Current.Session[CurrentUser];
            }
            set
            {
                if(HttpContext.Current.Session[CurrentUser] == null)
                    HttpContext.Current.Session.Add(CurrentUser, value);
                else
                    HttpContext.Current.Session[CurrentUser] = value;
            }
        }
    }
}