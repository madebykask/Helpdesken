namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DataAnnotationsExtensions;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public class WorkstationSearchModel
    {
        public WorkstationSearchModel(
            ConfigurableSearchFieldModel<List<SelectListItem>> regions,
            ConfigurableSearchFieldModel<List<SelectListItem>> departments,
            ConfigurableSearchFieldModel<List<SelectListItem>> computerTypes,
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
            this.Regions = regions;
            this.Departments = departments;
            this.ComputerTypes = computerTypes;
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

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> Regions { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> Departments { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> ComputerTypes { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> ContractStatuses { get; private set; }

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
    }
}