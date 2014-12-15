namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdateOrderRequest
    {
        public UpdateOrderRequest(
                FullOrderEditFields order, 
                int customerId, 
                DateTime dateAndTime, 
                List<int> deletedLogIds, 
                List<ManualLog> newLogs)
        {
            this.NewLogs = newLogs;
            this.DeletedLogIds = deletedLogIds;
            this.DateAndTime = dateAndTime;
            this.CustomerId = customerId;
            this.Order = order;
        }

        [NotNull]
        public FullOrderEditFields Order { get; private set; }

        [IsId]
        public int CustomerId { get; private set; }

        public DateTime DateAndTime { get; private set; }

        [NotNull]
        public List<int> DeletedLogIds { get; private set; }

        [NotNull]
        public List<ManualLog> NewLogs { get; private set; }
    }
}