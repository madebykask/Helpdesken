using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
