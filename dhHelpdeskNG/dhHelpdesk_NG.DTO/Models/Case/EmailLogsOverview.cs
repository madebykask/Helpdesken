namespace DH.Helpdesk.BusinessData.Models.Case.CaseHistory
{
    public class EmailLogsOverview
    {
        public EmailLogsOverview(int id, int mailId, string emailAddress)
        {
            Id = id;
            MailId = mailId;
            EmailAddress = emailAddress;
        }

        public int Id { get; }
        public int MailId { get; }
        public string EmailAddress { get; }
    }
}