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

        public List<Log> CaseLogs { get; set; }
        public string LogFileGuid { get; set; }
        public Setting CustomerSettings { get; set; }

        public CaseLogModel GetCaseLogModel()
        {
            return new CaseLogModel
            {
                CaseId = CaseId,
                CaseLogs = CaseLogs
            };
        }

        public ClosedCaseAlertModel GetClosedCaseAlertModel()
        {
            return new ClosedCaseAlertModel()
            {
                FinishingDate = CaseDataModel.FinishingDate,
                CustomerSettings = CustomerSettings
            };
        }

        public ExtendedCaseControlsPanelModel CreateExCaseControlsPanelModel(int position = 1)
        {
            return new ExtendedCaseControlsPanelModel(position, WorkflowSteps);
        }
    }

    public class ExtendedCaseControlsPanelModel
    {
        public ExtendedCaseControlsPanelModel(int poisition, List<WorkflowStepModel> workflowSteps)
        {
            Poisition = poisition;
            WorkflowSteps = workflowSteps;
        }

        public int Poisition { get; }
        public List<WorkflowStepModel> WorkflowSteps { get; }
    }
}