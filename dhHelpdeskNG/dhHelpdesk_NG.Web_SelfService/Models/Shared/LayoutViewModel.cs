using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Domain;
using DH.Helpdesk.SelfService.Models.CaseTemplate;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Language.Output;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Common.Types;

namespace DH.Helpdesk.SelfService.Models.Shared
{
    public class LayoutViewModel
    {
        public string AppType { get; set; }
        public bool IsLineManager { get; set; }
        public Customer CurrentCustomer { get; set; }
        public int CustomerId { get; set; }
        public List<CaseTemplateTreeViewModel> CaseTemplatesGroups { get; set; }
        public bool OrderModuleIsEnabled { get; set; }
        public bool UserHasOrderTypes { get; set; }
        public bool HideMenu { get; set; }
        public bool ShowLanguage{ get; set; }
        public bool HasError { get; set; }
        public LoginMode LoginMode { get; set; }
        public bool IsMultiCustomerSearchEnabled { get; set; }
        public IList<LanguageOverview> AllLanguages { get; set; }
        public int CurrentLanguageId { get; set; }
        public string CurrentSystemUser { get; set; }
        public UserIdentity CurrentUserIdentity { get; set; }
        public UserOverview CurrentLocalUser { get; set; }
    }
}