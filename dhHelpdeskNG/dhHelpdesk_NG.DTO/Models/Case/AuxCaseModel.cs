
using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class AuxCaseModel
    {
        public AuxCaseModel(int currentLanguageId, int currentUserId, 
                            TimeZoneInfo userTimeZone, string userIdentityName)
        {
            UtcNow = DateTime.UtcNow;
            CurrentLanguageId = currentLanguageId;
            CurrentUserId = currentUserId;
            UserTimeZone = userTimeZone;
            UserIdentityName = userIdentityName;
        }

        public DateTime UtcNow { get; }

        public int CurrentLanguageId { get; }

        public int CurrentUserId { get; }

        public TimeZoneInfo UserTimeZone { get; }

        public string UserIdentityName { get; }

    }
}
