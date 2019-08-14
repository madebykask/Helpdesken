using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.SelfService.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models;

    using Log = DH.Helpdesk.Domain.Log;

    public class CaseOverviewModel 
    {
        public bool IsFinished
        {
            get { return CasePreview?.FinishingDate.HasValue ?? false; }
        }

        public int CaseId { get; set; }

        public int CustomerId { get; set; }

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

        public IList<Region> Regions { get; set; }

        public IList<System> Systems { get; set; }

        public IList<Supplier> Suppliers { get; set; }

        public List<CaseListToCase> FieldSettings { get; set; }

        public IEnumerable<CaseSectionModel> CaseSectionSettings { get; set; }

        public string FollowerUsers { get; set; }

        public FilesModel LogFilesModel { get; set; }

        public string LogFileGuid { get; set; }

        public CaseLogsModel CaseLogsModel { get; set; }

        public ClosedCaseAlertModel ClosedCaseAlertModel { get; set; }

        public ApplicationType ApplicationType { get; set; }

        public bool ShowCaseActionsPanelOnTop { get; set; }

        public bool ShowCaseActionsPanelAtBottom { get; set; }

        public int AttachmentPlacement { get; set; }

        public bool ShowCommunicationForSelfService { get; set; }

        #region Methods

        public CaseControlsPanelModel CreateCaseControlsPanelModel(int position = 1)
        {
            return new CaseControlsPanelModel(position, false);
        }

        public string BuildCaseLogDownloadUrlParams()
        {
            var urlParams = new List<string>();
            if (!string.IsNullOrEmpty(LogFileGuid))
            {
                urlParams.Add($"id={LogFileGuid}");
            }

            if (CaseId > 0)
            {
                urlParams.Add($"caseId={CaseId}");
            }
            return urlParams.Any() ? string.Join("&", urlParams) : string.Empty;
        }

        #endregion
    }
}
