using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.SelfService.Models.Orders
{
    public class OrdersFilterModel
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
            SortField = sortField;
            RecordsOnPage = recordsOnPage;
            Text = text;
            StatusIds = statusIds;
            EndDate = endDate;
            StartDate = startDate;
            AdministratiorIds = administratiorIds;
            OrderTypeId = orderTypeId;
        }

        private OrdersFilterModel()
        {
            AdministratiorIds = new int[0];
            StatusIds = new int[0];
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
            return new OrdersFilterModel
            {
                RecordsOnPage = 100,
                SortField =
                    new SortField(BusinessData.Enums.Orders.FieldNames.GeneralFieldNames.OrderDate, SortBy.Descending)
            };
        }
    }
}