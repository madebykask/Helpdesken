using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region DOCUMENT

    public interface IDocumentRepository : IRepository<Document>
    {
    }

    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        public DocumentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region DOCUMENTCATEGORY

    public interface IDocumentCategoryRepository : IRepository<DocumentCategory>
    {
    }

    public class DocumentCategoryRepository : RepositoryBase<DocumentCategory>, IDocumentCategoryRepository
    {
        public DocumentCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
