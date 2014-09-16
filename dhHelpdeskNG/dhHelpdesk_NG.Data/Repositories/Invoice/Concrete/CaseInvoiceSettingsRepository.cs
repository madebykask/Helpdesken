namespace DH.Helpdesk.Dal.Repositories.Invoice.Concrete
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Invoice;

    public class CaseInvoiceSettingsRepository : Repository, ICaseInvoiceSettingsRepository
    {
        private readonly IEntityToBusinessModelMapper<CaseInvoiceSettingsEntity, CaseInvoiceSettings> toBusinessModelMapper;

        private readonly IBusinessModelToEntityMapper<CaseInvoiceSettings, CaseInvoiceSettingsEntity> toEntityMapper;

        public CaseInvoiceSettingsRepository(
                IDatabaseFactory databaseFactory, 
                IEntityToBusinessModelMapper<CaseInvoiceSettingsEntity, CaseInvoiceSettings> toBusinessModelMapper, 
                IBusinessModelToEntityMapper<CaseInvoiceSettings, CaseInvoiceSettingsEntity> toEntityMapper)
            : base(databaseFactory)
        {
            this.toBusinessModelMapper = toBusinessModelMapper;
            this.toEntityMapper = toEntityMapper;
        }

        public CaseInvoiceSettings GetSettings(int customerId)
        {
            var entities = this.DbContext.CaseInvoiceSettings
                            .Where(s => s.CustomerId == customerId)
                            .ToList();
            return entities
                .Select(s => this.toBusinessModelMapper.Map(s))
                .FirstOrDefault();
        }

        public int SaveSettings(CaseInvoiceSettings settings)
        {
            CaseInvoiceSettingsEntity entity;
            if (settings.Id > 0)
            {
                entity = this.DbContext.CaseInvoiceSettings.Find(settings.Id);
                this.toEntityMapper.Map(settings, entity);
            }
            else
            {
                entity = new CaseInvoiceSettingsEntity();
                this.toEntityMapper.Map(settings, entity);
                this.DbContext.CaseInvoiceSettings.Add(entity);
            }

            this.Commit();
            return entity.Id;
        }
    }
}