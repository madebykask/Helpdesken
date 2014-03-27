namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.MailTemplates;

    public interface IMailTemplateIdentifierRepository : IRepository<MailTemplateIdentifierEntity>
    {
        // expandable ....
    }
}