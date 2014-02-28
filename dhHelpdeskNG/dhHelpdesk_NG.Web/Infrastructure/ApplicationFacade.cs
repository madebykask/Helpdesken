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

    public static class ApplicationFacade
    {
        private const string _USER_CASE_INFO = "USER_CASE_INFO";

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
    }
}