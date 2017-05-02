
using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class AuxCaseModel
    {
        public AuxCaseModel(int currentLanguageId, TimeZoneInfo userTimeZone)
        {
            UtcNow = DateTime.UtcNow;
            CurrentLanguageId = currentLanguageId;
            UserTimeZone = userTimeZone;
        }

        public DateTime UtcNow { get; }

        public int CurrentLanguageId { get; }

        public TimeZoneInfo UserTimeZone { get; }

    }
}
