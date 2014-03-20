using System;
using System;
using System.Collections.Generic;

using DH.Helpdesk.BusinessData.Models.Case;


namespace DH.Helpdesk.SelfService.Models.Case
{

    using DH.Helpdesk.Domain;   
    public class CaseOverviewModel 
    {
        public CaseOverviewModel()
        { 

        }

        public CaseOverviewModel(Case casePreview, List<Log> caseLogs)
        {
            this.CasePreview = CasePreview;
            this.CaseLogs = CaseLogs;
        }

        public Case CasePreview { get; set; }

        public List<Log> CaseLogs { get; set; }
        
        
    }
}
