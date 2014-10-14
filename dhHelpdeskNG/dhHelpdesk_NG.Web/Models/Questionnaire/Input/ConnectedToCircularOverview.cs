namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using System;

    using DH.Helpdesk.Services.DisplayValues;

    public class ConnectedToCircularOverview
    {
        public ConnectedToCircularOverview()
        {
        }

        public ConnectedToCircularOverview(
            int circularId,
            int caseId,
            int caseNumber,
            string caption,
            string email,
            Guid guid,
            bool isSent)
        {
            this.CircularId = circularId;
            this.CaseId = caseId;
            this.CaseNumber = caseNumber;
            this.Caption = caption;
            this.Email = email;
            this.Guid = guid;
            this.IsSent = (BooleanDisplayValue)isSent;
        }

        public int CircularId { get; set; }

        public int CaseId { get; set; }

        public int CaseNumber { get; set; }

        public string Caption { get; set; }

        public string Email { get; set; }

        public Guid Guid { get; set; }        

        public BooleanDisplayValue IsSent { get; set; }
    }
}