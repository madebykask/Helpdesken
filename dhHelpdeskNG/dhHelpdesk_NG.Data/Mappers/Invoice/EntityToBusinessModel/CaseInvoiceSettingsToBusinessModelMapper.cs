namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceSettingsToBusinessModelMapper : IEntityToBusinessModelMapper<CaseInvoiceSettingsEntity, CaseInvoiceSettings>
    {
        public CaseInvoiceSettings Map(CaseInvoiceSettingsEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CaseInvoiceSettings(
                            entity.Id,
                            entity.CustomerId,
                            entity.ExportPath,
                            entity.Currency,
                            entity.OrderNoPrefix,
                            entity.Issuer,
                            entity.OurReference,
                            entity.DocTemplate);
        }
    }
}