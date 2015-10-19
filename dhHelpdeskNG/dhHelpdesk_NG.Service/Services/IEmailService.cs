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

        EmailResponse SendEmail(MailAddress from, List<MailAddress> recipients, Mail mail);

        EmailResponse SendEmail(MailAddress from, MailAddress recipient, Mail mail);

        EmailResponse SendEmail(MailItem mailItem);

        EmailResponse SendEmail(
            string from,
            string to,
            string subject,
            string body,
            List<Field> fields,            
            string mailMessageId = "",
            bool highPriority = false,
            List<string> files = null);

        EmailResponse SendEmail(EmailItem item);

        #endregion
    }
}