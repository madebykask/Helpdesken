namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DataAnnotationsExtensions;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public class WorkstationsSearchFilter
    {
        public WorkstationsSearchFilter()
        {
        }

        public WorkstationsSearchFilter(
            int? regionId,
            int? departmentId,
            int? computerTypeId,
            ContractStatuses? contractStatusId,
            DateRange contractStartDate,
            DateRange contractEndDate,
            DateRange scanDate,
            DateRange scrapDate,
            string searchFor,
            int recordsOnPage,
            bool isShowScrapped)
        {
            this.RegionId = regionId;
            this.DepartmentId = departmentId;
            this.ComputerTypeId = computerTypeId;
            this.ContractStatusId = contractStatusId;
            this.ContractStartDate = contractStartDate;
            this.ContractEndDate = contractEndDate;
            this.ScanDate = scanDate;
            this.ScrapDate = scrapDate;
            this.SearchFor = searchFor;
            this.RecordsOnPage = recordsOnPage;
            this.IsShowScrapped = isShowScrapped;
        }

        private WorkstationsSearchFilter(int recordsOnPage)
        {
            this.RecordsOnPage = recordsOnPage;
            this.ContractStartDate = new DateRange();
            this.ContractEndDate = new DateRange();
            this.ScanDate = new DateRange();
            this.ScrapDate = new DateRange();
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

        public static WorkstationsSearchFilter CreateDefault()
        {
            return new WorkstationsSearchFilter(500);
        }

        public ComputersFilter CreateRequest(int customerId)
        {
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
                this.RecordsOnPage);
        }
    }
}