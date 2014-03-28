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

        public CaseOverviewModel(string infoText, Case casePreview, List<string> caseFieldGroups, List<Log> caseLogs, List<CaseListToCase> fieldSettings)
        {
            this.InfoText = infoText;
            this.CasePreview = casePreview;
            this.CaseFieldGroups = caseFieldGroups;
            this.CaseLogs = caseLogs;
            this.FieldSettings = fieldSettings;
        }

        public string InfoText { get; set; }

        public Case CasePreview { get; set; }

        public List<string> CaseFieldGroups { get; set; }

        public List<Log> CaseLogs { get; set; }

        public List<CaseListToCase> FieldSettings { get; set; }

        public FilesModel LogFilesModel { get; set; }
                
    }
}
