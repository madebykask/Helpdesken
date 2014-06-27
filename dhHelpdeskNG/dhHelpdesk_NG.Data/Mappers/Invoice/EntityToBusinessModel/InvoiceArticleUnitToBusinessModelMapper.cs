namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class InvoiceArticleUnitToBusinessModelMapper : IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit>
    {
        public InvoiceArticleUnit Map(InvoiceArticleUnitEntity entity)
        {
            return new InvoiceArticleUnit(
                            entity.Id,
                            entity.Name);
        }
    }
}