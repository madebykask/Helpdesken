namespace DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceOrderFileToEntityMapper : IBusinessModelToEntityMapper<CaseInvoiceOrderFile, CaseInvoiceOrderFileEntity>
    {
        public void Map(CaseInvoiceOrderFile businessModel, CaseInvoiceOrderFileEntity entity)
        {
            entity.OrderId = businessModel.OrderId;
            entity.FileName = businessModel.FileName;
            entity.CreatedDate = businessModel.CreatedDate;
        }
    }
}