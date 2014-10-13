namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Read
{
    public sealed class CircularPart 
    {
        public CircularPart(int caseId, int caseNumber, string caption, string email, bool isSent)
        {
            this.CaseId = caseId;
            this.CaseNumber = caseNumber;
            this.Caption = caption;
            this.Email = email;
            this.IsSent = isSent;
        }

        public int CaseId { get; set; }

        public int CaseNumber { get; set; }

        public string Caption { get; set; }

        public string Email { get; set; }

        public bool IsSent { get; set; }
    }
}