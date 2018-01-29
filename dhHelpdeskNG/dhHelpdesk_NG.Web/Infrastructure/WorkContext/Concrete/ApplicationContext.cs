using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete
{
    public interface IApplicationContext
    {
        void AddLoggedInUser(LoggedInUsers loggedInUsers);
        void RemoveLoggedInUser(string sessionId);
        int GetLiveUserCount();
    }

    public class ApplicationContext : IApplicationContext
    {
        public void AddLoggedInUser(LoggedInUsers loggedInUsers)
        {
            ApplicationFacade.AddLoggedInUser(loggedInUsers);
        }

        public void RemoveLoggedInUser(string sessionId)
        {
            ApplicationFacade.RemoveLoggedInUser(sessionId);
        }

        public int GetLiveUserCount()
        {
            return ApplicationFacade.LoggedInUsers != null 
                ? ApplicationFacade.LoggedInUsers.Count() 
                : 0;
        }
    }
}