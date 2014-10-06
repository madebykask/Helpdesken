﻿namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public class PrinterSearchFilter
    {
        [IsId]
        public int CustomerId { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        public SortFieldModel SortField { get; set; }

        public static PrinterSearchFilter CreateDefault()
        {
            return new PrinterSearchFilter { SortField = new SortFieldModel() };
        }

        public PrintersFilter CreateRequest(int customerId)
        {
            return new PrintersFilter(customerId, this.DepartmentId, this.SearchFor);
        }
    }
}