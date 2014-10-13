namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using DH.Helpdesk.Services.DisplayValues;

    public class CircularPartOverview
    {
        public CircularPartOverview()
        {

        }

        public CircularPartOverview(int caseId, int caseNumber, string caption, string email, bool isSent)
        {
            this.CaseId = caseId;
            this.CaseNumber = caseNumber;
            this.Caption = caption;
            this.Email = email;
            this.IsSent = (BooleanDisplayValue)isSent;
        }

        public int CaseId { get; set; }

        public int CaseNumber { get; set; }

        public string Caption { get; set; }

        public string Email { get; set; }

        public BooleanDisplayValue IsSent { get; set; }
    }
}