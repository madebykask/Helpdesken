namespace DH.Helpdesk.Web.Areas.Orders.Models.Index
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class OrdersIndexModel : BaseIndexModel
    {
        public OrdersIndexModel(
                SelectList orderTypes, 
                MultiSelectList administrators,
                DateTime? startDate,
                DateTime? endDate,                
                MultiSelectList statuses,
                string text,
                int recordsOnPage,
                SortFieldModel sortField)
        {
            this.Statuses = statuses;
            this.Administrators = administrators;
            this.OrderTypes = orderTypes;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Text = text;
            this.RecordsOnPage = recordsOnPage;
            this.SortField = sortField;
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

        [LocalizedDisplay("Typ")]
        public int? OrderTypeId { get; set; }

        [NotNull]
        public MultiSelectList Administrators { get; private set; }

        [NotNull]
        [LocalizedDisplay("Handläggare")]
        public int[] AdministratorIds { get; set; }

        [LocalizedDisplay("Beställningsdatum")]
        public DateTime? StartDate { get; set; }

        [LocalizedDisplay("Beställningsdatum")]
        public DateTime? EndDate { get; set; }

        [NotNull]
        public MultiSelectList Statuses { get; private set; }

        [LocalizedDisplay("Status")]
        public int[] StatusIds { get; set; }

        [LocalizedDisplay("Sök")]
        public string Text { get; set; }

        [LocalizedDisplay("poster per sida")]
        [LocalizedInteger]
        [LocalizedMin(0)]
        public int RecordsOnPage { get; set; }

        public SortFieldModel SortField { get; set; }

        public OrdersFilterModel GetFilter()
        {
            SortField sortField = null;

            if (!string.IsNullOrEmpty(this.SortField.Name) && this.SortField.SortBy != null)
            {
                sortField = new SortField(this.SortField.Name, this.SortField.SortBy.Value);
            }

            return new OrdersFilterModel(
                                    this.OrderTypeId,
                                    this.AdministratorIds,
                                    this.StartDate,
                                    this.EndDate,
                                    this.StatusIds,
                                    this.Text,
                                    this.RecordsOnPage,
                                    sortField);
        }
    }
}