using DH.Helpdesk.BusinessData.Models.MailTemplates;
using DH.Helpdesk.BusinessData.OldComponents;

namespace DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Email;
    using DH.Helpdesk.Domain;

    public sealed class EmailFactory : IEmailFactory
    {
        public EmailItem CreateEmailItem(
            string fromAddress,
            string to,
            string subject,
            string body,
            List<Field> fields,
            string mailMessageId,
            bool isHighPriority,
            List<MailFile> files)
        {
            if (files == null)
                files = new List<MailFile>();

            var instance = new EmailItem(
                            fromAddress,
                            to,
                            subject,
                            body,
                            fields,
                            mailMessageId,
                            isHighPriority,
                            files);

            return instance;
        }

        public EmailLog CreateEmailLog(
            int caseHistoryId, 
            GlobalEnums.MailTemplates mailId, 
            string email, 
            string messageId)
        {
            var instance = new EmailLog(
                            caseHistoryId,
                            (int)mailId,
                            email,
                            messageId);

            return instance;
        }
    }
}