namespace DH.Helpdesk.Dal.Repositories.MailTemplates
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.MailTemplates;

    public interface IMailTemplateRepository : IRepository<MailTemplateEntity>
    {
        IEnumerable<MailTemplateList> GetMailTemplate(int customerId, int languageId);

        int GetTemplateId(ChangeTemplate template, int customerId);
    }
}