namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using DH.Helpdesk.Services.DisplayValues;

    public class ConnectedToCircularOverview
    {
        public ConnectedToCircularOverview()
        {

        }

        public ConnectedToCircularOverview(
            int id,
            int circularId,
            int caseId,
            int caseNumber,
            string caption,
            string email,
            BooleanDisplayValue isSent)
        {
            this.Id = id;
            this.CircularId = circularId;
            this.CaseId = caseId;
            this.CaseNumber = caseNumber;
            this.Caption = caption;
            this.Email = email;
            this.IsSent = isSent;
        }

        public int Id { get; set; }

        public int CircularId { get; set; }

        public int CaseId { get; set; }

        public int CaseNumber { get; set; }

        public string Caption { get; set; }

        public string Email { get; set; }

        public BooleanDisplayValue IsSent { get; set; }

        public string DisplaySent
        {
            get
            {
                return this.IsSent.GetDisplayValue();
            }
        }
    }
}