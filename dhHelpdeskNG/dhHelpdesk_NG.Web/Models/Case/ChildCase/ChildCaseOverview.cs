namespace DH.Helpdesk.Web.Models.Case.ChildCase
{
    public class ChildCaseOverview
    {
        public int Id { get; set; }

        public int CaseNo { get; set; }

        public string Subject { get; set; }

        public string CaseType { get; set; }

        public string CasePerformer { get; set; }
        
        public string SubStatus { get; set; }

        public string RegistrationDate { get; set; }

        public string ClosingDate { get; set; }
    }
}