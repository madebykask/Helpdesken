using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Infrastructure
{
    public class UserCaseInfo
    {
        public int UserId { get; set; }
        public int CaseId { get; set; }
        public DateTime Looked { get; set; }
    }

    public class LoggedInUsers
    {
        public decimal CaseNumber { get; set; }
        public int Customer_Id { get; set; }
        public int User_Id { get; set; }
        public string CustomerName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public DateTime LatestActivity { get; set; }
        public DateTime LoggedOnLastTime { get; set; }

    }

    public static class ApplicationFacade
    {
        private const string _USER_CASE_INFO = "USER_CASE_INFO";
        private const string _USER_LOGGED_IN = "USER_LOGGED_IN";

        public static IList<UserCaseInfo> UserCaseInfo
        {
            get
            {
                return (IList<UserCaseInfo>)HttpContext.Current.Application[_USER_CASE_INFO];
            }
            private set {}
        }

        public static UserCaseInfo GetUserCaseInfo(int caseId)
        {
            if(UserCaseInfo == null) return null;

            return UserCaseInfo.Where(x => x.CaseId == caseId).FirstOrDefault();
        }

        public static void AddCaseUserInfo(int userId, int caseId)
        {
            if(UserCaseInfo == null)
            {
                HttpContext.Current.Application[_USER_CASE_INFO] = new List<UserCaseInfo>();
            }

            if(UserCaseInfo.Where(x=>x.UserId == userId && x.CaseId == caseId).ToList().Count == 0)
                UserCaseInfo.Add(new UserCaseInfo { UserId = userId, CaseId = caseId, Looked = DateTime.UtcNow });
        }

        public static void RemoveCaseUserInfo(int userId)
        {
            if(UserCaseInfo == null) return;

            var userCaseInfos = UserCaseInfo.Where(x => x.UserId == userId).ToList();
            foreach(var userCaseInfo in userCaseInfos)
            {
                UserCaseInfo.Remove(userCaseInfo);
            }
        }

        public static UserCaseInfo GetUserCaseInfoByUser(int userId)
        {
            if (UserCaseInfo == null) return null;


            return UserCaseInfo.Where(x => x.UserId == userId).FirstOrDefault();
        }


        public static IList<LoggedInUsers> LoggedInUsers
        {
            get
            {
                return (IList<LoggedInUsers>)HttpContext.Current.Application[_USER_LOGGED_IN];
            }
            private set { }
        }

        public static IList<LoggedInUsers> GetLoggedInUsers(int customerId)
        {
            if (LoggedInUsers == null) return null;

            var loggedInUsers = LoggedInUsers.Where(x => x.Customer_Id == customerId).ToList();

            return loggedInUsers;
        }

        public static void AddLoggedInUser(LoggedInUsers loggedInUsers)
        {
            if (LoggedInUsers == null)
            {
                HttpContext.Current.Application[_USER_LOGGED_IN] = new List<LoggedInUsers>();
            }

            LoggedInUsers.Add(loggedInUsers);
        }
    }
}