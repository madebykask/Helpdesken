using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.FinishingCause;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.Web.Common.Enums.Case;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Models.CaseSolution
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.Case;
    using CaseRules;
    using DH.Helpdesk.Domain.Cases;

    
    public class CaseSolutionInputViewModel
    {
        public CaseSolutionInputViewModel()
        {
            this.CaseSolutionSettingModels = CaseSolutionSettingModel.CreateDefaultModel();
        }

        public int Schedule { get; set; }
        public int ScheduleMonthly { get; set; }
        public int ScheduleMonthlyDay { get; set; }
        public int ScheduleMonthlyOrder { get; set; }
        public int ScheduleMonthlyWeekday { get; set; }
        public int ScheduleTime { get; set; }
        public int ScheduleType { get; set; }
        public int ScheduleWatchDate { get; set; }
        public string ScheduleDays { get; set; }
        public string ScheduleMonths { get; set; }
        public string[] ScheduleDay { get; set; }
        public string[] ScheduleMonth { get; set; }
        public string Finishing_Cause_Path { get; set; }
        public string ParantPath_ProductArea { get; set; }
        public string ParantPath_Category { get; set; }
        public string ParantPath_CaseType { get; set; }
        public int? CountryId { get; set; }

        public CaseSolution CaseSolution { get; set; }
        public UserSearch Users { get; set; }

        public IList<CaseSolutionSettingModel> CaseSolutionSettingModels { get; set; }
        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }
        public IList<SelectListItem> CsCategories { get; set; }
        public IList<CaseTypeOverview> CaseTypes { get; set; }
        public IList<SelectListItem> CaseWorkingGroups { get; set; }
//        public IList<SelectListItem> Categories { get; set; }
        public IList<CategoryOverview> Categories { get; set; }
        public IList<SelectListItem> Departments { get; set; }
        public IList<FinishingCauseOverview> FinishingCauses { get; set; }

        /// <summary>
        /// List of available case administrators
        /// </summary>
        public SelectList PerformerUsers { get; set; }

        public IList<SelectListItem> Priorities { get; set; }
        public IList<ProductAreaOverview> ProductAreas { get; set; }
        public IList<SelectListItem> projects { get; set; }
        //public IList<ProjectOverview> Projects { get; set; }
        public IList<SelectListItem> WorkingGroups { get; set; }
        public IList<SelectListItem> Regions { get; set; }
        public IList<ComputerUserCategoryOverview> UserSearchCategories{ get; set; }
        public IList<SelectListItem> OUs { get; set; }
        public IList<SelectListItem> Systems { get; set; }
        public IList<SelectListItem> Urgencies { get; set; }
        public IList<SelectListItem> Impacts { get; set; }
        public IList<SelectListItem> Status { get; set; }
        public IList<SelectListItem> StateSecondaries { get; set; }
        public IList<Country> Countries { get; set; }
        public IList<SelectListItem> Suppliers { get; set; }
        public IList<Currency> currencies { get; set; }
        public IList<SelectListItem> problems{ get; set; }
        public IList<SelectListItem> changes { get; set; }
        public IList<SelectListItem> CausingParts { get; set; }
        public IList<SelectListItem> RegistrationSources { get; set; }

        public string EmptyUserCategoryName { get; set; }

        public AccessMode EditMode { get; set; }
        public CaseFilesModel CaseFilesModel { get; set; }

        public IList<SelectListItem> IsAbout_Regions { get; set; }
        public IList<SelectListItem> IsAbout_Departments { get; set; }
        public IList<SelectListItem> IsAbout_OUs { get; set; }

        public IList<SelectListItem> ButtonList { get; set; }

        public IList<SelectListItem> ActionList { get; set; }
     
        public CaseRuleModel RuleModel { get; set; }

        public Setting CustomerSetting { get; set; }

        //public IList<SelectListItem> TabList { get; set; }
        public string DefaultTab { get; set; }
        public string ValidateOnChange { get; set; }
        public int? NextStepState { get; set; }

        public string SelectedCaseSecondaries { get; set; }

        public List<SelectListItem> CaseSolutionFieldSettings { get; set; }

        /// <summary>
        /// ///////
        /// </summary>
        public List<CaseSolutionSettingsField> CSSettingsField { get; set; }

        public List<CaseSolutionSettingsFieldModel> CSSelectedSettingsField { get; set; }
        public IList<CaseSolutionOverview> SplitToCaseSolutions { get; internal set; }


        public IList<SelectListItem> SplitToAllCaseSolutions { get; internal set; }

        public bool isCopy { get; set; }

        public int[] SplitToCaseSolutionIds { get; set; }

        public int? UserSearchCategory_Id { get; set; }
        public int? IsAbout_UserSearchCategory_Id { get; set; }
    }
}