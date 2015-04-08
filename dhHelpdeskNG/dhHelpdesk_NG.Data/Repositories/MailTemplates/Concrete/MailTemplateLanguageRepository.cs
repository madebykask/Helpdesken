namespace DH.Helpdesk.Dal.Repositories.MailTemplates.Concrete
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories.MailTemplates;
    using DH.Helpdesk.Domain.MailTemplates;

    public sealed class MailTemplateLanguageRepository : RepositoryBase<MailTemplateLanguageEntity>,
        IMailTemplateLanguageRepository
    {
        public MailTemplateLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public MailTemplateLanguageEntity GetMailTemplateForCustomerAndLanguage(
            int customerId,
            int languageId,
            int mailTemplateId)
        {
            return (from m in this.DataContext.MailTemplates
                join l in this.DataContext.MailTemplateLanguages on m.Id equals l.MailTemplate_Id
                where m.MailID == mailTemplateId && l.Language_Id == languageId && m.Customer_Id == customerId
                select l).FirstOrDefault();

        }

        public MailTemplateLanguageEntity GetMailTemplateLanguageForCustomer(int Id, int customerId, int languageId)
        {
            return (from m in this.DataContext.MailTemplates
                join l in this.DataContext.MailTemplateLanguages on m.Id equals l.MailTemplate_Id
                where
                    m.MailID == Id && l.Language_Id == languageId
                    && (m.Customer_Id == customerId || m.Customer_Id == null)
                    orderby m.Customer_Id descending
                select l).FirstOrDefault();
        }

        public MailTemplate GetTemplate(int mailTemplateId, int languageId)
        {
            return
                this.DataContext.MailTemplateLanguages.Where(
                    tl => tl.MailTemplate_Id == mailTemplateId && tl.Language_Id == languageId)
                    .Select(tl => new { tl.Subject, tl.Body })
                    .ToList()
                    .Select(tl => new MailTemplate(tl.Subject, tl.Body))
                    .FirstOrDefault();
        }

        public MailTemplateLanguageEntity GetMailTemplateLanguageForCustomerToSave(int Id, int customerId, int languageId)
        {
            return (from m in this.DataContext.MailTemplates
                    join l in this.DataContext.MailTemplateLanguages on m.Id equals l.MailTemplate_Id
                    where m.MailID == Id && l.Language_Id == languageId && m.Customer_Id == customerId
                    select l).FirstOrDefault();
        }
    }
}