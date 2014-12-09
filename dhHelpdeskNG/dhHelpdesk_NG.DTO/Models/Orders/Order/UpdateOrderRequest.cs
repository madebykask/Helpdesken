namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdateOrderRequest
    {
        [NotNull]
        public FullOrderEditFields Order { get; private set; }
    }
}