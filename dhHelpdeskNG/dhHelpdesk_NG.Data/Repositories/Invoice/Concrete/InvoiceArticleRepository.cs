using System;

namespace DH.Helpdesk.Dal.Repositories.Invoice.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Invoice;

    public class InvoiceArticleRepository : Repository, IInvoiceArticleRepository
    {
        private readonly IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper;

        private readonly IBusinessModelToEntityMapper<InvoiceArticle, InvoiceArticleEntity> toEntityMapper;

        public InvoiceArticleRepository(
                IDatabaseFactory databaseFactory, 
                IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper, 
                IBusinessModelToEntityMapper<InvoiceArticle, InvoiceArticleEntity> toEntityMapper)
            : base(databaseFactory)
        {
            this.articleMapper = articleMapper;
            this.toEntityMapper = toEntityMapper;
        }

        public InvoiceArticle[] GetArticles(int customerId, int productAreaId)
        {
            var productAreas = new List<int> { productAreaId };
            productAreas.AddRange(this.GetProductAreaChildren(productAreaId));

            var entities = this.DbContext.InvoiceArticles
                        .Where(a => a.CustomerId == customerId && (a.Blocked == false || a.Blocked == null) && a.IsActive == 1 &&
                            a.ProductAreas.Where(p=>productAreas.Contains(p.Id)).Any()) //does this work?
                        .OrderBy(a => a.Number)
                        .ToList();

            return entities
                    .Select(a => this.articleMapper.Map(a))
                    .ToArray();
        }

        public InvoiceArticle[] GetArticles(int customerId)
        {
            var entities = this.DbContext.InvoiceArticles
                        .Where(a => a.CustomerId == customerId)
                        .OrderBy(a => a.Number)
                        .ToList();

            return entities
                    .Select(a => this.articleMapper.Map(a))
                    .ToArray();
        }

		public List<InvoiceArticle> GetActiveArticles(int customerId)
		{
			var entities = this.DbContext.InvoiceArticles
						.Where(a => a.CustomerId == customerId && (a.Blocked == false || a.Blocked == null) && a.IsActive == 1)
						.OrderBy(a => a.Number)
						.ToList();

			return entities
					.Select(a => this.articleMapper.Map(a))
					.ToList();
		}

		public int SaveArticle(InvoiceArticle article)
        {
            InvoiceArticleEntity entity;
            if (article.Id > 0)
            {
                entity = this.DbContext.InvoiceArticles.Find(article.Id);
                this.toEntityMapper.Map(article, entity);
            }
            else
            {
                entity = new InvoiceArticleEntity();
                this.toEntityMapper.Map(article, entity);
                this.DbContext.InvoiceArticles.Add(entity);
            }

            this.Commit();
            return entity.Id;
        }

        public void SaveArticleProductArea(InvoiceArticleProductAreaSelectedFilter selectedItems) //,userId
        {

            var articles = selectedItems.SelectedInvoiceArticles;
            var productareas = selectedItems.SelectedProductAreas;

            var existsRecords = this.DbContext.InvoiceArticles.Where(i => articles.Contains(i.Id)).ToList();

            var existsRecordsStr = new List<string>();
            foreach (var ex in existsRecords)
            {
                var ps = ex.ProductAreas.Where(p => productareas.Contains(p.Id)).ToList();
                foreach (var p in ps)
                    existsRecordsStr.Add(string.Format("{0}-{1}", ex.Id, p.Id));
            }

            var invoiceArticleEntities = this.DbContext.InvoiceArticles.Where(i => articles.Contains(i.Id)).ToList();
            var productAreaEntities = this.DbContext.ProductAreas.Where(p => productareas.Contains(p.Id)).ToList();

            var hasNewRecord = false;
            foreach (var art in invoiceArticleEntities)
            {                
                foreach (var prod in productAreaEntities)
                {
                    var rowStr = string.Format("{0}-{1}", art, prod);
                    if (!existsRecordsStr.Contains(rowStr))
                    {
                        hasNewRecord = true;
                        art.ProductAreas.Add(prod);                        
                        //TODO:
                        //entity.UserId = userId;
                        //this.DbContext.InvoiceArticleProductArea.Add(entity);                        
                    }
                }
            }

            if (hasNewRecord)
                this.Commit();           
        }

        public void DeleteArticleProductArea(int articleid, int productareaid)
        {
            var invoiceArticleEntities = this.DbContext.InvoiceArticles.Where(i => i.Id == articleid).ToList();

            var productAreaEntities = this.DbContext.ProductAreas.Where(p => p.Id == productareaid).ToList();

            foreach (var art in invoiceArticleEntities)
            {
                foreach (var prod in productAreaEntities)
                {
                    art.ProductAreas.Remove(prod);
                }
            }

            this.Commit();
        }

        public void DeactivateArticlesBySyncDate(int customerId, DateTime lastSyncDate)
        {
            var articleEntities = DbContext.InvoiceArticles.Where(i => i.CustomerId == customerId && i.LastSyncedDate < lastSyncDate).ToList();
            articleEntities.ForEach(x => x.IsActive = 0);

            this.Commit();
        }

        public void SaveArticles(List<InvoiceArticle> articles)
        {
            foreach (var article in articles)
            {
                if (article.Id > 0)
                {
                    var entity = this.DbContext.InvoiceArticles.Find(article.Id);
                    this.toEntityMapper.Map(article, entity);
                }
                else
                {
                    var entity = new InvoiceArticleEntity();
                    this.toEntityMapper.Map(article, entity);
                    this.DbContext.InvoiceArticles.Add(entity);
                }
            }

            this.Commit();
        }

        private IEnumerable<int> GetProductAreaChildren(int parentId)
        {
            var children = new List<int>();
            this.GetProductAreaChildrenProcess(parentId, children);
            return children.ToArray();
        }

        private void GetProductAreaChildrenProcess(int parentId, List<int> list)
        {
            var children = this.DbContext.ProductAreas
                        .Where(a => a.Parent_ProductArea_Id == parentId)
                        .Select(a => a.Id)
                        .ToList();

            foreach (var child in children)
            {
                list.Add(child);
                this.GetProductAreaChildrenProcess(child, list);
            }
        }
    }
}