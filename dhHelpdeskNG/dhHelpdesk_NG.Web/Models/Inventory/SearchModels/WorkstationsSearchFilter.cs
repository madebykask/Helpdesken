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
            ConfigurableSearchFieldModel<DateRange> contractStartDate,
            ConfigurableSearchFieldModel<DateRange> contractEndDate,
            ConfigurableSearchFieldModel<DateRange> scanDate,
            ConfigurableSearchFieldModel<DateRange> scrapDate,
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
        public ConfigurableSearchFieldModel<DateRange> ContractStartDate { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<DateRange> ContractEndDate { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<DateRange> ScanDate { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<DateRange> ScrapDate { get; private set; }

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
                this.ContractStartDate.Value.DateFrom,
                this.ContractStartDate.Value.DateTo,
                this.ContractEndDate.Value.DateFrom,
                this.ContractEndDate.Value.DateTo,
                this.ScanDate.Value.DateFrom,
                this.ScanDate.Value.DateTo,
                this.ScrapDate.Value.DateFrom,
                this.ScrapDate.Value.DateTo,
                this.SearchFor,
                this.IsShowScrapped);
        }
    }
}