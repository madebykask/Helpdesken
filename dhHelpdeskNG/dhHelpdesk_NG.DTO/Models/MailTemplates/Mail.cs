namespace DH.Helpdesk.BusinessData.Models.MailTemplates
{
    public sealed class Mail
    {
        public Mail(string subject, string body)
        {
            this.Subject = subject;
            this.Body = body;
        }

        public string Subject { get; private set; }

        public string Body { get; private set; }
    }
}