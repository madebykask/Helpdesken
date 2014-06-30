namespace DH.Helpdesk.Dal.Repositories.Invoice.Concrete
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Invoice;

    public class InvoiceArticleRepository : Repository, IInvoiceArticleRepository
    {
        private readonly IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper;

        public InvoiceArticleRepository(
                IDatabaseFactory databaseFactory, 
                IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper)
            : base(databaseFactory)
        {
            this.articleMapper = articleMapper;
        }

        public InvoiceArticle[] GetArticles(int customerId, int productAreaId)
        {
            var entities = this.DbContext.InvoiceArticles
                        .Where(a => a.CustomerId == customerId &&
                                a.ProductAreaId == productAreaId)
                        .ToList();

            return entities
                    .Select(a => this.articleMapper.Map(a))
                    .ToArray();
        }
    }
}