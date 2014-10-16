namespace DH.Helpdesk.Domain.MailTemplates
{
    using DH.Helpdesk.Domain.Interfaces;

    public class MailTemplateLanguageEntity : ILanguageEntity
    {
        #region Public Properties

        public string Body { get; set; }

        public virtual Language Language { get; set; }

        public int Language_Id { get; set; }

        public string MailFooter { get; set; }

        public virtual MailTemplateEntity MailTemplate { get; set; }

        public string MailTemplateName { get; set; }

        public int MailTemplate_Id { get; set; }

        public string Subject { get; set; }

        #endregion
    }
}