namespace DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Email;
    using DH.Helpdesk.Domain;

    public interface IEmailFactory
    {
        EmailItem CreateEmailItem(
            string fromAddress,
            string to,
            string subject,
            string body,
            List<Field> fields,
            string mailMessageId,
            bool isHighPriority,
            List<string> files);

        EmailLog CreatEmailLog(
            int caseHistoryId, 
            int mailId, 
            string email, 
            string messageId);
    }
}