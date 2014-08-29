namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceToBusinessModelMapper : IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice>
    {
        private readonly IEntityToBusinessModelMapper<Case, CaseOverview> caseMapper;

        public CaseInvoiceToBusinessModelMapper(
            IEntityToBusinessModelMapper<Case, CaseOverview> caseMapper)
        {
            this.caseMapper = caseMapper;
        }

        public CaseInvoice Map(CaseInvoiceEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CaseInvoice(
                    entity.Id,
                    entity.CaseId,
                    this.caseMapper.Map(entity.Case),
                    entity.Orders.Select(o => new CaseInvoiceOrder(o.Id, o.InvoiceId, null, o.Number, o.DeliveryPeriod, null)).OrderBy(o => o.Number).ToArray());
        }
    }
}