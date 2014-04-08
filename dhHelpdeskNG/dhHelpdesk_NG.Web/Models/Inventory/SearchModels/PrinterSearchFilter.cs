namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class PrinterSearchFilter
    {
        public PrinterSearchFilter(int customerId, int? departmentId, string searchFor)
        {
            this.CustomerId = customerId;
            this.DepartmentId = departmentId;
            this.SearchFor = searchFor;
        }

        private PrinterSearchFilter(int customerId)
        {
            this.CustomerId = customerId;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; private set; }

        public static PrinterSearchFilter CreateDefault(int customerId)
        {
            return new PrinterSearchFilter(customerId);
        }

        public PrintersFilter CreateRequest()
        {
            return new PrintersFilter(this.CustomerId, this.DepartmentId, this.SearchFor);
        }
    }
}