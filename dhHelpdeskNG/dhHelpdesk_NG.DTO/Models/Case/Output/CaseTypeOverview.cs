using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
    public sealed class CaseTypeOverview
    {
        public CaseTypeOverview()
        {
            SubCaseTypes = new List<CaseTypeOverview>();
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public int ShowOnExternalPage { get; set; }
        //For next release #57742
        public int ShowOnExtPageCases { get; set; }

        public int IsActive { get; set; }
            
        public int Selectable { get; set; }

        public List<CaseTypeOverview> SubCaseTypes { get; set; }
    }
}