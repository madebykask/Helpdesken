namespace DH.Helpdesk.Dal.Repositories.MailTemplates
{
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.MailTemplates;

    public interface IMailTemplateLanguageRepository : IRepository<MailTemplateLanguageEntity>
    {
        MailTemplateLanguageEntity GetMailTemplateForCustomerAndLanguage(
            int customerId,
            int languageId,
            int mailTemplateId);

        MailTemplateLanguageEntity GetMailTemplateLanguageForCustomer(int Id, int customerId, int languageId);

        MailTemplate GetTemplate(int mailTemplateId, int languageId);
    }
}