using System;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.SelfService.Infrastructure.LocalizedAttributes;
using DH.Helpdesk.SelfService.Models.Common;

namespace DH.Helpdesk.SelfService.Models.Orders
{
    public class OrdersIndexModel
    {
        public OrdersIndexModel(
        SelectList orderTypesSearch,
        SelectList orderTypes,
        MultiSelectList administrators,
        DateTime? startDate,
        DateTime? endDate,
        MultiSelectList statuses,
        string text,
        int recordsOnPage,
        SortFieldModel sortField,
        SelectList orderTypesForCreateOrder)
        {
            OrderTypesForCreateOrder = orderTypesForCreateOrder;
            Statuses = statuses;
            Administrators = administrators;
            OrderTypes = orderTypes;
            StartDate = startDate;
            EndDate = endDate;
            Text = text;
            RecordsOnPage = recordsOnPage;
            SortField = sortField;
            OrderTypesSearch = orderTypesSearch;
        }

        public OrdersIndexModel()
        {
            AdministratorIds = new int[0];
            StatusIds = new int[0];
        }

        [NotNull]
        public SelectList OrderTypesForCreateOrder { get; private set; }

        [LocalizedDisplay("Typ")]
        public int? OrderTypeForCreateOrderId { get; set; }

        [NotNull]
        public SelectList OrderTypes { get; private set; }

        [NotNull]
        public SelectList OrderTypesSearch { get; private set; }

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