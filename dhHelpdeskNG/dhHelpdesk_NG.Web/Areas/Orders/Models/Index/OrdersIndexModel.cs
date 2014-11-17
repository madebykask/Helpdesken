namespace DH.Helpdesk.Web.Areas.Orders.Models.Index
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdersIndexModel : BaseIndexModel
    {
        public OrdersIndexModel(
                SelectList orderTypes, 
                MultiSelectList administrators, 
                MultiSelectList statuses)
        {
            this.Statuses = statuses;
            this.Administrators = administrators;
            this.OrderTypes = orderTypes;
        }

        public OrdersIndexModel()
        {
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
        public SelectList OrderTypes { get; private set; }

        public int? OrderTypeId { get; set; }

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
                                    this.OrderTypeId,
                                    this.AdministratorIds,
                                    this.StartDate,
                                    this.EndDate,
                                    this.StatusIds,
                                    this.Text);
        }
    }
}