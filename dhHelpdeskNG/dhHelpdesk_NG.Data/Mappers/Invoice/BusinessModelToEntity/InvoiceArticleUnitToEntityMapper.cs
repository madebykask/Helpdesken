namespace DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class InvoiceArticleUnitToEntityMapper : IBusinessModelToEntityMapper<InvoiceArticleUnit, InvoiceArticleUnitEntity>
    {
        public void Map(InvoiceArticleUnit businessModel, InvoiceArticleUnitEntity entity)
        {
            entity.Name = businessModel.Name;
            entity.CustomerId = businessModel.CustomerId;
        }
    }
}