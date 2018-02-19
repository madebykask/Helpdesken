using System;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseSolutionOverview
    {
        public int CaseSolutionId { get; set; }
        public string Name { get; set; }
        public int? StateSecondaryId { get; set; }
        public int? NextStepState { get; set; }
        public int SortOrder { get; set; }
        public int Status { get; set; }
        public int? ConnectedButton { get; set; }
        public StateSecondaryOverview StateSecondary { get; set; }
        public List<CaseSolutionConditionOverview> Conditions { get; set; }
    }

    public class CaseSolutionConditionOverview
    {
        public int Id { get; set; }
        public string Property { get; set; }
        public string Values { get; set; }
        
    }
}