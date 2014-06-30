namespace DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceArticleToEntityMapper : IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity>
    {
        public void Map(CaseInvoiceArticle businessModel, CaseInvoiceArticleEntity entity)
        {
            entity.CaseId = businessModel.CaseId;
            entity.IsInvoiced = businessModel.IsInvoiced;
            entity.Name = businessModel.Name;
            entity.Ppu = businessModel.Ppu;
            entity.UnitId = businessModel.UnitId;
        }
    }
}