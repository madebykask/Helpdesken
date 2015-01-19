namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class HistoryOverview
    {
        public HistoryOverview(
                int id, 
                DateTime dateAndTime, 
                UserName registeredBy,
                FullOrderHistoryFields order)
        {
            this.Order = order;
            this.RegisteredBy = registeredBy;
            this.DateAndTime = dateAndTime;
            this.Id = id;
        }

        [IsId]
        public int Id { get; private set; }

        public DateTime DateAndTime { get; private set; }

        public UserName RegisteredBy { get; private set; }

        [NotNull]
        public FullOrderHistoryFields Order { get; private set; }
    }
}