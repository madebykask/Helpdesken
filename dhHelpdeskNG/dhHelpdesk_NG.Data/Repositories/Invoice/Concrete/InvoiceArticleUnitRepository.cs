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

        public InvoiceArticleUnitRepository(
            IDatabaseFactory databaseFactory, 
            IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit> unitMapper)
            : base(databaseFactory)
        {
            this.unitMapper = unitMapper;
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
    }
}