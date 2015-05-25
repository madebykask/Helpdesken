﻿namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Common.Enums.Settings;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.Case;

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
            bool isGlobalVisibility = model.caseFieldSettings.IsFieldVisible(caseFieldName);
            //bool isLocalVisibility = model.CaseSolutionSettingModels.Single(x => x.CaseSolutionField == caseTemplateFieldName).CaseSolutionMode != CaseSolutionModes.Hide;

            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);
            bool isLocalVisibility = (fieldSetting == null) ? false : fieldSetting.CaseSolutionMode != CaseSolutionModes.Hide;

            if (!isGlobalVisibility || !isLocalVisibility)
            {
                return "display:none";
            }

            return string.Empty;
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
            if (!isGlobalVisibility || !isLocalVisibility)
            {
                return false;
            }

            bool isReadOnly = model.EditMode == Enums.AccessMode.ReadOnly || fieldSetting.CaseSolutionMode == CaseSolutionModes.ReadOnly;

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

            bool isRequired = model.caseFieldSettings.CaseFieldSettingRequiredCheck(caseFieldName.ToString()) == 1;
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