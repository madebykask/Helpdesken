namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceOrderFileToBusinessModelMapper : IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile>
    {
        public CaseInvoiceOrderFile Map(CaseInvoiceOrderFileEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CaseInvoiceOrderFile(
                        entity.Id,
                        entity.OrderId,
                        entity.FileName,
                        entity.CreatedDate);
        }
    }
}