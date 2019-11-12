namespace DH.Helpdesk.BusinessData.Models.Case.CaseHistory
{
    public class EmailLogsOverview
    {
        public EmailLogsOverview()
        {
        }

        public EmailLogsOverview(int id, int mailId, string emailAddress, string ccEmailAddress)
        {
            Id = id;
            MailId = mailId;
            EmailAddress = emailAddress;
            CcEmailAddress = ccEmailAddress;
        }

        public int Id { get; set; }
        public int MailId { get; set; }
        public string EmailAddress { get; set; }
        public string CcEmailAddress { get; set; }
    }
}