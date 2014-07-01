namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceArticleToBusinessModelMapper : IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle>
    {
        private readonly IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit> unitMapper;

        private readonly IEntityToBusinessModelMapper<Case, CaseOverview> caseMapper;

        public CaseInvoiceArticleToBusinessModelMapper(
            IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit> unitMapper, 
            IEntityToBusinessModelMapper<Case, CaseOverview> caseMapper)
        {
            this.unitMapper = unitMapper;
            this.caseMapper = caseMapper;
        }

        public CaseInvoiceArticle Map(CaseInvoiceArticleEntity entity)
        {
            return new CaseInvoiceArticle(
                        entity.Id,
                        entity.CaseId,
                        this.caseMapper.Map(entity.Case),
                        entity.Number,
                        entity.Name,
                        entity.Amount,
                        entity.UnitId,
                        this.unitMapper.Map(entity.Unit),
                        entity.Ppu,
                        entity.IsInvoiced);
        }
    }
}