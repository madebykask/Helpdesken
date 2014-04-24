namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class PrinterSearchFilter
    {
        public PrinterSearchFilter()
        {
        }

        public PrinterSearchFilter(int customerId, int? departmentId, string searchFor)
        {
            this.CustomerId = customerId;
            this.DepartmentId = departmentId;
            this.SearchFor = searchFor;
        }

        [IsId]
        public int CustomerId { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        public static PrinterSearchFilter CreateDefault()
        {
            return new PrinterSearchFilter();
        }

        public PrintersFilter CreateRequest(int customerId)
        {
            return new PrintersFilter(customerId, this.DepartmentId, this.SearchFor);
        }
    }
}