using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Domain;
using System.Collections.Generic;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.SelfService.Models.Case
{       
    public class ExtendedCaseViewModel
    {
        public ExtendedCaseViewModel()
        {
            CaseOU = null;
            Result = new ProcessResult("");
        }

        public int CustomerId { get; set; }
        public int LanguageId { get; set; }
        public int CaseId { get; set; }
        public int? CaseTemplateId { get; set; }
        public int? SelectedWorkflowStep { get; set; }
        public string UserRole { get; set; }
        public int StateSecondaryId { get; set; }
        public string CurrentUser { get; set; }
        public Customer CurrentCustomer { get; set; }
        public CaseModel CaseDataModel { get; set; }
        public Dictionary<string, string> StatusBar { get; internal set; }
        public ExtendedCaseDataModel ExtendedCaseDataModel { get; set; }
        public OU CaseOU { get; set; } 

        public ProcessResult Result { get; set; }

        public bool ShowRegistrationMessage { get; set; }

        public string CaseRegistrationMessage { get; set; }
        
        public string LogFileGuid { get; set; }
        

        public CaseLogModel CaseLogModel { get; set; }

        public ClosedCaseAlertModel ClosedCaseAlertModel { get; set; }
        public int AttachmentPlacement { get; set; }
        public bool ShowCommunicationForSelfservice { get; set; }
        public bool ShowCaseActionsPanelOnTop { get; set; }
        public bool ShowCaseActionsPanelAtBottom { get; set; }
        public ApplicationType ApplicationType { get; set; }

        public CaseControlsPanelModel CreateExtendedCaseControlsPanelModel(int position = 1)
        {
            return new CaseControlsPanelModel(position, true);
        }
    }
}