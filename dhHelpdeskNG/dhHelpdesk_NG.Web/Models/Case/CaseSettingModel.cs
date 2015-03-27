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
        public static readonly IEnumerable<SelectListItem> FieldStyles = new[]
            {
                new SelectListItem
                    {
                        Value = "colnormal",
                        Text =
                            @Translation.Get(
                                "Normal",
                                Enums
                            .TranslationSource
                            .TextTranslation)
                    },
                new SelectListItem
                    {
                        Value = "colwide",
                        Text =
                            @Translation.Get(
                                "Bred",
                                Enums
                            .TranslationSource
                            .TextTranslation)
                    },
                new SelectListItem
                    {
                        Value = "colnarrow",
                        Text =
                            @Translation.Get(
                                "Smal",
                                Enums
                            .TranslationSource
                            .TextTranslation)
                    }
            };

        public static readonly IEnumerable<SelectListItem> FontStyles = new[]
                {
                    new SelectListItem { Value = "normaltext", Text = @Translation.Get("Normal", Enums.TranslationSource.TextTranslation) },
                    new SelectListItem { Value = "smalltext", Text = @Translation.Get("Mindre", Enums.TranslationSource.TextTranslation) },
                    new SelectListItem { Value = "smallertext", Text = @Translation.Get("Minst", Enums.TranslationSource.TextTranslation) }
                };

        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public IEnumerable<CaseOverviewGridColumnSetting> AvailableColumns { get; set; }

        public IEnumerable<CaseOverviewGridColumnSetting> SelectedColumns { get; set; }
        
        public IList<SelectListItem> LineList { get; set; }
   
        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }

        public string SelectedFontStyle { get; set; }

        public static IList<SelectListItem> GetColStyles(string selectedStyle)
        {
            var selectedVal = string.IsNullOrEmpty(selectedStyle) ? string.Empty : selectedStyle.ToLower();
            return FieldStyles.Select(fieldStyle => new SelectListItem() { Value = fieldStyle.Value, Selected = selectedVal == fieldStyle.Value.ToLower(), Text = fieldStyle.Text }).ToList();
        }
    }

    public sealed class CaseSettingModel
    {
        public CaseSettingModel()
        {
            
        }

        public int CustomerId { get; set; }

        public int UserId { get; set; }
        public int RefreshContent { get; set; }

        public bool RegionCheck { get; set; }
        public IList<Region> Regions { get; set; }
        public string SelectedRegion { get; set; }

        public bool RegisteredByCheck { get; set; }
        public IList<User> RegisteredBy { get; set; }
        public string SelectedRegisteredBy { get; set; }

        public bool CaseTypeCheck { get; set; }

        public string CaseTypePath { get; set; }

        public int CaseTypeId { get; set; }

        public IList<CaseType> CaseTypes { get; set; }

        public bool ProductAreaCheck { get; set; }
        public string ProductAreaPath { get; set; }
        public int ProductAreaId { get; set; }
        public IList<ProductArea> ProductAreas { get; set; }

        public bool WorkingGroupCheck { get; set; }
        public IList<WorkingGroupEntity> WorkingGroups { get; set; }
        public string SelectedWorkingGroup { get; set; }

        public bool ResponsibleCheck { get; set; }

        public bool AdministratorCheck { get; set; }
        public IList<User> Administrators { get; set; }
        public string SelectedAdministrator { get; set; }

        public bool PriorityCheck { get; set; }
        public IList<Priority> Priorities { get; set; }
        public string SelectedPriority { get; set; }

        public bool StateCheck { get; set; }

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

        public string InitiatorName { get; set; }
    }
}