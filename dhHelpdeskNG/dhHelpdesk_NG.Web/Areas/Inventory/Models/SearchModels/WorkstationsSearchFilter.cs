
using DH.Helpdesk.Web.Enums.Inventory;

namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using DataAnnotationsExtensions;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public class WorkstationsSearchFilter
    {
        public WorkstationsSearchFilter()
        {
            this.ContractStartDate = new DateRange();
            this.ContractEndDate = new DateRange();
            this.ScanDate = new DateRange();
            this.ScrapDate = new DateRange();
        }

        private WorkstationsSearchFilter(int recordsOnPage)
            : this()
        {
            this.RecordsOnPage = recordsOnPage;

            this.SortField = new SortFieldModel();
        }

        [IsId]
        public int? RegionId { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        [IsId]
        public int? ComputerTypeId { get; set; }

        public ContractStatuses? ContractStatusId { get; set; }

        [NotNull]
        public DateRange ContractStartDate { get; set; }

        [NotNull]
        public DateRange ContractEndDate { get; set; }

        [NotNull]
        public DateRange ScanDate { get; set; }

        [NotNull]
        public DateRange ScrapDate { get; set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; set; }

        [Min(0)]
        [LocalizedDisplay("Poster per sida")]
        public int RecordsOnPage { get; set; }

        [LocalizedDisplay("Visa även skrotade datorer")]
        public bool IsShowScrapped { get; set; }

        public SortFieldModel SortField { get; set; }

        public int? RecordsCount { get; set; }

        public static WorkstationsSearchFilter CreateDefault()
        {
            return new WorkstationsSearchFilter(500);
        }

        public ComputersFilter CreateRequest(int customerId)
        {
            SortField sf = null;
            if (!string.IsNullOrEmpty(this.SortField.Name))
            {
                sf = new SortField(this.SortField.Name, this.SortField.SortBy ?? SortBy.Ascending);
            }
            return new ComputersFilter(
                customerId,
                this.RegionId,
                this.DepartmentId,
                this.ComputerTypeId,
                (int?)this.ContractStatusId,
                this.ContractStartDate.DateFrom,
                this.ContractStartDate.DateTo,
                this.ContractEndDate.DateFrom,
                this.ContractEndDate.DateTo,
                this.ScanDate.DateFrom,
                this.ScanDate.DateTo,
                this.ScrapDate.DateFrom,
                this.ScrapDate.DateTo,
                this.SearchFor,
                this.IsShowScrapped,
                this.RecordsOnPage,
                sf,
                this.RecordsCount);
        }

        public static string CreateFilterId()
        {
            return $"{TabName.Inventories}{InventoryFilterMode.Workstation}";
        }
    }
}