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
        private readonly IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle> toBusinessModelMapper;

        private readonly IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity> toEntityMapper;

        public CaseInvoiceArticleRepository(
                IDatabaseFactory databaseFactory, 
                IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle> toBusinessModelMapper, 
                IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity> toEntityMapper)
            : base(databaseFactory)
        {
            this.toBusinessModelMapper = toBusinessModelMapper;
            this.toEntityMapper = toEntityMapper;
        }

        public CaseInvoiceArticle[] GetCaseArticles(int caseId)
        {
            var entities = this.DbContext.CaseInvoiceArticles
                         .Where(a => a.CaseId == caseId)
                         .OrderBy(a => a.Position)
                         .ToList();

            return entities
                    .Select(a => this.toBusinessModelMapper.Map(a))
                    .ToArray();
        }

        public void SaveCaseArticles(int caseId, CaseInvoiceArticle[] articles)
        {
            if (articles == null)
            {
                return;
            }

            var ids = articles
                    .Where(a => !a.IsNew())
                    .Select(a => a.Id);
            var articlesForDelete = this.DbContext.CaseInvoiceArticles
                                .Where(a => a.CaseId == caseId &&
                                    !ids.Contains(a.Id))
                                .ToList();
            foreach (var articleForDelete in articlesForDelete)
            {
                this.DbContext.CaseInvoiceArticles.Remove(articleForDelete);
                this.Commit();
            }

            foreach (var article in articles)
            {
                CaseInvoiceArticleEntity entity;
                if (article.IsNew())
                {
                    entity = new CaseInvoiceArticleEntity();
                    this.toEntityMapper.Map(article, entity);
                    entity.CaseId = caseId;
                    this.DbContext.CaseInvoiceArticles.Add(entity);
                    this.Commit();
                    continue;
                }

                entity = this.DbContext.CaseInvoiceArticles.Find(article.Id);
                this.toEntityMapper.Map(article, entity);
                this.Commit();                            
            }
        }

        public void DeleteCaseArticles(int caseId)
        {
            var articlesForDelete = this.DbContext.CaseInvoiceArticles
                                .Where(a => a.CaseId == caseId)
                                .ToList();
            foreach (var articleForDelete in articlesForDelete)
            {
                this.DbContext.CaseInvoiceArticles.Remove(articleForDelete);
                this.Commit();
            }
        }
    }
}