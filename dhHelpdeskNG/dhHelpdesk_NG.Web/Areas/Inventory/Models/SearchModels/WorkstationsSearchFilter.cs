
using DH.Helpdesk.Web.Enums.Inventory;

namespace DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels
{
    using DataAnnotationsExtensions;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Requests.Inventory;
    using Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public class WorkstationsSearchFilter
    {
        public WorkstationsSearchFilter()
        {
            ContractStartDate = new DateRange();
            ContractEndDate = new DateRange();
            ScanDate = new DateRange();
            ScrapDate = new DateRange();
        }

        private WorkstationsSearchFilter(int recordsOnPage)
            : this()
        {
            RecordsOnPage = recordsOnPage;

            SortField = new SortFieldModel();
        }

        
        [IsId]
        public int? DomainId { get; set; }

        [IsId]
        public int? RegionId { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        [IsId]
        public int? ComputerTypeId { get; set; }

        public int? ContractStatusId { get; set; }

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
            if (!string.IsNullOrEmpty(SortField.Name))
            {
                sf = new SortField(SortField.Name, SortField.SortBy ?? SortBy.Ascending);
            }
            return new ComputersFilter(
                customerId,
                DomainId,
                RegionId,
                DepartmentId,
                ComputerTypeId,
                (int?)ContractStatusId,
                ContractStartDate.DateFrom,
                ContractStartDate.DateTo,
                ContractEndDate.DateFrom,
                ContractEndDate.DateTo,
                ScanDate.DateFrom,
                ScanDate.DateTo,
                ScrapDate.DateFrom,
                ScrapDate.DateTo,
                SearchFor,
                IsShowScrapped,
                RecordsOnPage,
                sf,
                RecordsCount);
        }

        public static string CreateFilterId()
        {
            return $"{TabName.Inventories}{InventoryFilterMode.Workstation}";
        }
    }
}