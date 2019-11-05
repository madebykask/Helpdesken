namespace DH.Helpdesk.Services.Requests.Inventory
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputersFilter
    {
        public ComputersFilter(int customerId,
            int? domainId,
            int? regionId,
            int? unitId,
            int? departmentId,
            int? computerTypeId,
            int? contractStatusId,
            DateTime? contractStartDateFrom,
            DateTime? contractStartDateTo,
            DateTime? contractEndDateFrom,
            DateTime? contractEndDateTo,
            DateTime? scanDateFrom,
            DateTime? scanDateTo,
            DateTime? scrapDateFrom,
            DateTime? scrapDateTo,
            string searchFor,
            bool isShowScrapped,
            int recordsOnPage,
            SortField sortField,
            int? recordsCount)
        {
            SortField = sortField;
            CustomerId = customerId;
            DomainId = domainId;
            RegionId = regionId;
            UnitId = unitId;
            DepartmentId = departmentId;
            ComputerTypeId = computerTypeId;
            ContractStatusId = contractStatusId;
            ContractStartDateFrom = contractStartDateFrom;
            ContractStartDateTo = contractStartDateTo;
            ContractEndDateFrom = contractEndDateFrom;
            ContractEndDateTo = contractEndDateTo;
            ScanDateFrom = scanDateFrom;
            ScanDateTo = scanDateTo;
            ScrapDateFrom = scrapDateFrom;
            ScrapDateTo = scrapDateTo;
            SearchFor = searchFor;
            IsShowScrapped = isShowScrapped;
            RecordsOnPage = recordsOnPage;
            RecordsCount = recordsCount;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int? DomainId { get; private set; }

        [IsId]
        public int? RegionId { get; private set; }

        [IsId]
        public int? UnitId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [IsId]
        public int? ComputerTypeId { get; private set; }

        [IsId]
        public int? ContractStatusId { get; private set; }

        public DateTime? ContractStartDateFrom { get; private set; }

        public DateTime? ContractStartDateTo { get; private set; }

        public DateTime? ContractEndDateFrom { get; private set; }

        public DateTime? ContractEndDateTo { get; private set; }

        public DateTime? ScanDateFrom { get; private set; }

        public DateTime? ScanDateTo { get; private set; }

        public DateTime? ScrapDateFrom { get; private set; }

        public DateTime? ScrapDateTo { get; private set; }

        public string SearchFor { get; private set; }

        [MinValue(0)]
        public int RecordsOnPage { get; private set; }

        public bool IsShowScrapped { get; private set; }

        public SortField SortField { get; private set; }

        public int? RecordsCount { get; set; }
    }
}