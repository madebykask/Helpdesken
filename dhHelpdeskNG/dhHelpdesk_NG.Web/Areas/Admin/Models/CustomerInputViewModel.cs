using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class CustomerCaseSummaryViewModel
    {
        public int UserGroupId { get; set; }

        public CaseSettings CSetting { get; set; }
        public Customer Customer { get; set; }

        public IList<CaseSettings> CaseSettings { get; set; }
        public IList<ListCases> ListSummaryForLabel { get; set; }
        public IList<SelectListItem> CaseFieldSetting { get; set; }
     
        public IList<CaseFieldSettingsWithLanguage> CaseFieldSettingLanguages { get; set; }
        public IList<SelectListItem> LineList { get; set; }

        public CustomerInputViewModel ViewModel { get; set; }
    }

    public class CustomerIndexViewModel
    {
        public string SearchCs { get; set; }

        public Customer Customer { get; set; }

        public IList<Customer> Customers { get; set; }
    }

    public class CustomerInputViewModel : BaseTabInputViewModel
    {
        public int ConfirmPassword { get; set; }
        public int NewPassword { get; set; }
        public int OrderPermission { get; set; }
        public int PasswordHis { get; set; }
        public int CreateCaseFromOrder { get; set; }

        public Customer Customer { get; set; }
        public Setting Setting { get; set; }
        public Language Language { get; set; }
        public UserGroup UserGroup { get; set; }

        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        public IEnumerable<CaseFieldSettingsWithLanguage> CaseFieldSettingWithLangauges { get; set; }

        public CaseFieldSetting CaseFieldSetting { get; set; }
        public CaseFieldSettingLanguage CaseFieldSettingLanguage { get; set; }
        public CaseFieldSettingsWithLanguage CaseFieldSettingWithLangauge { get; set; }

        public IList<CaseListToCase> ListCaseForLabel { get; set; }
        public IList<CustomerReportList> ListCustomerReports { get; set; }

        public IList<SelectListItem> Customers { get; set; }
        public IList<SelectListItem> Languages { get; set; }
        public IList<SelectListItem> MinimumPasswordLength { get; set; }
        public IList<SelectListItem> PasswordHistory { get; set; }
        public IList<SelectListItem> UsAvailable { get; set; } 
        public IList<SelectListItem> UsSelected { get; set; }
        public IList<SelectListItem> UserGroups { get; set; }

        public CustomerCaseSummaryViewModel CustomerCaseSummaryViewModel { get; set; }
    }
}