namespace DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceOrderToEntityMapper : IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity>
    {
        private readonly IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity> articleMapper;

        public CaseInvoiceOrderToEntityMapper(
            IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity> articleMapper)
        {
            this.articleMapper = articleMapper;
        }

        public void Map(CaseInvoiceOrder businessModel, CaseInvoiceOrderEntity entity)
        {
            entity.Id = businessModel.Id;
            entity.InvoiceId = businessModel.InvoiceId;
            entity.Number = businessModel.Number;
            entity.DeliveryPeriod = businessModel.DeliveryPeriod;
            entity.Reference = businessModel.Reference;
            var articles = new List<CaseInvoiceArticleEntity>();
            foreach (var article in businessModel.Articles)
            {
                var articleEntity = new CaseInvoiceArticleEntity();
                this.articleMapper.Map(article, articleEntity);
                articles.Add(articleEntity);
            }

            entity.Articles = articles;
        }
    }
}