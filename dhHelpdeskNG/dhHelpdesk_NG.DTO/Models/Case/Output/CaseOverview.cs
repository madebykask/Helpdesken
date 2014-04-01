using System;

namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
    public sealed class CaseOverview
    {
        public int Customer_Id { get; set; }
        public DateTime? FinishingDate { get; set; }
        public int Deleted { get; set; }
        public int User_Id { get; set; }
        public int? Status_Id { get; set; }
    }
}