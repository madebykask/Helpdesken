namespace DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceToEntityMapper : IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity>
    {
        public void Map(CaseInvoice businessModel, CaseInvoiceEntity entity)
        {
            entity.CaseId = businessModel.CaseId;
        }
    }
}