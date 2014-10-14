namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Read
{
    using System;

    public sealed class ConnectedCase
    {
        public ConnectedCase(int caseId, int caseNumber, string caption, string email, Guid guid, bool isSent)
        {
            this.CaseId = caseId;
            this.CaseNumber = caseNumber;
            this.Caption = caption;
            this.Email = email;
            this.Guid = guid;
            this.IsSent = isSent;
        }

        public int CaseId { get; set; }

        public int CaseNumber { get; set; }

        public string Caption { get; set; }

        public string Email { get; set; }

        public Guid Guid { get; set; }

        public bool IsSent { get; set; }
    }
}