namespace DH.Helpdesk.Web.Models.Case
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Infrastructure;

    /// <summary>
    /// Case overview grid settings model
    /// </summary>
    public class CaseColumnsSettingsModel
    {
        #region Static fields

        public static IEnumerable<SelectListItem> FieldStyles
        {
            get
            {
                return new[]
                           {
                               new SelectListItem
                                   {
                                       Value = "colnormal",
                                       Text =
                                           @Translation.Get(
                                               "Normal",
                                               Enums.TranslationSource.TextTranslation)
                                   },
                               new SelectListItem
                                   {
                                       Value = "colwide",
                                       Text =
                                           @Translation.Get(
                                               "Bred",
                                               Enums.TranslationSource.TextTranslation)
                                   },
                               new SelectListItem
                                   {
                                       Value = "colnarrow",
                                       Text =
                                           @Translation.Get(
                                               "Smal",
                                               Enums.TranslationSource.TextTranslation)
                                   }
                           };
            }
        }

        public static IEnumerable<SelectListItem> FontStyles
        {
            get
            {
                return new[]
                           {
                               new SelectListItem
                                   {
                                       Value = "normaltext",
                                       Text =
                                           @Translation.Get(
                                               "Normal",
                                               Enums.TranslationSource.TextTranslation)
                                   },
                               new SelectListItem
                                   {
                                       Value = "smalltext",
                                       Text =
                                           @Translation.Get(
                                               "Mindre",
                                               Enums.TranslationSource.TextTranslation)
                                   },
                               new SelectListItem
                                   {
                                       Value = "smallertext",
                                       Text =
                                           @Translation.Get(
                                               "Minst",
                                               Enums.TranslationSource.TextTranslation)
                                   }
                           };
            }
        }

		public static IEnumerable<SelectListItem> PageSizes
		{
			get
			{
				return new[]
						   {
							   new SelectListItem { Value = "50", Text = "50" },
							   new SelectListItem { Value = "250", Text = "250" },
							   new SelectListItem { Value = "500", Text = "500" },
						   };
			}
		}

        public static IEnumerable<SelectListItem> PageSizesModal => new[]
        {
            new SelectListItem { Value = "5", Text = "5" },
            new SelectListItem { Value = "10", Text = "10" },
            new SelectListItem { Value = "15", Text = "15" },
        };

        #endregion

        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public IEnumerable<CaseOverviewGridColumnSetting> AvailableColumns { get; set; }

        public IEnumerable<CaseOverviewGridColumnSetting> SelectedColumns { get; set; }
        
        public IList<SelectListItem> LineList { get; set; }
   
        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }

        public string SelectedFontStyle { get; set; }

		public int SelectedPageSize { get; set; }

		public static IList<SelectListItem> GetColStyles(string selectedStyle)
        {
            var selectedVal = string.IsNullOrEmpty(selectedStyle) ? string.Empty : selectedStyle.ToLower();
            return FieldStyles.Select(fieldStyle => new SelectListItem() { Value = fieldStyle.Value, Selected = selectedVal == fieldStyle.Value.ToLower(), Text = fieldStyle.Text }).ToList();
        }
    }

    public sealed class CaseSettingModel
    {
        #region HTML Inputs names
        public const string DepartmentsControlName = "selectedDepartments";
        #endregion

        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public bool RegionCheck { get; set; }
        public IList<Region> Regions { get; set; }
        public string SelectedRegion { get; set; }

        /// <summary>
        /// Checkbox value for departments
        /// </summary>
        public bool IsDepartmentChecked { get; set; }

        /// <summary>
        /// List of available values for departments
        /// </summary>
        public IList<Department> Departments { get; set; }
        
        /// <summary>
        /// Selected values for departments represented as "1,2,44" string
        /// </summary>
        public string SelectedDepartments { get; set; }

        public bool RegisteredByCheck { get; set; }

        public bool CaseTypeCheck { get; set; }

        public string CaseTypePath { get; set; }

        public int CaseTypeId { get; set; }

        public IList<CaseType> CaseTypes { get; set; }

        public bool ProductAreaCheck { get; set; }
        public string ProductAreaPath { get; set; }
        public int ProductAreaId { get; set; }
        public IList<ProductArea> ProductAreas { get; set; }
        public bool CategoryCheck { get; set; }

        public string CategoryPath { get; set; }
        public int CategoryId { get; set; }
        public IList<Category> Categories { get; set; }
        //public string SelectedCategory { get; set; }
        public bool WorkingGroupCheck { get; set; }
        public IList<WorkingGroupEntity> WorkingGroups { get; set; }
        public string SelectedWorkingGroup { get; set; }

        public bool ResponsibleCheck { get; set; }

        public bool AdministratorCheck { get; set; }

        public bool PriorityCheck { get; set; }
        public IList<Priority> Priorities { get; set; }
        public string SelectedPriority { get; set; }

        public bool StateCheck { get; set; }
        public IList<Status> States { get; set; }
        public string SelectedState { get; set; }
        public bool SubStateCheck { get; set; }
        public IList<StateSecondary> SubStates { get; set; }        
        public string SelectedSubState { get; set; }

        public DateTime? CaseRegistrationDateStartFilter { get; set; }

        public DateTime? CaseRegistrationDateEndFilter { get; set; }

        public DateTime? CaseWatchDateStartFilter { get; set; }

        public DateTime? CaseWatchDateEndFilter { get; set; }

        public DateTime? CaseClosingDateStartFilter { get; set; }

        public DateTime? CaseClosingDateEndFilter { get; set; }

        public bool CaseRegistrationDateFilterShow { get; set; }

        public bool CaseWatchDateFilterShow { get; set; }

        public bool CaseClosingDateFilterShow { get; set; }

        public bool ClosingReasonCheck { get; set; }

        public IList<FinishingCause> ClosingReasons { get; set; }

        public int ClosingReasonId { get; set; }

        public string ClosingReasonPath { get; set; }

        public CaseColumnsSettingsModel ColumnSettingModel { get; set; }

        public bool CaseRemainingTimeChecked { get; set; }
        public IList<SelectListItem> filterCaseRemainingTime { get; set; }
        public string SelectedCaseRemainingTime { get; set; }

        /// <summary>
        /// Flag to display "filter by intitator" field on case overview page
        /// </summary>
        public bool CaseInitiatorFilterShow { get; set; }

        /// <summary>
        /// Available users for "registered by" case field in search settings
        /// </summary>
        public SelectList RegisteredByUserList { get; set; }

        /// <summary>
        /// Selected users in search settings for "Registered by" case field
        /// </summary>
        public int[] lstRegisterBy { get; set; }        
        
        /// <summary>
        /// Available users for "Administrator" case field in search settings
        /// </summary>
        public SelectList AvailablePerformersList { get; set; }

        /// <summary>
        /// Selected users in search settings for "Adminsitrator" case field
        /// </summary>
        public int[] lstAdministrator { get; set; }
    }
}