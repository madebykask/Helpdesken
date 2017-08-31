
using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class AuxCaseModel
    {
        public AuxCaseModel(int currentLanguageId, int currentUserId, 
                            string userIdentityName, string aboluteUrl,
                            string currentApp, TimeZoneInfo userTimeZone = null)
        {
            UtcNow = DateTime.UtcNow;
            CurrentLanguageId = currentLanguageId;
            CurrentUserId = currentUserId;            
            UserIdentityName = userIdentityName;
            AbsolutreUrl = aboluteUrl;
            CurrentApp = currentApp;
            UserTimeZone = userTimeZone;
        }

        public DateTime UtcNow { get; }

        public int CurrentLanguageId { get; }

        public int CurrentUserId { get; }        

        public string UserIdentityName { get; }

        public string AbsolutreUrl { get; }

        public string CurrentApp { get; }

        public TimeZoneInfo UserTimeZone { get; set; }

    }
}
