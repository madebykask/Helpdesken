namespace DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceToEntityMapper : IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity>
    {
        private readonly IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity> orderMapper;

        public CaseInvoiceToEntityMapper(
                IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity> orderMapper)
        {
            this.orderMapper = orderMapper;
        }

        public void Map(CaseInvoice businessModel, CaseInvoiceEntity entity)
        {
            entity.Id = businessModel.Id;
            entity.CaseId = businessModel.CaseId;
            var orders = new List<CaseInvoiceOrderEntity>();
            foreach (var order in businessModel.Orders)
            {
                var orderEntity = new CaseInvoiceOrderEntity();
                this.orderMapper.Map(order, orderEntity);
                orders.Add(orderEntity);
            }

            entity.Orders = orders;
        }
    }
}