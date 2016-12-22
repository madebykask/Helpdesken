namespace ECT.Model.Entities
{
    public class MailTemplate
    {
        public int Id { get; set; }
        public int Customer_Id { get; set; }
        public int MailId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
