using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseSolutionOverview
    {
        public int CaseSolutionId { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? StateSecondaryId { get; set; }
        public int? NextStepState { get; set; }
        public int SortOrder { get; set; }
        public int Status { get; set; }
        public int? ConnectedButton { get; set; }
        public string WorkingGroupName { get; set; }
        public StateSecondaryOverview StateSecondary { get; set; }
        public List<CaseSolutionConditionOverview> Conditions { get; set; }
        public int ShowInsideCase { get; set; }
        public int ShowOnCaseOverview { get; set; }
        public int? WorkingGroupId { get; set; }
		public string CaseSolutionCategoryName { get; internal set; }
		public int? CaseSolutionCategoryId { get; internal set; }
        public bool HasFinishingCauseId { get; set; }
	}
}