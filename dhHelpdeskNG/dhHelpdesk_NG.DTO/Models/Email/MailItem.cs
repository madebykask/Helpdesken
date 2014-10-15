namespace DH.Helpdesk.BusinessData.Models.Email
{
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class MailItem
    {
        public MailItem(string @from, string to, Mail mail)
        {
            this.From = @from;
            this.To = to;
            this.Mail = mail;
        }

        [NotNullAndEmpty]
        public string From { get; private set; }

        [NotNullAndEmpty]
        public string To { get; private set; }

        [NotNull]
        public Mail Mail { get; private set; }
    }
}