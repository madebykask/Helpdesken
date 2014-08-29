namespace DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceArticleToEntityMapper : IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity>
    {
        public void Map(CaseInvoiceArticle businessModel, CaseInvoiceArticleEntity entity)
        {
            entity.OrderId = businessModel.OrderId;
            entity.IsInvoiced = businessModel.IsInvoiced;
            entity.ArticleId = businessModel.ArticleId;
            entity.Name = businessModel.Name;
            entity.Amount = businessModel.Amount;
            entity.Ppu = businessModel.Ppu;
            entity.Position = businessModel.Position;
        }
    }
}