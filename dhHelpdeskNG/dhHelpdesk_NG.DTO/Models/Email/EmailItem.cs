namespace DH.Helpdesk.BusinessData.Models.Email
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    public sealed class EmailItem
    {
        public EmailItem(
            string fromAddress,
            string to,
            string subject,
            string body,
            List<Field> fields,
            string mailMessageId,
            bool isHighPriority,
            List<string> files)
        {
            this.Files = files;
            this.IsHighPriority = isHighPriority;
            this.MailMessageId = mailMessageId;
            this.Fields = fields;
            this.Body = body;
            this.Subject = subject;
            this.To = to;
            this.From = fromAddress;
        }

        private EmailItem()
        {
            this.Fields = new List<Field>();
            this.IsHighPriority = false;
            this.MailMessageId = string.Empty;
            this.Files = new List<string>();
        }

        [NotNullAndEmpty]
        public string From { get; private set; }

        [NotNullAndEmpty]
        public string To { get; private set; }

        [NotNullAndEmpty]
        public string Subject { get; private set; }

        [NotNullAndEmpty]
        public string Body { get; private set; }

        [NotNull]
        public List<Field> Fields { get; private set; }

        [NotNull]
        public string MailMessageId { get; private set; }

        public bool IsHighPriority { get; private set; }

        [NotNull]
        public List<string> Files { get; private set; }
    }
}