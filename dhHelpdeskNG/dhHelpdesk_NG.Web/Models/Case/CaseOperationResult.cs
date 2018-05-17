namespace DH.Helpdesk.Web.Models.Case
{
    public class CaseOperationResult
    {
        public int CaseId { get; set; }
        public string CaseNumber { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}