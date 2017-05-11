
using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class AuxCaseModel
    {
        public AuxCaseModel(int currentLanguageId, int currentUserId, 
                            string userIdentityName, string aboluteUrl,
                            TimeZoneInfo userTimeZone = null)
        {
            UtcNow = DateTime.UtcNow;
            CurrentLanguageId = currentLanguageId;
            CurrentUserId = currentUserId;            
            UserIdentityName = userIdentityName;
            AbsolutreUrl = aboluteUrl;
            UserTimeZone = userTimeZone;
        }

        public DateTime UtcNow { get; }

        public int CurrentLanguageId { get; }

        public int CurrentUserId { get; }        

        public string UserIdentityName { get; }

        public string AbsolutreUrl { get; }

        public TimeZoneInfo UserTimeZone { get; set; }

    }
}
