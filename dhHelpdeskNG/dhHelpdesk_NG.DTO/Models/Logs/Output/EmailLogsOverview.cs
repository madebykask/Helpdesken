namespace DH.Helpdesk.BusinessData.Models.Logs.Output
{
    public class EmailLogsOverview
    {
        public EmailLogsOverview(int id, string emailAddress)
        {
            Id = id;
            EmailAddress = emailAddress;
        }

        public int Id { get; set; }
        public string EmailAddress { get; set; }
    }
}