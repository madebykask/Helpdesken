namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;

    public class ExtraFieldCaseHistory 
    {                
        public string CaseFile { get; set; }

        public string LogFile { get; set; }

        public string CaseLog { get; set; }

        public string ClosingReason { get; set; }

        public int LeadTime { get; set; } 
    }
}
