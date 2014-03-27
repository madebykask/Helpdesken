namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.MailTemplates;

    public class MailTemplateIdentifierRepository : RepositoryBase<MailTemplateIdentifierEntity>, IMailTemplateIdentifierRepository
    {
        public MailTemplateIdentifierRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}