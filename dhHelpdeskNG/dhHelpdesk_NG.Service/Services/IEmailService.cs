namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    using System.Net.Mail;

    using DH.Helpdesk.BusinessData.Models.Email;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Domain;

    public interface IEmailService
    {
        #region Public Methods and Operators

        string GetMailMessageId(string helpdeskFromAddress);

        bool IsValidEmail(string inputEmail);

        void SendEmail(MailAddress from, List<MailAddress> recipients, Mail mail);

        void SendEmail(MailAddress from, MailAddress recipient, Mail mail);

        void SendEmail(MailItem mailItem);

        void SendEmail(
            string from,
            string to,
            string subject,
            string body,
            List<Field> fields,
            string mailMessageId = "",
            bool highPriority = false,
            List<string> files = null);

        void SendEmail(EmailItem item);

        #endregion
    }
}