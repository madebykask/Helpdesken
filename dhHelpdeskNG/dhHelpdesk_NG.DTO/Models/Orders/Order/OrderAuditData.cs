namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderAuditData
    {
        public OrderAuditData(int historyId, FullOrderEditFields existingOrder)
        {
            this.ExistingOrder = existingOrder;
            this.HistoryId = historyId;
        }

        [IsId]
        public int HistoryId { get; private set; }

        public FullOrderEditFields ExistingOrder { get; private set; }
    }
}