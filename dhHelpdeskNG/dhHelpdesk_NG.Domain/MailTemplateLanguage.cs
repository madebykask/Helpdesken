namespace DH.Helpdesk.Domain
{
    public class MailTemplateLanguage
    {
        public int MailTemplate_Id { get; set; }
        public int Language_Id { get; set; }
        public string Body { get; set; }
        public string MailFooter { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }

        public virtual Language Language { get; set; }
        public virtual MailTemplate MailTemplate { get; set; }
    }
}
