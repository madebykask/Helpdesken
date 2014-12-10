namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdateOrderRequest
    {
        public UpdateOrderRequest(
                FullOrderEditFields order, 
                int customerId, 
                DateTime dateAndTime)
        {
            this.DateAndTime = dateAndTime;
            this.CustomerId = customerId;
            this.Order = order;
        }

        [NotNull]
        public FullOrderEditFields Order { get; private set; }

        [IsId]
        public int CustomerId { get; private set; }

        public DateTime DateAndTime { get; private set; }
    }
}