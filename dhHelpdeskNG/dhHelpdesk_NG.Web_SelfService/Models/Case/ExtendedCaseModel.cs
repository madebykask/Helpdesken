using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Domain;
using System.Collections.Generic;

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
        public List<WorkflowStepModel> WorkflowSteps { get; set; }
        public OU CaseOU { get; set; } 

        public ProcessResult Result { get; set; }

        public bool ShowRegistrationMessage { get; set; }
        public string CaseRegistrationMessage { get; set; }
        
        public string LogFileGuid { get; set; }
        public Setting CustomerSettings { get; set; }

        public CaseLogModel CaseLogModel { get; set; }

        public ClosedCaseAlertModel GetClosedCaseAlertModel()
        {
            return new ClosedCaseAlertModel()
            {
                FinishingDate = CaseDataModel.FinishingDate,
                CustomerSettings = CustomerSettings
            };
        }

        public CaseControlsPanelModel CreateCaseControlsPanelModel(int position = 1)
        {
            return new CaseControlsPanelModel(position, WorkflowSteps, false);
        }

        public CaseControlsPanelModel CreateExtendedCaseControlsPanelModel(int position = 1)
        {
            return new CaseControlsPanelModel(position, WorkflowSteps, true);
        }
    }

    public class CaseControlsPanelModel
    {
        public CaseControlsPanelModel(int position, List<WorkflowStepModel> workflowSteps, bool isExtendedCase)
        {
            IsExtendedCase = isExtendedCase;
            Position = position;
            WorkflowSteps = workflowSteps;
        }

        public int Position { get; }
        public bool IsExtendedCase { get; }
        public List<WorkflowStepModel> WorkflowSteps { get; }
    }
}