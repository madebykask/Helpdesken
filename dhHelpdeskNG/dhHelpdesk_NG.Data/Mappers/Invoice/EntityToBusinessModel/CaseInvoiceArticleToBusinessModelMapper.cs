namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceArticleToBusinessModelMapper : IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle>
    {
        private readonly IEntityToBusinessModelMapper<CaseInvoiceOrderEntity, CaseInvoiceOrder> invoiceOrderMapper;

        private readonly IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper;

        public CaseInvoiceArticleToBusinessModelMapper(
            IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper, 
            IEntityToBusinessModelMapper<CaseInvoiceOrderEntity, CaseInvoiceOrder> invoiceOrderMapper)
        {
            this.articleMapper = articleMapper;
            this.invoiceOrderMapper = invoiceOrderMapper;
        }

        public CaseInvoiceArticle Map(CaseInvoiceArticleEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CaseInvoiceArticle(
                        entity.Id,
                        entity.OrderId,
                        this.invoiceOrderMapper.Map(entity.Order),
                        entity.ArticleId,
                        this.articleMapper.Map(entity.Article),
                        entity.Name,
                        entity.Amount,
                        entity.Ppu,
                        entity.Position,
                        entity.CreditedForArticle_Id);
        }
    }
}