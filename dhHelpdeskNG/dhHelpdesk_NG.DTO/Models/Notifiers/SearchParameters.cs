namespace DH.Helpdesk.BusinessData.Models.Notifiers
{
    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchParameters
    {
        public SearchParameters(
            int customerId,
            int? domainId,
            int? regionId,
            int? departmentId,
            int? organizationUnitId,
            int? divisionId,
            string pharse,
			int? computerUserCategoryID,
			NotifierStatus status,
            int recordsOnPage,
            SortField sortField)
        {
            this.CustomerId = customerId;
            this.DomainId = domainId;
            this.RegionId = regionId;
            this.DepartmentId = departmentId;
            this.OrganizationUnit = organizationUnitId;
            this.DivisionId = divisionId;
            this.Pharse = pharse;
			this.ComputerUserCategoryID = computerUserCategoryID;
			this.Status = status;
            this.SelectCount = recordsOnPage;
            this.SortField = sortField;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int? DomainId { get; private set; }

        [IsId]
        public int? RegionId { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [IsId]
        public int? OrganizationUnit { get; private set; }
        
        [IsId]
        public int? DivisionId { get; private set; }

        public string Pharse { get; private set; }

        public NotifierStatus Status { get; private set; }

        [IsId]
        public int SelectCount { get; private set; }

        public SortField SortField { get; private set; }

		public int? ComputerUserCategoryID { get; private set; }
	}
}