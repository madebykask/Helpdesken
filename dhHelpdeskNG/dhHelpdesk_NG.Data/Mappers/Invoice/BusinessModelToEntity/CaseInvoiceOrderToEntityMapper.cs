namespace DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceOrderToEntityMapper : IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity>
    {
        public void Map(CaseInvoiceOrder businessModel, CaseInvoiceOrderEntity entity)
        {
            entity.InvoiceId = businessModel.InvoiceId;
            entity.Number = businessModel.Number;
            entity.DeliveryPeriod = businessModel.DeliveryPeriod;
            entity.Reference = businessModel.Reference;
            entity.Date = businessModel.Date;
        }
    }
}