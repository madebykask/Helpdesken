namespace DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class InvoiceArticleToEntityMapper : IBusinessModelToEntityMapper<InvoiceArticle, InvoiceArticleEntity>
    {
        public void Map(InvoiceArticle businessModel, InvoiceArticleEntity entity)
        {
            entity.Name = businessModel.Name;
            entity.NameEng = businessModel.NameEng;
            entity.Description = businessModel.Description;
            entity.Number = businessModel.Number;
            entity.ParentId = businessModel.ParentId;
            entity.Ppu = businessModel.Ppu;
            //entity.ProductAreaId = businessModel.ProductAreaId;
            entity.UnitId = businessModel.UnitId;
            entity.CustomerId = businessModel.CustomerId;
        }
    }
}