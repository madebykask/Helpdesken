namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceToBusinessModelMapper : IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice>
    {
        private readonly IEntityToBusinessModelMapper<Case, CaseOverview> caseMapper;

        private readonly IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper;

        private readonly IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile> filesMapper;

        public CaseInvoiceToBusinessModelMapper(
            IEntityToBusinessModelMapper<Case, CaseOverview> caseMapper, 
            IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper, 
            IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile> filesMapper)
        {
            this.caseMapper = caseMapper;
            this.articleMapper = articleMapper;
            this.filesMapper = filesMapper;
        }

        public CaseInvoice Map(CaseInvoiceEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CaseInvoice(
                    entity.Id,
                    entity.CaseId,
                    this.caseMapper.Map(entity.Case),
                    entity.Orders
                        .Select(o => new CaseInvoiceOrder(
                                o.Id, 
                                o.InvoiceId, 
                                null, 
                                o.Number, 
                                o.DeliveryPeriod, 
                                o.Reference,
                                o.Date,
                                o.Articles.Select(a => new CaseInvoiceArticle(
                                                    a.Id,
                                                    a.OrderId,
                                                    null,
                                                    a.ArticleId,
                                                    this.articleMapper.Map(a.Article),
                                                    a.Name,
                                                    a.Amount,
                                                    a.Ppu,
                                                    a.Position,
                                                    a.IsInvoiced)).ToArray(),
                                                    o.Files.Select(f => this.filesMapper.Map(f)).OrderBy(f => f.FileName).ToArray()))
                                .OrderBy(o => o.Number).ToArray());
        }
    }
}