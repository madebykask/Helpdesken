namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderHistoryModel : BusinessModel
    {
        private OrderHistoryModel()
        {            
        }

        [NotNull]
        public FullOrderEditFields Order { get; private set; }

        public DateTime CreatedDateAndTime { get; private set; }

        [IsId]
        public int CreatedByUserId { get; private set; }

        public static OrderHistoryModel CreateNew(
                    FullOrderEditFields order,
                    DateTime createdDateAndTime,
                    int createdByUserId)
        {
            return new OrderHistoryModel
            {
                CreatedByUserId = createdByUserId,
                CreatedDateAndTime = createdDateAndTime,
                Order = order
            };
        }
    }
}