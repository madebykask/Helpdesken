using DH.Helpdesk.BusinessData.Models.MailTemplates;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.MailTemplates;

namespace DH.Helpdesk.Dal.Repositories.MailTemplates
{
    public interface IMailTemplateLanguageRepository : IRepository<MailTemplateLanguageEntity>
    {
        MailTemplateLanguageEntity GetMailTemplateForCustomerAndLanguage(
            int customerId,
            int languageId,
            int mailTemplateId,
            int? orderTypeId = null);

        MailTemplate GetTemplate(int mailTemplateId, int languageId);

        MailTemplateLanguageEntity GetMailTemplateLanguageForCustomerToSave(int Id, int customerId, int languageId);

        MailTemplateLanguageEntity GetMailTemplateLanguageForCustomerToSave(int Id, int customerId, int languageId, int orderTypeId);
    }
}