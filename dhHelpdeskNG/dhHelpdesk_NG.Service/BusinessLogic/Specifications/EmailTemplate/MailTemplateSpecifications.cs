namespace DH.Helpdesk.Services.BusinessLogic.Specifications.EmailTemplate
{
    using System.Linq;

    using DH.Helpdesk.Domain.MailTemplates;

    public static class MailTemplateSpecifications
    {
        public static IQueryable<MailTemplateEntity> GetMailIdTemplates(
            this IQueryable<MailTemplateEntity> query,
            int mailId)
        {
            query = query.Where(x => x.MailID == mailId);
            return query;
        }
    }
}