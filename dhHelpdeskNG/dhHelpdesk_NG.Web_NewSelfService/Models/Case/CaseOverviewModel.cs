using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DH.Helpdesk.NewSelfService.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models;

    using Log = DH.Helpdesk.Domain.Log;

    

    public class CaseOverviewModel 
    {
        public CaseOverviewModel()
        { 

        }

        public CaseOverviewModel(string infoText, Case casePreview, List<string> caseFieldGroups, List<Log> caseLogs, List<CaseListToCase> fieldSettings,
                                 IList<Region> regions, IList<System> systems, IList<Supplier> suppliers )
        {
            this.InfoText = infoText;            
            this.CasePreview = casePreview;
            this.CaseFieldGroups = caseFieldGroups;
            this.CaseLogs = caseLogs;
            this.FieldSettings = fieldSettings;
            this.Regions = regions;
            this.Systems = systems;
            this.Suppliers = suppliers;
        }

        public bool CanAddExternalNote { get; set; }

        public string ExLogFileGuid { get; set; }

        public string InfoText { get; set; }        

        public string ReceiptFooterMessage { get; set; }

        public string MailGuid { get; set; }

        [StringLength(3000)]
        public string ExtraNote { get; set; }

        public Case CasePreview { get; set; }

        public List<string> CaseFieldGroups { get; set; }

        public List<Log> CaseLogs { get; set; }

        public IList<Region> Regions { get; set; }

        public IList<System> Systems { get; set; }

        public IList<Supplier> Suppliers { get; set; }

        public List<CaseListToCase> FieldSettings { get; set; }

        public FilesModel LogFilesModel { get; set; }
                
    }
  
}
