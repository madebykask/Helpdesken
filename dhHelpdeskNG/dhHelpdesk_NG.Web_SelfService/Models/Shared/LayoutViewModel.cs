using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Domain;
using DH.Helpdesk.SelfService.Models.Case;
using DH.Helpdesk.SelfService.Models.CaseTemplate;
using System.Collections.Generic;

namespace DH.Helpdesk.SelfService.Models.Shared
{
    public class LayoutViewModel
    {
        public LayoutViewModel()
        {            
        }

        public string AppType { get; set; }
        public Customer CurrentCustomer { get; set; }
        public int CustomerId { get; set; }
        public int? CurrentCase_Id { get; set; }
        public List<CaseTemplateTreeViewModel> CaseTemplatesGroups { get; set; }
        public bool OrderModuleIsEnabled { get; set; }
        public bool UserHasOrderTypes { get; set; }
        public bool HideMenu { get; set; }
        public bool ShowLanguage{ get; set; }
        public CaseLogModel CaseLog { get; set; }
        public bool HasError { get; set; }
        public string LoginType { get; set; }
        public bool IsMultiCustomerSearchEnabled { get; set; }

    }
}