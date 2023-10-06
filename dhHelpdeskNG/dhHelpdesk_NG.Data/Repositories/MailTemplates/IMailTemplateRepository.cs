namespace DH.Helpdesk.Dal.Repositories.MailTemplates
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;

    public interface IMailTemplateRepository : IRepository<MailTemplateEntity>
    {
        IEnumerable<MailTemplateList> GetMailTemplates(int customerId, int languageId);

        CustomMailTemplate GetCustomMailTemplate(int mailTemplateId);

        List<CustomMailTemplate> GetCustomMailTemplatesFull(int customerId);
        List<CustomMailTemplate> GetCustomMailTemplatesList(int customerId);

        int? GetTemplateId(ChangeTemplate template, int customerId);

        int GetNewMailTemplateMailId();
        int GetMailTemlpateMailId(int templateId);

        MailTemplateEntity GetMailTemplateForCustomer(int id, int customerId, int languageId);
        IEnumerable<MailTemplateList> GetAllMailTemplatesForCustomer(int customerId);
    }
}