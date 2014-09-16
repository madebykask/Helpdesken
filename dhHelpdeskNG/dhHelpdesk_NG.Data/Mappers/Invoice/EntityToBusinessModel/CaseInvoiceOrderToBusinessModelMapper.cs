namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceOrderToBusinessModelMapper : IEntityToBusinessModelMapper<CaseInvoiceOrderEntity, CaseInvoiceOrder>
    {
        private readonly IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice> caseInvoiceMapper;

        private readonly IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle> caseArticleMapper;

        public CaseInvoiceOrderToBusinessModelMapper(
            IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice> caseInvoiceMapper, 
            IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle> caseArticleMapper)
        {
            this.caseInvoiceMapper = caseInvoiceMapper;
            this.caseArticleMapper = caseArticleMapper;
        }

        public CaseInvoiceOrder Map(CaseInvoiceOrderEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CaseInvoiceOrder(
                        entity.Id,
                        entity.InvoiceId,
                        this.caseInvoiceMapper.Map(entity.Invoice),
                        entity.Number,
                        entity.DeliveryPeriod,
                        entity.Reference,
                        entity.Date,
                        entity.Articles.Select(a => this.caseArticleMapper.Map(a)).OrderBy(a => a.Position).ToArray());
        }
    }
}