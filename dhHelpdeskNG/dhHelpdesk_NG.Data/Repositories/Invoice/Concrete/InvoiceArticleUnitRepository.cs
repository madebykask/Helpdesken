namespace DH.Helpdesk.Dal.Repositories.Invoice.Concrete
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class InvoiceArticleUnitRepository : Repository, IInvoiceArticleUnitRepository
    {
        private readonly IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit> unitMapper;

        private readonly IBusinessModelToEntityMapper<InvoiceArticleUnit, InvoiceArticleUnitEntity> toEntityMapper;

        public InvoiceArticleUnitRepository(
            IDatabaseFactory databaseFactory, 
            IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit> unitMapper, 
            IBusinessModelToEntityMapper<InvoiceArticleUnit, InvoiceArticleUnitEntity> toEntityMapper)
            : base(databaseFactory)
        {
            this.unitMapper = unitMapper;
            this.toEntityMapper = toEntityMapper;
        }

        public InvoiceArticleUnit[] GetUnits(int customerId)
        {
            var entities = this.DbContext.InvoiceArticleUnits
                                .Where(u => u.CustomerId == customerId)
                                .ToList();

            return entities
                    .Select(u => this.unitMapper.Map(u))
                    .ToArray();
        }

        public void SaveUnit(InvoiceArticleUnit unit)
        {
            InvoiceArticleUnitEntity entity;
            if (unit.Id > 0)
            {
                entity = this.DbContext.InvoiceArticleUnits.Find(unit.Id);
                this.toEntityMapper.Map(unit, entity);
            }
            else
            {
                entity = new InvoiceArticleUnitEntity();
                this.toEntityMapper.Map(unit, entity);
                this.DbContext.InvoiceArticleUnits.Add(entity);
            }

            this.Commit();            
        }
    }
}