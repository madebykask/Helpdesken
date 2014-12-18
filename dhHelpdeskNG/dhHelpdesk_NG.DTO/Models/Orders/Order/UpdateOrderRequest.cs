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
                List<ManualLog> newLogs, 
                int userId, 
                bool informOrderer, 
                bool informReceiver, 
                bool createCase, 
                int languageId)
        {
            this.LanguageId = languageId;
            this.CreateCase = createCase;
            this.InformReceiver = informReceiver;
            this.InformOrderer = informOrderer;
            this.UserId = userId;
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

        [IsId]
        public int UserId { get; private set; }

        public DateTime DateAndTime { get; private set; }

        [NotNull]
        public List<int> DeletedLogIds { get; private set; }

        [NotNull]
        public List<ManualLog> NewLogs { get; private set; }

        public bool InformOrderer { get; private set; }

        public bool InformReceiver { get; private set; }

        public bool CreateCase { get; private set; }

        [IsId]
        public int LanguageId { get; private set; }
    }
}