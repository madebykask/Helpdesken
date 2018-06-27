using DH.Helpdesk.BusinessData.Models;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Common.Enums.Settings;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models;
    using Models.CaseSolution;

    public static class GeneralExtensions
    {
        public static bool UserHasRole(UserOverview user, string roleToHave)
        {
            var bolRet = false;

            if (user != null)
                if (user.UserGroupId.ToString() == roleToHave)
                {
                    bolRet = true;
                }

            return bolRet;
        }

        public static bool UserHasPermission(UserOverview user, string permissionName)
        {
            return user != null && (int)user.GetType().GetProperty(permissionName).GetValue(user) == 1;
        }

        public static string ReturnSelectedValueCaseSolution(int id, IEnumerable<CaseSolutionSchedule> css)
        {
            string strRet = "";

            if (css != null)
            {
                foreach (CaseSolutionSchedule s in css)
                {
                    if (s.CaseSolution_Id == id)
                    {
                        strRet = "checked";
                        break;
                    }
                }
            }

            return strRet;
        }

        public static string CaseSolution_SetSelectedScheduleDay_Day(this HtmlHelper html, string compare, CaseSolutionSchedule css)
        {
            if (css != null)
            {
                foreach (var s in css.ScheduleDay.Split(','))
                {
                    if (s == compare)
                        return " checked=\"checked\"";
                }
            }

            return "";
        }

        public static string returnMaskedPwdForCustomerSettings(int id, string ldapPassword)
        {
            if (id == 0 && ldapPassword == null || ldapPassword == "")
                return string.Empty;
            else
                return "|||||||||||";
        }

        public static string GetFieldStyle(this CaseInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);

           // if (!model.caseFieldSettings.IsFieldRequired(caseFieldName) && fieldSetting != null && fieldSetting.CaseSolutionMode == CaseSolutionModes.Hide)
            if (fieldSetting != null && fieldSetting.CaseSolutionMode == CaseSolutionModes.Hide)
                return "display:none";

            if ((model.caseFieldSettings.IsFieldVisible(caseFieldName))
                || (CaseSolutionSettingModel.IsFieldAlwaysVisible(caseTemplateFieldName) || model.caseFieldSettings.IsFieldRequiredOrVisible(caseFieldName)))
            {
                return string.Empty;
            }
            
            return "display:none";
        }

        public static string GetFieldStyle(this CaseSolutionInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);

            // if (!model.caseFieldSettings.IsFieldRequired(caseFieldName) && fieldSetting != null && fieldSetting.CaseSolutionMode == CaseSolutionModes.Hide)
            if (fieldSetting != null && fieldSetting.CaseSolutionMode == CaseSolutionModes.Hide)
                return "display:none";

            if ((model.CaseFieldSettings.IsFieldVisible(caseFieldName))
                || (CaseSolutionSettingModel.IsFieldAlwaysVisible(caseTemplateFieldName) || model.CaseFieldSettings.IsFieldRequiredOrVisible(caseFieldName)))
            {
                return string.Empty;
            }

            return "display:none";
        }

        public static IList<SelectListItem> BuildComputerCategoriesSelectList(this CaseInputViewModel model, int? selectedCategoryId)
        {
            var computerCategoriesSelectList =
                BuildComputerCategoriesSelectListInner(model.ComputerUserCategories, selectedCategoryId, model.EmptyComputerCategoryName);

            return computerCategoriesSelectList;
        }

        public static IList<SelectListItem> BuildComputerCategoriesSelectList(this CaseSolutionInputViewModel model, int? selectedCategoryId)
        {
            var computerCategoriesSelectList =
                BuildComputerCategoriesSelectListInner(model.UserSearchCategories, selectedCategoryId, model.EmptyUserCategoryName);

            return computerCategoriesSelectList;
        }

        private static IList<SelectListItem> BuildComputerCategoriesSelectListInner(IList<ComputerUserCategoryOverview> categories, int? selectedCategoryId, string emptyCategoryName)
        {
            var computerCategoriesSelectList = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = Translation.GetCoreTextTranslation(emptyCategoryName ?? "Employee"),
                    Value = "null"
                }
            };

            computerCategoriesSelectList.AddRange(categories.Select(o => new SelectListItem()
            {
                Text = o.Name,
                Value = o.Id.ToString(),
                Selected = selectedCategoryId.HasValue && o.Id == selectedCategoryId.Value
            }));;
            
            return computerCategoriesSelectList.ToList();
        }

        public static string GetCaseTemplateFieldStyle(this CaseInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            //bool isGlobalVisibility = model.caseFieldSettings.IsFieldVisible(caseFieldName);
            
            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);
            bool isLocalVisibility = (fieldSetting == null) ? false : fieldSetting.CaseSolutionMode != CaseSolutionModes.Hide;

            if (!isLocalVisibility)
            {
                return "display:none";
            }

            return string.Empty;
        }

        public static string GetFieldStyle(this CaseInputViewModel model, CaseSolutionFields caseTemplateFieldName)
        {
            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);
            bool isLocalVisibility = (fieldSetting != null) && fieldSetting.CaseSolutionMode != CaseSolutionModes.Hide;

            return !isLocalVisibility ? "display:none" : string.Empty;
        }

        public static bool IsReadOnly(this CaseInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            bool isGlobalVisibility = model.caseFieldSettings.IsFieldVisible(caseFieldName);
            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);
            bool isLocalVisibility = (fieldSetting != null) && fieldSetting.CaseSolutionMode != CaseSolutionModes.Hide;
            
            if(model.DynamicCase != null && (!model.CurrentUserRole.IsCustomerOrSystemAdminRole() && model.caseFieldSettings.IsFieldLocked(caseFieldName)))
                return true;

            if (model.ExtendedCases != null && model.ExtendedCases.Count > 0 && (!model.CurrentUserRole.IsCustomerOrSystemAdminRole() && model.caseFieldSettings.IsFieldLocked(caseFieldName) ))
                return true;

            if (!isGlobalVisibility || !isLocalVisibility)
            {
                return false;
            }

            bool isReadOnly = model.EditMode == Enums.AccessMode.ReadOnly || fieldSetting.CaseSolutionMode == CaseSolutionModes.ReadOnly;

            return isReadOnly;
        }

        public static bool IsReadOnly(this CaseInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName)
        {
            bool isGlobalVisibility = model.caseFieldSettings.IsFieldVisible(caseFieldName);

            if (model.DynamicCase != null && model.caseFieldSettings.IsFieldLocked(caseFieldName))
                return true;

            if (!isGlobalVisibility)
            {
                return false;
            }

            bool isReadOnly = model.EditMode == Enums.AccessMode.ReadOnly;

            return isReadOnly;
        }

        public static bool IsReadOnly(this CaseInputViewModel model, CaseSolutionFields caseTemplateFieldName)
        {
            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);
            bool isLocalVisibility = (fieldSetting != null) && fieldSetting.CaseSolutionMode != CaseSolutionModes.Hide;
            if (!isLocalVisibility)
            {
                return false;
            }

            bool isReadOnly = model.EditMode == Enums.AccessMode.ReadOnly || fieldSetting.CaseSolutionMode == CaseSolutionModes.ReadOnly;

            return isReadOnly;
        }

        public static bool IsRequired(this CaseInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            bool isGlobalVisibility = model.caseFieldSettings.IsFieldVisible(caseFieldName);
            //bool isLocalVisibility = model.CaseSolutionSettingModels.Single(x => x.CaseSolutionField == caseTemplateFieldName).CaseSolutionMode != CaseSolutionModes.Hide;

            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);
            bool isLocalVisibility = (fieldSetting == null) ? false : fieldSetting.CaseSolutionMode != CaseSolutionModes.Hide;

            if (!isGlobalVisibility || !isLocalVisibility)
            {
                return false;
            }

            bool isRequired = model.caseFieldSettings.CaseFieldSettingRequiredCheck(caseFieldName.ToString(), model.IsCaseReopened) == 1;
            return isRequired;
        }

        public static string GetDisabledString(
            this CaseInputViewModel model,
            GlobalEnums.TranslationCaseFields caseFieldName,
            CaseSolutionFields caseTemplateFieldName)
        {
            return model.IsReadOnly(caseFieldName, caseTemplateFieldName) ? "disabled" : string.Empty;
        }
    }
}