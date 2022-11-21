namespace DH.Helpdesk.BusinessData.Models.Case.CaseHistory
{
    public class EmailLogsOverview
    {
        public EmailLogsOverview()
        {
        }

        public EmailLogsOverview(int id, int mailId, string emailAddress, string ccEmailAddress, string responseMessage, string body)
        {
            Id = id;
            MailId = mailId;
            EmailAddress = emailAddress;
            CcEmailAddress = ccEmailAddress;
            ResponseMessage = responseMessage;
            Body = body;
            
        }

        public int Id { get; set; }
        public int MailId { get; set; }
        public string EmailAddress { get; set; }
        public string CcEmailAddress { get; set; }
        public string ResponseMessage { get; set; }
        public string Body { get; set; }
    }
}