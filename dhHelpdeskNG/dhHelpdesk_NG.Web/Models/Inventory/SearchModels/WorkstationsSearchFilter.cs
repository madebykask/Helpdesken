namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using DataAnnotationsExtensions;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public class WorkstationsSearchFilter
    {
        public WorkstationsSearchFilter(
            int customerId,
            int? regionId,
            int? departmentId,
            int? computerTypeId,
            int? contractStatusId,
            DateRange contractStartDate,
            DateRange contractEndDate,
            DateRange scanDate,
            DateRange scrapDate,
            string searchFor,
            int recordsOnPage,
            bool isShowScrapped)
        {
            this.CustomerId = customerId;
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

        private WorkstationsSearchFilter(int customerId, int recordsOnPage)
        {
            this.CustomerId = customerId;
            this.RecordsOnPage = recordsOnPage;
            this.ContractStartDate = new DateRange();
            this.ContractEndDate = new DateRange();
            this.ScanDate = new DateRange();
            this.ScrapDate = new DateRange();
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int? RegionId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [IsId]
        public int? ComputerTypeId { get; private set; }

        [IsId]
        public int? ContractStatusId { get; private set; }

        [NotNull]
        public DateRange ContractStartDate { get; private set; }

        [NotNull]
        public DateRange ContractEndDate { get; private set; }

        [NotNull]
        public DateRange ScanDate { get; private set; }

        [NotNull]
        public DateRange ScrapDate { get; private set; }

        [LocalizedDisplay("Sök")]
        public string SearchFor { get; private set; }

        [Min(0)]
        [LocalizedDisplay("Poster per sida")]
        public int RecordsOnPage { get; private set; }

        [LocalizedDisplay("Visa även skrotade datorer")]
        public bool IsShowScrapped { get; private set; }

        public static WorkstationsSearchFilter CreateDefault(int customerId)
        {
            return new WorkstationsSearchFilter(customerId, 500);
        }

        public ComputersFilter CreateRequest()
        {
            return new ComputersFilter(
                this.CustomerId,
                this.DepartmentId,
                this.ComputerTypeId,
                this.ContractStatusId,
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