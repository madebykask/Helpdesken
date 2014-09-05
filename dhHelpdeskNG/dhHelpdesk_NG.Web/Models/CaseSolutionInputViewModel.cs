namespace DH.Helpdesk.Web.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    public class CaseSolutionIndexViewModel
    {
        public CaseSolutionIndexViewModel(string activeTab)
        {
            this.ActiveTab = activeTab;
        }

        public string SearchCss { get; set; }

        public CaseSolution CaseSolution { get; set; }

        public IEnumerable<CaseSolution> CSolutions { get; set; }

        public IList<CaseSolution> CaseSolutions { get; set; }

        public IList<CaseSolutionCategory> CaseSolutionCategories { get; set; }

        public string ActiveTab { get; set; }

    }

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
        public string ParantPath_CaseType { get; set; }

        public CaseSolution CaseSolution { get; set; }
        public UserSearch Users { get; set; }

        public IList<CaseSolutionSettingModel> CaseSolutionSettingModels { get; set; }
        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }
        public IList<SelectListItem> CsCategories { get; set; }
        public IList<CaseType> CaseTypes { get; set; }
        public IList<SelectListItem> CaseWorkingGroups { get; set; }
        public IList<SelectListItem> Categories { get; set; }
        public IList<SelectListItem> Departments { get; set; }
        public IList<FinishingCause> FinishingCauses { get; set; }
        public IList<SelectListItem> PerformerUsers { get; set; }
        public IList<SelectListItem> Priorities { get; set; }
        public IList<ProductArea> ProductAreas { get; set; }
        public IList<SelectListItem> Projects { get; set; }
        public IList<SelectListItem> WorkingGroups { get; set; }
    }
}