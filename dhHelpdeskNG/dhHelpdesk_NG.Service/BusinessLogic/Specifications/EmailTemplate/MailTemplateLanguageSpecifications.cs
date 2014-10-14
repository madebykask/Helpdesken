namespace DH.Helpdesk.Services.BusinessLogic.Specifications.EmailTemplate
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Domain.MailTemplates;

    public static class MailTemplateLanguageSpecifications
    {
        public static IQueryable<MailTemplateLanguageEntity> GetLanguageTemplates(
            this IQueryable<MailTemplateLanguageEntity> query,
            int languageId)
        {
            query = query.Where(x => x.Language_Id == languageId);
            return query;
        }

        public static MailTemplate ExtractMailTemplate(
            this IQueryable<MailTemplateLanguageEntity> query,
            IQueryable<MailTemplateEntity> templates,
            int customerId,
            int languageId,
            int templateId)
        {
            var anonymus = (from l in query.GetLanguageTemplates(languageId)
                            join t in templates.GetByCustomer((int?)customerId).GetMailIdTemplates(templateId) on
                                l.MailTemplate_Id equals t.Id
                            select new { l.Body, l.Subject }).Single();

            var mailTemplate = new MailTemplate(anonymus.Subject, anonymus.Body);

            return mailTemplate;
        }
    }
}
