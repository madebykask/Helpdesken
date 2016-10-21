using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Web.Areas.Orders.Models.Index
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdersFilterModel
    {
        public OrdersFilterModel(
                int? orderTypeId, 
                int[] administratiorIds, 
                DateTime? startDate, 
                DateTime? endDate, 
                int[] statusIds, 
                string text, 
                int recordsOnPage, 
                SortField sortField)
        {
            this.SortField = sortField;
            this.RecordsOnPage = recordsOnPage;
            this.Text = text;
            this.StatusIds = statusIds;
            this.EndDate = endDate;
            this.StartDate = startDate;
            this.AdministratiorIds = administratiorIds;
            this.OrderTypeId = orderTypeId;
        }

        private OrdersFilterModel()
        {
            this.AdministratiorIds = new int[0];
            this.StatusIds = new int[0];
        }

        [IsId]
        public int? OrderTypeId { get; private set; }

        [NotNull]
        public int[] AdministratiorIds { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        [NotNull]
        public int[] StatusIds { get; private set; }

        public string Text { get; private set; }

        [MinValue(0)]
        public int RecordsOnPage { get; private set; }

        public SortField SortField { get; private set; }

        public static OrdersFilterModel CreateDefault()
        {
            return new OrdersFilterModel { RecordsOnPage = 100, SortField = new SortField(BusinessData.Enums.Orders.FieldNames.GeneralFieldNames.OrderDate, SortBy.Descending) };
        }
    }
}