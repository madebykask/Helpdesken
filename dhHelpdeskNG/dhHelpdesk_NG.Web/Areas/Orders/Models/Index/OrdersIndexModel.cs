namespace DH.Helpdesk.Web.Areas.Orders.Models.Index
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdersIndexModel : BaseIndexModel
    {
        public OrdersIndexModel(
                MultiSelectList orderTypes, 
                MultiSelectList administrators, 
                MultiSelectList statuses)
        {
            this.Statuses = statuses;
            this.Administrators = administrators;
            this.OrderTypes = orderTypes;
        }

        public OrdersIndexModel()
        {
            this.OrderTypeIds = new int[0];
            this.AdministratorIds = new int[0];
            this.StatusIds = new int[0];
        }

        public override IndexModelType Type
        {
            get
            {
                return IndexModelType.Orders;
            }
        }

        [NotNull]
        public MultiSelectList OrderTypes { get; private set; }

        [NotNull]
        public int[] OrderTypeIds { get; set; }

        [NotNull]
        public MultiSelectList Administrators { get; private set; }

        [NotNull]
        public int[] AdministratorIds { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [NotNull]
        public MultiSelectList Statuses { get; private set; }

        public int[] StatusIds { get; set; }

        public string Text { get; set; }

        public OrdersFilterModel GetFilter()
        {
            return new OrdersFilterModel(
                                    this.OrderTypeIds,
                                    this.AdministratorIds,
                                    this.StartDate,
                                    this.EndDate,
                                    this.StatusIds,
                                    this.Text);
        }
    }
}