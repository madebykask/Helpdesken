namespace DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceSettingsToEntityMapper : IBusinessModelToEntityMapper<CaseInvoiceSettings, CaseInvoiceSettingsEntity>
    {
        public void Map(CaseInvoiceSettings businessModel, CaseInvoiceSettingsEntity entity)
        {
            entity.CustomerId = businessModel.CustomerId;
            entity.ExportPath = businessModel.ExportPath;
            entity.Currency = businessModel.Currency;
            entity.OrderNoPrefix = businessModel.OrderNoPrefix;
            entity.Issuer = businessModel.Issuer;
            entity.OurReference = businessModel.OurReference;
            entity.DocTemplate = businessModel.DocTemplate;
        }
    }
}