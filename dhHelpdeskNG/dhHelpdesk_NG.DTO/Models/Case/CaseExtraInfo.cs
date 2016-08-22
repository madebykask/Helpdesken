namespace DH.Helpdesk.BusinessData.Models.Case
{
    public sealed class CaseExtraInfo
    {
        public CaseExtraInfo()
        {            
        }

        public string CreatedByApp  { get; set; }

        public int LeadTimeForNow { get; set; }
    }
}