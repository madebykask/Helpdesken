using System;
using System;
using System.Collections.Generic;

using DH.Helpdesk.BusinessData.Models.Case;


namespace DH.Helpdesk.SelfService.Models.Case
{

    using DH.Helpdesk.Domain;
using DH.Helpdesk.BusinessData.Models;   
    public class CaseOverviewModel 
    {
        public CaseOverviewModel()
        { 

        }

        public CaseOverviewModel(string infoText, Case casePreview, List<string> CasefieldGroups, List<Log> caseLogs, List<CaseListToCase> fieldSettings)
        {
            this.InfoText = infoText;
            this.CasePreview = CasePreview;
            this.CaseFieldGroups = CaseFieldGroups;
            this.CaseLogs = CaseLogs;
            this.FieldSettings = fieldSettings;
        }

        public string InfoText { get; set; }

        public Case CasePreview { get; set; }

        public List<string> CaseFieldGroups { get; set; }

        public List<Log> CaseLogs { get; set; }

        public List<CaseListToCase> FieldSettings { get; set; }
                
    }
}
