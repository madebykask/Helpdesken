using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Domain;

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
        public CaseModel CaseDataModel { get; set; }
        public ExtendedCaseDataModel ExtendedCaseDataModel { get; set; }
        public OU CaseOU { get; set; } 

        public ProcessResult Result { get; set; }
    }
    
}