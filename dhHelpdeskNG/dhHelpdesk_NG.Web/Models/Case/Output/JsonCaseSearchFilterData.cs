namespace DH.Helpdesk.Web.Models.Case.Output
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Enums;

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
                                     attrName = CaseFilterFields.InitiatorNameAttribute,
                                     value = string.Empty
                                 });
				res.data.Add(new JsonCaseSearchFilterItem()
				{
					attrName = CaseFilterFields.InitiatorSearchScopeAttribute,
					value = string.Empty
				});
			}

            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Region_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.RegionNameAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedRegion)
                });
            }

            // "Department"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Department_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.DepartmentNameAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedDepartments)
                });
            }

            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.User_Id.ToString()) == 1) 
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.RegisteredByNameAttribute,
                    value = defaultSearchFilter.lstRegisterBy
                });
            }

            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.CaseTypeIdNameAttribute,
                    value = defaultSearchFilter.CaseTypeId
                });
            }

            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.ProductAreaIdNameAttribute,
                    value = defaultSearchFilter.ProductAreaId
                });
            }

            // "Working group"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.WorkingGroupNameAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedWorkingGroup)
                });
            }

            // "Responsible"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString()) == 1)
            {
                 res.data.Add(new JsonCaseSearchFilterItem()
                                 {
                                     attrName = CaseFilterFields.ResponsibleNameAttribute,
                                     value = string.Empty
                                 });
            }

            // "Performer"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.PerformerNameAttribute,
                    value = defaultSearchFilter.lstAdministrator
                });
            }

            // "Priority"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.PriorityNameAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedPriority)
                });
            }

            // "Status"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Status_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.StatusNameAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedState)
                });
            }
            // "Category"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Category_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.CategoryNameAttribute,
                    value = defaultSearchFilter.CategoryId
                });
            }

            // "State secondary"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.StateSecondaryNameAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedSubState)
                });
            }

            // "Registration time"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.RegTime.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.CaseRegistrationDateStartFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseRegistrationDateStartFilter)
                });
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.CaseRegistrationDateEndFilterFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseRegistrationDateEndFilter)
                });
            }

            // "WatchDate"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.WatchDate.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.CaseWatchDateStartFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseWatchDateStartFilter)
                });
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.CaseWatchDateEndFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseWatchDateEndFilter)
                });
            }

            // "FinishingDate"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.FinishingDate.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.CaseClosingDateStartFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseClosingDateStartFilter)
                });
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.CaseClosingDateEndFilterNameAttribute,
                    value = FmtDate(defaultSearchFilter.CaseClosingDateEndFilter)
                });
            }

            // "Closing reason"
            if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.ClosingReason.ToString()) == 1)
            {
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.ClosingReasonNameAttribute,
                    value = defaultSearchFilter.ClosingReasonId
                });
            }

            // "CaseRemainingTime"
            //if (caseFieldSettings.getShowOnStartPage(GlobalEnums.TranslationCaseFields.Department_Id.ToString()) == 1)
            //{
                res.data.Add(new JsonCaseSearchFilterItem()
                {
                    attrName = CaseFilterFields.CaseRemainingTimeAttribute,
                    value = SaveExtractArray(defaultSearchFilter.SelectedCaseRemainingTime)
                });
            //}
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
