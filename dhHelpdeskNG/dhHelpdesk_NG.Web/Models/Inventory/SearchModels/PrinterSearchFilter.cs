namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.LocalizedAttributes;

    public class PrinterSearchFilter
    {
        public PrinterSearchFilter(int customerId, int? departmentId, string searchFor)
        {
            this.CustomerId = customerId;
            this.DepartmentId = departmentId;
            this.SearchFor = searchFor;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; private set; }
    }
}