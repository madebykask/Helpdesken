namespace DH.Helpdesk.Dal.Repositories.Invoice.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Invoice;
    using DH.Helpdesk.Common.Enums;
    using System;

    public class CaseInvoiceArticleRepository : Repository, ICaseInvoiceArticleRepository
    {
        private readonly IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice> invoiceToBusinessModelMapper;

        private readonly IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity> invoiceToEntityMapper;

        private readonly IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity> orderMapper;
        
        private readonly IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity> articleMapper;

        private readonly IBusinessModelToEntityMapper<CaseInvoiceOrderFile, CaseInvoiceOrderFileEntity> filesMapper;
		private readonly ICaseRepository _caseRepository;

		public CaseInvoiceArticleRepository(
                IDatabaseFactory databaseFactory, 
                IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice> invoiceToBusinessModelMapper, 
                IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity> invoiceToEntityMapper, 
                IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity> orderMapper,                 
                IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity> articleMapper, 
                IBusinessModelToEntityMapper<CaseInvoiceOrderFile, CaseInvoiceOrderFileEntity> filesMapper,
				ICaseRepository caseRepository)
            : base(databaseFactory)
        {
            this.invoiceToBusinessModelMapper = invoiceToBusinessModelMapper;
            this.invoiceToEntityMapper = invoiceToEntityMapper;
            this.orderMapper = orderMapper;            
            this.articleMapper = articleMapper;
            this.filesMapper = filesMapper;
			_caseRepository = caseRepository;

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

        public CaseInvoiceOrder[] GetOrders(int caseId, InvoiceOrderFetchStatus orderStatus)
        {
            var res = new List<CaseInvoiceOrder>();
            var invoiceEntities = this.DbContext.CaseInvoices
                                             .Where(i => i.CaseId == caseId)
                                             .ToList();

            var orderEntities = new List<CaseInvoiceOrderEntity>();
            foreach (var invoiceEntity in invoiceEntities)
            {
                var invoiceModel = invoiceToBusinessModelMapper.Map(invoiceEntity);
                var orderModels = new List<CaseInvoiceOrder>();

                switch (orderStatus)
                {
                    case InvoiceOrderFetchStatus.All:
                        orderModels = invoiceModel.Orders.ToList();
                        break;

                    case InvoiceOrderFetchStatus.AllNotSent:
                        orderModels = invoiceModel.Orders
                                                  .Where(o => o.OrderState == (int) InvoiceOrderStates.Saved)
                                                  .ToList();
                        break;

                    case InvoiceOrderFetchStatus.AllSent:
                        orderModels = invoiceModel.Orders
                                                  .Where(o => o.OrderState == (int) InvoiceOrderStates.Sent)
                                                  .ToList();
                        break;

                    case InvoiceOrderFetchStatus.Orders:
                        orderModels = invoiceModel.Orders
                                                  .Where(o => !o.CreditForOrder_Id.HasValue)
                                                  .ToList();
                        break;

                    case InvoiceOrderFetchStatus.OrderNotSent:
                        orderModels = invoiceModel.Orders
                                                  .Where(o => !o.CreditForOrder_Id.HasValue && o.OrderState == (int)InvoiceOrderStates.Saved)
                                                  .ToList();
                        break;

                    case InvoiceOrderFetchStatus.OrderSent:
                        orderModels = invoiceModel.Orders
                                                  .Where(o => !o.CreditForOrder_Id.HasValue && o.OrderState == (int)InvoiceOrderStates.Sent)
                                                  .ToList();
                        break;

                    case InvoiceOrderFetchStatus.Credits:
                        orderModels = invoiceModel.Orders
                                                  .Where(o => o.CreditForOrder_Id.HasValue)
                                                  .ToList();
                        break;

                    case InvoiceOrderFetchStatus.CreditNotSent:
                        orderModels = invoiceModel.Orders
                                                  .Where(o => o.CreditForOrder_Id.HasValue && o.OrderState == (int)InvoiceOrderStates.Saved)
                                                  .ToList();
                        break;

                    case InvoiceOrderFetchStatus.CreditSent:
                        orderModels = invoiceModel.Orders
                                                  .Where(o => o.CreditForOrder_Id.HasValue && o.OrderState == (int)InvoiceOrderStates.Sent)
                                                  .ToList();
                        break;

                    default:
                        return res.ToArray();
                }

                res.AddRange(orderModels);                
            }

            return res.ToArray();
        }

        public void CancelInvoiced(int caseId, int invoiceOrderId)
        {                       
            var orderEntity = this.DbContext.CaseInvoiceOrders.Find(invoiceOrderId);
            if (orderEntity != null)
            {
                orderEntity.InvoiceDate = null;
                orderEntity.InvoicedByUserId = null;
                orderEntity.OrderState = (int)InvoiceOrderStates.Saved;   
                this.Commit();
            }                                          
        }

        public int SaveCaseInvoices(IEnumerable<CaseInvoice> invoices, int caseId, int userId)
        {
            var newOrderId = 0;

			var loadIntoContext = _caseRepository.GetCaseById(caseId);

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
            
                var hasInfoToSave = true;
                var isNewOrder = false;
                var now = DateTime.UtcNow;
                foreach (var order in invoice.Orders)
                {
                    hasInfoToSave = true;
                    isNewOrder = false;

                    var orderEntity = new CaseInvoiceOrderEntity();
                    if (order.Id > 0)
                    {
                        if (order.OrderState != (int)InvoiceOrderStates.Deleted)
                        {
                            orderEntity = this.DbContext.CaseInvoiceOrders.Find(order.Id);
                            if (orderEntity.OrderState == (int)InvoiceOrderStates.Saved)
                            {
                                var articlesForDelete = new List<int>();
                                articlesForDelete.AddRange(orderEntity.Articles.Where(a => order.Articles.All(ar => ar.Id != a.Id)).Select(a => a.Id));
                                foreach (var articleForDelete in articlesForDelete)
                                {
                                    var a = this.DbContext.CaseInvoiceArticles.Find(articleForDelete);
                                    this.DbContext.CaseInvoiceArticles.Remove(a);
                                }

                                if (!order.Articles.Any())
                                    order.OrderState = (int)InvoiceOrderStates.Deleted;

                                this.orderMapper.Map(order, orderEntity);
                                orderEntity.ChangedByUser_Id = userId;
                                orderEntity.ChangedTime = now;
                            }
                        }
                        else
                        {
                            hasInfoToSave = false;
                        }
                    }
                    else
                    {
                        if (order.Articles.Any())
                        {
                            if (order.OrderState != (int)InvoiceOrderStates.Sent)
                                order.OrderState = (int)InvoiceOrderStates.Saved;
                            this.orderMapper.Map(order, orderEntity);
                            orderEntity.InvoiceId = entity.Id;
                            orderEntity.CreatedByUser_Id = userId;
                            orderEntity.CreatedTime = now;
                            orderEntity.ChangedByUser_Id = userId;
                            orderEntity.ChangedTime = now;
                            this.DbContext.CaseInvoiceOrders.Add(orderEntity);                            
                            isNewOrder = true;
                        }
                        else 
                        {
                            hasInfoToSave = false;                            
                        }
                    }
                    this.Commit();

                    if (isNewOrder)
                        newOrderId = orderEntity.Id;
                    
                    if (hasInfoToSave)
                    {
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

                        this.Commit();

                        if (order.Articles.Any())
                        {
                            /* First save articles/texts have saved before */ 
                            foreach (var article in order.Articles.Where(a=> (a.Id > 0 && !a.TextForArticle_Id.HasValue) || 
                                                                             (a.Id > 0 && a.TextForArticle_Id.HasValue && a.TextForArticle_Id.Value > 0) ||
                                                                             (a.Id < 0 && a.TextForArticle_Id.HasValue && a.TextForArticle_Id.Value > 0)).ToList())
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
                                    articleEntity.Id = 0;
                                    articleEntity.OrderId = orderEntity.Id;
                                    this.DbContext.CaseInvoiceArticles.Add(articleEntity);
                                }                                                          
                            }
                            this.Commit();

                            /* Save articles/texts have not saved yet */
                            foreach (var article in order.Articles.Where(a => a.Id < 0 && !a.TextForArticle_Id.HasValue).ToList())
                            {
                                var tempId = article.Id;
                                var articleEntity = new CaseInvoiceArticleEntity();
                                this.articleMapper.Map(article, articleEntity);
                                articleEntity.OrderId = orderEntity.Id;
                                this.DbContext.CaseInvoiceArticles.Add(articleEntity);                                
                                this.Commit();
                                var newMainArticleId = articleEntity.Id;
                                foreach (var textArticle in order.Articles.Where(a => a.TextForArticle_Id.HasValue && a.TextForArticle_Id.Value == tempId).ToList())
                                {
                                    textArticle.TextForArticle_Id = newMainArticleId;
                                    var _articleEntity = new CaseInvoiceArticleEntity();
                                    this.articleMapper.Map(textArticle, _articleEntity);
                                    _articleEntity.OrderId = orderEntity.Id;
                                    this.DbContext.CaseInvoiceArticles.Add(_articleEntity);
                                    this.Commit();
                                }
                            }
                            
                        }
                    }
                }
            }

            return newOrderId;
        }        
    }
}