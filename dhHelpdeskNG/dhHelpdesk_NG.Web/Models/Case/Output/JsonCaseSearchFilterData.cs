namespace DH.Helpdesk.Web.Models.Case.Output
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Web.Infrastructure.Extensions;

    public class JsonCaseSearchFilterItem
    {
        /// <summary>
        /// HTML control "name" attribute
        /// </summary>
        public string attrName;

        /// <summary>
        /// Value for control
        /// </summary>
        public object value;
    }

    public class JsonCaseSearchFilterData
    {
        public List<JsonCaseSearchFilterItem> data { get; set; }

        public static JsonCaseSearchFilterData MapFrom(CaseSettingModel defaultSearchFilter)
        {
            var caseFieldSettings = defaultSearchFilter.ColumnSettingModel.CaseFieldSettings;
            var res = new JsonCaseSearchFilterData()
                          {
                              data = new List<JsonCaseSearchFilterItem>()
                          };

            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Persons_Name.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                                 {
                                     attrName = CaseSearchFilter.InitiatorNameAttribute,
                                     value = string.Empty
                                 });
            }

            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Region_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.RegionNameAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedRegion)
                });
            }

            // "Department"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Department_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.DepartmentNameAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedDepartments)
                });
            }

            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.User_Id.ToString()) == 1) 
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.RegisteredByNameAttribute,
                    value = defaultSearchFilter.lstRegisterBy
                });
            }

            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.CaseTypeIdNameAttribute,
                    value = defaultSearchFilter.CaseTypeId
                });
            }

            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.ProductAreaIdNameAttribute,
                    value = defaultSearchFilter.ProductAreaId
                });
            }

            // "Working group"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.WorkingGroupNameAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedWorkingGroup)
                });
            }

            // "Responsible"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString()) == 1)
            {
                 res.data.Add(new JsonCaseSearchFilterItem()
                                 {
                                     attrName = CaseSearchFilter.ResponsibleNameAttribute,
                                     value = string.Empty
                                 });
            }

            // "Performer"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.PerformerNameAttribute,
                    value = defaultSearchFilter.lstAdministrator
                });
            }

            // "Priority"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.PriorityNameAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedPriority)
                });
            }

            // "Status"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Status_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.StatusNameAttribute,
                    value = string.Empty
                });
            }

            // "State secondary"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.StateSecondaryNameAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedSubState)
                });
            }

            // "Registration time"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.RegTime.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.CaseRegistrationDateStartFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseRegistrationDateStartFilter)
                });
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.CaseRegistrationDateEndFilterFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseRegistrationDateEndFilter)
                });
            }

            // "WatchDate"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.WatchDate.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.CaseWatchDateStartFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseWatchDateStartFilter)
                });
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.CaseWatchDateEndFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseWatchDateEndFilter)
                });
            }

            // "FinishingDate"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.FinishingDate.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.CaseClosingDateStartFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseClosingDateStartFilter)
                });
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.CaseClosingDateEndFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseClosingDateEndFilter)
                });
            }

            // "Closing reason"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.ClosingReason.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseSearchFilter.ClosingReasonNameAttribute,
                    value = defaultSearchFilter.ClosingReasonId
                });
            }

            return res;
        }
        
        private static string FmtDate(DateTime? dt)
        {
            return dt.HasValue
                       ? dt.Value.ToString(Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern)
                       : string.Empty;

        }

        private static string[] SaveExtractArray(string src)
        {
            if (string.IsNullOrEmpty(src))
            {
                return new string[0];
            }

            return src.Split(',');
        }
    }
}
