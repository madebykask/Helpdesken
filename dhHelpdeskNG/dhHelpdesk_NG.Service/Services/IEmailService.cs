using DH.Helpdesk.BusinessData.Enums.Email;

namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    using System.Net.Mail;

    using DH.Helpdesk.BusinessData.Models.Email;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Domain;

    public interface IEmailService
    {
        MailMessage GetMailMessage(string from, string to, string cc, string subject, string body, List<Field> fields, string mailMessageId = "", bool highPriority = false, List<MailFile> files = null, string siteSelfService = "", string siteHelpdesk = "", EmailType emailType = EmailType.ToMail, string siteSelfServiceMergeParent = "");
        #region Public Methods and Operators

        string GetMailMessageId(string helpdeskFromAddress);

        bool IsValidEmail(string inputEmail);

        EmailResponse SendEmail(MailAddress from, List<MailAddress> recipients, Mail mail, EmailSettings emailsettings);

        EmailResponse SendEmail(MailAddress from, MailAddress recipient, Mail mailint, EmailSettings emailsettings);

        EmailResponse SendEmail(MailItem mailItem, EmailSettings emailsettings);

        EmailResponse SendEmail(
            string from,
            string to,
            string cc,
            string subject,
            string body,
            List<Field> fields,
            EmailSettings emailsettings,
            string mailMessageId = "",
            bool highPriority = false,
            List<MailFile> files = null,
            string siteSelfService = "",
            string siteHelpdesk = "",
            EmailType emailType = EmailType.ToMail,
            string siteSelfServiceMergeParent = "");

        EmailResponse SendEmail(
            EmailLog el,
            string from,
            string to,
            string cc,
            string subject,
            string body,
            List<Field> fields,
            EmailSettings emailsettings,
            string mailMessageId = "",
            bool highPriority = false,
            List<MailFile> files = null,
            string siteSelfService = "",
            string siteHelpdesk = "",
            EmailType emailType = EmailType.ToMail,
            string siteSelfServiceMergeParent = ""
            );

        EmailResponse SendEmail(EmailLog el, EmailItem item, EmailSettings emailsettings, string siteSelfService = "",
            string siteHelpdesk = "", EmailType emailType = EmailType.ToMail);

        #endregion
    }
}