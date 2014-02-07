namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IDocumentationRepository : IRepository<Documentation>
    {
    }

    public class DocumentationRepository : RepositoryBase<Documentation>, IDocumentationRepository
    {
        public DocumentationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
