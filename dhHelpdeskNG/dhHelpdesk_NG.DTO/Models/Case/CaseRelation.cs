using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseRelation
    {
        public int Id { get; set; }
        public Decimal CaseNumber { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public DateTime? FinishingDate { get; set; }
    }
}

