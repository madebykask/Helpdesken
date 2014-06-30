namespace DH.Helpdesk.Dal.Repositories.Invoice.Concrete
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Invoice;

    public class CaseInvoiceArticleRepository : Repository, ICaseInvoiceArticleRepository
    {
        private readonly IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle> caseArticleMapper;

        public CaseInvoiceArticleRepository(
                IDatabaseFactory databaseFactory, 
                IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle> caseArticleMapper)
            : base(databaseFactory)
        {
            this.caseArticleMapper = caseArticleMapper;
        }

        public CaseInvoiceArticle[] GetCaseArticles(int caseId)
        {
            var entities = this.DbContext.CaseInvoiceArticles
                        .Where(a => a.CaseId == caseId);

            return entities
                    .Select(a => this.caseArticleMapper.Map(a))
                    .ToArray();
        }
    }
}