namespace DH.Helpdesk.Services.BusinessLogic.Specifications.EmailTemplate
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Domain.MailTemplates;

    public static class MailTemplateLanguageSpecifications
    {
        public static MailTemplate ExtractMailTemplate(
            this IQueryable<MailTemplateLanguageEntity> query,
            IQueryable<MailTemplateEntity> templates,
            int customerId,
            int languageId,
            int templateId)
        {
            var nullableCustomerId = (int?)customerId;

            var anonymus = (from l in query.GetByLanguage(languageId)
                            join t in templates.GetByNullableCustomer(nullableCustomerId).GetMailIdTemplates(templateId) on
                                l.MailTemplate_Id equals t.Id
                            select new { l.Body, l.Subject }).FirstOrDefault();

            if (anonymus == null)
            {
                return new MailTemplate(string.Empty, string.Empty);
            }

            var mailTemplate = new MailTemplate(anonymus.Subject, anonymus.Body);

            return mailTemplate;
        }

        public static MailTemplate ExtractMailTemplateById(
            this IQueryable<MailTemplateLanguageEntity> query,
            IQueryable<MailTemplateEntity> templates,
            int customerId,
            int languageId,
            int id)
        {
            var nullableCustomerId = (int?)customerId;

            var anonymus = (from l in query.GetByLanguage(languageId)
                            join t in templates.GetByNullableCustomer(nullableCustomerId).Where(x => x.Id == id) on
                                l.MailTemplate_Id equals t.Id
                            select new { l.Body, l.Subject }).FirstOrDefault();

            if (anonymus == null)
            {
                return new MailTemplate(string.Empty, string.Empty);
            }

            var mailTemplate = new MailTemplate(anonymus.Subject, anonymus.Body);

            return mailTemplate;
        }
    }
}
