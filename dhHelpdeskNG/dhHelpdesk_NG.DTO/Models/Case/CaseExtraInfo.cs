using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public sealed class CaseExtraInfo
    {
        public CaseExtraInfo()
        {            
        }

        public string CreatedByApp  { get; set; }

        public int LeadTimeForNow { get; set; }

        public int ActionLeadTime { get; set; }

        public int ActionExternalTime { get; set; }

        public static CaseExtraInfo CreateHelpdesk5()
        {
            return new CaseExtraInfo
            {
                ActionExternalTime = 0,
                ActionLeadTime = 0,
                CreatedByApp = CreatedByApplications.Helpdesk5,
                LeadTimeForNow = 0
            };
        }
    }
}