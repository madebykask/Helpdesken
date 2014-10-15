namespace DH.Helpdesk.BusinessData.Models.Email
{
    using System;

    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class QuestionnaireMailItem
    {
        public QuestionnaireMailItem(Guid guid, MailItem mailItem)
        {
            this.Guid = guid;
            this.MailItem = mailItem;
        }

        public QuestionnaireMailItem(Guid guid, string @from, string to, Mail mail)
        {
            this.Guid = guid;
            this.MailItem = new MailItem(@from, to, mail);
        }

        [NotNull]
        public Guid Guid { get; private set; }

        [NotNull]
        public MailItem MailItem { get; private set; }
    }
}