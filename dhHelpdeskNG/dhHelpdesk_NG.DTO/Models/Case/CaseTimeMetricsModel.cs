using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseTimeMetricsModel
    {      
        public int ExternalTime { get; set; }
        public int ActionExternalTime { get; set; }
        public int LeadTime { get; set; }
        public int LeadTimeForNow { get; set; }
        public int ActionLeadTime { get; set; }
        public DateTime? LatestSLACountDate { get; set; }
        
    }
}
