namespace DH.Helpdesk.Dal.Repositories.Invoice.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Invoice;

    public class CaseInvoiceArticleRepository : Repository, ICaseInvoiceArticleRepository
    {
        private readonly IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice> invoiceToBusinessModelMapper;

        private readonly IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity> invoiceToEntityMapper;

        private readonly IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity> orderMapper;
        
        private readonly IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity> articleMapper;

        private readonly IBusinessModelToEntityMapper<CaseInvoiceOrderFile, CaseInvoiceOrderFileEntity> filesMapper;

        public CaseInvoiceArticleRepository(
                IDatabaseFactory databaseFactory, 
                IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice> invoiceToBusinessModelMapper, 
                IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity> invoiceToEntityMapper, 
                IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity> orderMapper,                 
                IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity> articleMapper, 
                IBusinessModelToEntityMapper<CaseInvoiceOrderFile, CaseInvoiceOrderFileEntity> filesMapper)
            : base(databaseFactory)
        {
            this.invoiceToBusinessModelMapper = invoiceToBusinessModelMapper;
            this.invoiceToEntityMapper = invoiceToEntityMapper;
            this.orderMapper = orderMapper;            
            this.articleMapper = articleMapper;
            this.filesMapper = filesMapper;
        }

        public CaseInvoice[] GetCaseInvoices(int caseId)
        {
            var entities = this.DbContext.CaseInvoices
                        .Where(i => i.CaseId == caseId)
                        .ToList();

            return entities.Select(i => this.invoiceToBusinessModelMapper.Map(i)).ToArray();
        }

        public CaseInvoiceOrder GetCaseInvoiceOrder(int caseId, int invoiceOrderId)
        {
            CaseInvoiceOrder ret = null;
            var invoices = this.GetCaseInvoices(caseId).FirstOrDefault();
            if (invoices != null && invoices.Orders != null)
            {
                return invoices.Orders.Where(o => o.Id == invoiceOrderId).FirstOrDefault();
            }            
            return ret;
        }

        public void SaveCaseInvoices(IEnumerable<CaseInvoice> invoices, int caseId)
        {
            foreach (var invoice in invoices)
            {
                invoice.CaseId = caseId;
                CaseInvoiceEntity entity;
                if (invoice.IsNew())
                {
                    entity = new CaseInvoiceEntity();
                    this.invoiceToEntityMapper.Map(invoice, entity);
                    this.DbContext.CaseInvoices.Add(entity);
                }
                else
                {
                    entity = this.DbContext.CaseInvoices.Find(invoice.Id);
                    this.invoiceToEntityMapper.Map(invoice, entity);
                }
                this.Commit();

                foreach (var order in invoice.Orders)
                {
                    CaseInvoiceOrderEntity orderEntity;
                    if (order.Id > 0)
                    {
                        orderEntity = this.DbContext.CaseInvoiceOrders.Find(order.Id);
                        if (orderEntity.InvoiceDate == null)
                        {
                            var articlesForDelete = new List<int>();
                            articlesForDelete.AddRange(orderEntity.Articles.Where(a => order.Articles.All(ar => ar.Id != a.Id)).Select(a => a.Id));
                            foreach (var articleForDelete in articlesForDelete)
                            {
                                var a = this.DbContext.CaseInvoiceArticles.Find(articleForDelete);
                                this.DbContext.CaseInvoiceArticles.Remove(a);
                            }

                            this.orderMapper.Map(order, orderEntity);
                        }
                    }
                    else
                    {
                        orderEntity = new CaseInvoiceOrderEntity();
                        this.orderMapper.Map(order, orderEntity);
                        orderEntity.InvoiceId = entity.Id;
                        this.DbContext.CaseInvoiceOrders.Add(orderEntity);
                    }

                    this.Commit();

                    var orderFiles = this.DbContext.CaseInvoiceOrderFiles.Where(f => f.OrderId == orderEntity.Id);
                    foreach (var orderFile in orderFiles)
                    {
                        this.DbContext.CaseInvoiceOrderFiles.Remove(orderFile);
                    }

                    if (order.Files != null)
                    {
                        foreach (var file in order.Files)
                        {
                            var fileEntity = new CaseInvoiceOrderFileEntity();
                            this.filesMapper.Map(file, fileEntity);
                            fileEntity.OrderId = orderEntity.Id;
                            this.DbContext.CaseInvoiceOrderFiles.Add(fileEntity);
                        }
                    }

                    foreach (var article in order.Articles)
                    {
                        CaseInvoiceArticleEntity articleEntity;
                        if (article.Id > 0)
                        {
                            articleEntity = this.DbContext.CaseInvoiceArticles.Find(article.Id);
                            this.articleMapper.Map(article, articleEntity);         
                        }
                        else
                        {
                            articleEntity = new CaseInvoiceArticleEntity();
                            this.articleMapper.Map(article, articleEntity);
                            articleEntity.OrderId = orderEntity.Id;
                            this.DbContext.CaseInvoiceArticles.Add(articleEntity);
                        }
                    }

                    this.Commit();
                }
            }
        }        
    }
}