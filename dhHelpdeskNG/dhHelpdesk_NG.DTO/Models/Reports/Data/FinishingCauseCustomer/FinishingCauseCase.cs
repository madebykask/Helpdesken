namespace DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCustomer
{
    public sealed class FinishingCauseCase
    {
        public int CaseId { get; set; }
        
        public int? DepartmentId { get; set; }

        public int? FinishingCause { get; set; }
    }
}