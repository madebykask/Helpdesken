using System;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.Web.Common.Enums.Case;

namespace DH.Helpdesk.Models.CasesOverview
{
    public class SearchOverviewFilterInputModel
    {
        public int? CurrentCaseId { get; set; }
        public IList<int> CustomersIds { get; set; }
        public string Initiator { get; set; }
        public int? CaseTypeId { get; set; }
        public CaseInitiatorSearchScope? InitiatorSearchScope { get; set; }
        public int? ProductAreaId { get; set; }
        public int? CategoryId { get; set; }
        public IList<int> RegionIds { get; set; }
        public IList<int> RegisteredByIds { get; set; }
        public IList<int> WorkingGroupIds { get; set; }
        public IList<int> ResponsibleUserIds { get; set; }
        public IList<int> PerfomerUserIds { get; set; }
        public IList<int> PriorityIds { get; set; }
        public IList<int> StatusIds { get; set; }
        public IList<int> StateSecondaryIds { get; set; }
        public DateTime? CaseRegistrationDateStartFilter { get; set; }
        public DateTime? CaseRegistrationDateEndFilter { get; set; }
        public DateTime? CaseWatchDateStartFilter { get; set; }
        public DateTime? CaseWatchDateEndFilter { get; set; }
        public DateTime? CaseClosingDateStartFilter { get; set; }
        public DateTime? CaseClosingDateEndFilter { get; set; }
        public int? CaseClosingReasonId { get; set; }
        public bool SearchInMyCasesOnly { get; set; }
        public bool IncludeExtendedCaseValues { get; set; }
        public bool IsConnectToParent { get; set; }
        public CaseProgressFilterEnum CaseProgress { get; set; }
        public int? CaseFilterFavoriteId { get; set; }
        public string FreeTextSearch { get; set; }
        public IList<int> DepartmentIds { get; set; }
        public IList<int> OrganizationUnitIds { get; set; }
        public RemainingTimes? CaseRemainingTime { get; set; }
        public int? CaseRemainingTimeFilter { get; set; }
        public int? CaseRemainingTimeUntilFilter { get; set; }
        public int? CaseRemainingTimeMaxFilter { get; set; }
        public bool? CaseRemainingTimeHoursFilter { get; set; }
        public int? PageSize { get; set; }
        public int? Page { get; set; }
        public string OrderBy { get; set; }
        public bool? Ascending { get; set; }
        public bool CountOnly { get; set; }
    }
}
