namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DataAnnotationsExtensions;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.LocalizedAttributes;

    public class InventorySearchFilter
    {
        public InventorySearchFilter(int customerId, int? departmentId, string searchFor, int recordsOnPage)
        {
            this.CustomerId = customerId;
            this.DepartmentId = departmentId;
            this.SearchFor = searchFor;
            this.RecordsOnPage = recordsOnPage;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; private set; }

        [Min(0)]
        [LocalizedDisplay("Poster per sida")]
        public int RecordsOnPage { get; private set; }
    }
}