namespace DH.Helpdesk.Web.Areas.Orders.Models.Index
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdersFilterModel
    {
        public OrdersFilterModel(
                int[] orderTypeIds, 
                int[] administratiorIds, 
                DateTime? startDate, 
                DateTime? endDate, 
                int[] statusIds, 
                string text)
        {
            this.Text = text;
            this.StatusIds = statusIds;
            this.EndDate = endDate;
            this.StartDate = startDate;
            this.AdministratiorIds = administratiorIds;
            this.OrderTypeIds = orderTypeIds;
        }

        private OrdersFilterModel()
        {
            this.OrderTypeIds = new int[0];
            this.AdministratiorIds = new int[0];
            this.StatusIds = new int[0];
        }

        [NotNull]
        public int[] OrderTypeIds { get; private set; }

        [NotNull]
        public int[] AdministratiorIds { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        [NotNull]
        public int[] StatusIds { get; private set; }

        public string Text { get; private set; }

        public static OrdersFilterModel CreateDefault()
        {
            return new OrdersFilterModel();
        }
    }
}