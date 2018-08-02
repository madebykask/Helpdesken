using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DH.Helpdesk.SelfService.Models.Case
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
                                 IList<Region> regions, IList<System> systems, 
                                 IList<Supplier> suppliers,
                                 bool showRegistringMessage
            )
        {
            this.InfoText = infoText;            
            this.CasePreview = casePreview;
            this.CaseFieldGroups = caseFieldGroups;
            this.CaseLogs = caseLogs;
            this.FieldSettings = fieldSettings;
            this.Regions = regions;
            this.Systems = systems;
            this.Suppliers = suppliers;
            this.ShowRegistringMessage = showRegistringMessage;
        }

        public bool IsFinished
        {
            get { return CasePreview?.FinishingDate.HasValue ?? false; }
        }

        public bool ShowRegistringMessage { get; set; }

        public bool CanAddExternalNote { get; set; }

        public string ExLogFileGuid { get; set; }

        public string InfoText { get; set; }        

        public string CaseRegistrationMessage { get; set; }

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

        public string LogFileGuid { get; set; }  

        public Setting CustomerSettings { get; set; }
        public string FollowerUsers { get; set; }

        public CaseLogModel GetCaseLogModel()
        {
            return new CaseLogModel
            {
                CaseId = CasePreview?.Id ?? 0,
                CaseLogs = CaseLogs
            };
        }

        public ClosedCaseAlertModel GetClosedCaseAlertModel()
        {
            return new ClosedCaseAlertModel()
            {
                FinishingDate = CasePreview?.FinishingDate,
                CustomerSettings = CustomerSettings
            };
        }
    }
}
