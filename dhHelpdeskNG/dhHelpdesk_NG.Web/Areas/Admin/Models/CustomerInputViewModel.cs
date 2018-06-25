using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using BusinessData.Models.CaseSolution;

    public class CustomerCaseSummaryViewModel
    {
        public int UserGroupId { get; set; }
        public string SortedFields { get; set; }
        public string Seperator { get; set; }

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

    public class OutputCaseField
    {
        public string Label { get; set; }

        public bool Enabled { get; set; }

        public string FieldName { get; set; }
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

        public IEnumerable<CaseSectionModel> CaseSections { get; set; }
        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        public IEnumerable<CaseFieldSettingsWithLanguage> CaseFieldSettingWithLangauges { get; set; }

        public IEnumerable<CaseFieldSettingLanguage> CaseFieldSettingLanguages { get; set; }
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
        public CustomSelectList UsMultiSelect { get; set; }
        public CustomSelectList AvUsMultiSelect { get; set; }
        public IList<SelectListItem> UserGroups { get; set; }

        public IList<SelectListItem> CWNSelect { get; set; }
        public IList<SelectListItem> LockedFieldOptions { get; set; }

        public CustomerCaseSummaryViewModel CustomerCaseSummaryViewModel { get; set; }

        /// <summary>
        /// Availiable representation modes of users first last name
        /// </summary>
        public SelectList UserFirstLastNameRepresentationList { get; set; }

        /// <summary>
        /// Selected representation modes of users first last name 
        /// </summary>
        public UserFirstLastNameModes UserFirstLastNameRepresentationId { get; set; }

        public IList<SelectListItem> CaseSolutionList { get; set; }

        public IList<int> ShowStatusBarIds { get; set; }
        public IList<int> ShowExternalStatusBarIds { get; set; }

        public IList<SelectListItem> SearchCategories { get; set; }
    }
}