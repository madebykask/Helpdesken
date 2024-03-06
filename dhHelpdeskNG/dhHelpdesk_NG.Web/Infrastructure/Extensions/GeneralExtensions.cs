using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Web.Common.Enums.Case;

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
    using System.Web;
    using HtmlAgilityPack;
    using System.Text.RegularExpressions;
    using System;

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
        public static string StripHTML(this string input)
        {
            if (input == null)
            {
                return string.Empty;
            }
            input.Replace("<br>", "&nbsp;");
            input.Replace("<br />", "&nbsp;");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(input);
            var output = "";
            foreach (HtmlNode node in doc.DocumentNode.ChildNodes)
            {
                output += node.InnerText;
            }

            output = output.Replace("<", "").Replace(">", "").Replace("/", "").Replace("\\", "");
            return output;

        }
        public static string RemoveEmptyTagsExceptImg(this string input)
        {

            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(input);
            //Checking if there is any image tag in the html
            bool containsImgTag = doc.DocumentNode.SelectSingleNode("//img") != null;
            //If no image or text, we shall not save a lognote
            if (string.IsNullOrWhiteSpace(doc.DocumentNode.InnerText) && !containsImgTag)
            {
                return "";
            }
            else
            {
                return doc.DocumentNode.InnerHtml;
            }
        }
        public static string HTMLToTableCell(this string input)
        {

            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            input = input.Replace("\r\n", "<br />").Replace("\n\r", "<br />");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(input);

            HtmlNodeCollection tables = doc.DocumentNode.SelectNodes("//table[@style]");
            if (tables != null)
            {
                foreach (HtmlNode table in tables)
                {
                    if(table.Attributes["style"] != null)
                    {
                        table.Attributes["style"].Remove();
                    }
                    if(table.Attributes["width"] != null)
                    {
                        table.Attributes["width"].Remove();
                    }
                }
            }
            HtmlNodeCollection trs = doc.DocumentNode.SelectNodes("//tr[@style]");
            if (trs != null)
            {
                foreach (HtmlNode tr in trs)
                {
                    if (tr.Attributes["style"] != null)
                    {
                        tr.Attributes["style"].Remove();
                    }
                }
            }

            HtmlNodeCollection divs = doc.DocumentNode.SelectNodes("//div[@style]");
            if (divs != null)
            {
                foreach (HtmlNode div in divs)
                {
                    string style = div.Attributes["style"].Value;
                    string newStyle = CleanWidth(style);
                    div.Attributes["style"].Value = newStyle;

                }
            }

            HtmlNodeCollection a = doc.DocumentNode.SelectNodes("//a[@style]");
            if (a != null)
            {
                foreach (HtmlNode singlea in a)
                {
                    string style = singlea.Attributes["style"].Value;
                    string newStyle = CleanWidth(style);
                    singlea.Attributes["style"].Value = newStyle;
                                       
                }
            }

            HtmlNodeCollection h = doc.DocumentNode.SelectNodes("//h[@style]");
            if (h != null)
            {
                foreach (HtmlNode singleh in h)
                {
                    if(singleh.Attributes["style"] != null)
                    {
                        singleh.Attributes["style"].Remove();
                    }                   
                }
            }
            HtmlNodeCollection a2 = doc.DocumentNode.SelectNodes("//a");
            if (a2 != null)
            {
                foreach (HtmlNode singlea in a2)
                {
                    if (singlea.Attributes["href"]!= null)
                    {
                        singlea.Attributes.Add("class", "textblue");
                    }
                }
            }

            HtmlNodeCollection imgs = doc.DocumentNode.SelectNodes("//img[@style]");
            if (imgs != null)
            {
                foreach (HtmlNode img in imgs)
                {
                    string style = img.Attributes["style"].Value;
                    string newStyle = CleanWidth(style);
                    img.Attributes["style"].Remove();

                }
            }

            HtmlNode allNodes = doc.DocumentNode;
            var ret = allNodes.InnerHtml;

            return ret;
        }
        
        private static string CleanWidth(string oldStyles)
        {
            string newStyles = "";
            foreach (var entries in oldStyles.Split(';'))
            {
                var values = entries.Split(':');
                switch (values[0].ToLower().Trim())
                {

                    case "width":
                        break;
                        
                    default:
                        if (values.Length == 2)
                        {
                            newStyles += values[0] + ":" + values[1] + ";";
                        }

                        break;

                }
            }
            return newStyles;
        }

        public static string GetFieldStyle(this CaseInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            return GetFieldStyleInner(model.caseFieldSettings, model.CaseSolutionSettingModels, caseFieldName, caseTemplateFieldName);
        }

        public static string GetFieldStyle(this CaseLogViewModel model, GlobalEnums.TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            return GetFieldStyleInner(model.CaseFieldSettings, model.CaseSolutionSettingModels, caseFieldName, caseTemplateFieldName);
        }

        private static string GetFieldStyleInner(IList<CaseFieldSetting> caseFieldSettings, 
            IList<CaseSolutionSettingModel> caseSolutionSettingModels, 
            GlobalEnums.TranslationCaseFields caseFieldName, 
            CaseSolutionFields caseTemplateFieldName)
        {
            var fieldSetting = caseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);

            // if (!model.caseFieldSettings.IsFieldRequired(caseFieldName) && fieldSetting != null && fieldSetting.CaseSolutionMode == CaseSolutionModes.Hide)
            if (fieldSetting != null && fieldSetting.CaseSolutionMode == CaseSolutionModes.Hide)
                return "display:none";

            if (caseFieldSettings.IsFieldVisible(caseFieldName) ||
                CaseSolutionSettingModel.IsFieldAlwaysVisible(caseTemplateFieldName) || 
                caseFieldSettings.IsFieldRequiredOrVisible(caseFieldName))
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

        public static IList<SelectListItem> BuildComputerCategoriesSelectList(this CaseInputViewModel model, int? selectedCategoryId, bool addEmpty = false)
        {
            var computerCategoriesSelectList =
                BuildComputerCategoriesSelectListInner(model.ComputerUserCategories, selectedCategoryId, model.EmptyComputerCategoryName);

            if (addEmpty)
            {
                computerCategoriesSelectList.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = ""
                });
            }

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
            var computerCategoriesSelectList = categories.Select(o => new SelectListItem()
            {
                Text = Translation.GetMasterDataTranslation(o.Name),
                Value = o.Id.ToString(),
                Selected = selectedCategoryId.HasValue && o.Id == selectedCategoryId.Value
            }).OrderBy(x => x.Text).ToList();
            
            computerCategoriesSelectList.Insert(0, new SelectListItem()
            {
                Text = Translation.GetMasterDataTranslation(emptyCategoryName ?? ComputerUserCategory.EmptyCategoryDefaultName),
                Value = ComputerUserCategory.EmptyCategoryId.ToString(),
                Selected = selectedCategoryId == ComputerUserCategory.EmptyCategoryId //used for empty category
            });

            return computerCategoriesSelectList.ToList();
        }

        public static string GetCaseTemplateFieldStyle(this CaseInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            //bool isGlobalVisibility = model.caseFieldSettings.IsFieldVisible(caseFieldName);
            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);
            var isLocalVisibility = (fieldSetting != null) && fieldSetting.CaseSolutionMode != CaseSolutionModes.Hide;

            if (!isLocalVisibility)
            {
                return "display:none";
            }

            return string.Empty;
        }

        public static string GetFieldStyle(this CaseInputViewModel model, CaseSolutionFields caseTemplateFieldName)
        {
            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);
            var isLocalVisibility = (fieldSetting != null) && fieldSetting.CaseSolutionMode != CaseSolutionModes.Hide;

            return !isLocalVisibility ? "display:none" : string.Empty;
        }

        public static bool IsCaseFieldVisible(this CaseInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            var isGlobalVisibility = model.caseFieldSettings.IsFieldVisible(caseFieldName);

            //check visibility in case template options
            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);
            var isLocalVisibility = fieldSetting != null && fieldSetting.CaseSolutionMode != CaseSolutionModes.Hide;

            return isGlobalVisibility && isLocalVisibility;
        }

        public static bool IsReadOnly(this CaseInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            if (model.DynamicCase != null && (!model.CurrentUserRole.IsCustomerOrSystemAdminRole() && model.caseFieldSettings.IsFieldLocked(caseFieldName)))
                return true;

            if (model.ExtendedCases != null && model.ExtendedCases.Count > 0 && (!model.CurrentUserRole.IsCustomerOrSystemAdminRole() && model.caseFieldSettings.IsFieldLocked(caseFieldName)))
                return true;

            var isVisible = IsCaseFieldVisible(model, caseFieldName, caseTemplateFieldName);
            if (!isVisible)
                return false;

            var fieldTemplateSettings = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);
            var isTemplateReadonly = fieldTemplateSettings != null && (fieldTemplateSettings.CaseSolutionMode == CaseSolutionModes.ReadOnly);
            var isReadOnly = model.EditMode == AccessMode.ReadOnly || isTemplateReadonly;

            if (model.DisableCaseFields != null && model.DisableCaseFields.Contains(caseTemplateFieldName.GetName()))
            {
                isReadOnly = true;
            }

            return isReadOnly;
        }

        public static bool IsReadOnly(this CaseInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName)
        {
            if (model.DynamicCase != null && model.caseFieldSettings.IsFieldLocked(caseFieldName))
                return true;

            var isGlobalVisibility = model.caseFieldSettings.IsFieldVisible(caseFieldName);
            if (!isGlobalVisibility)
            {
                return false;
            }

            var isReadOnly = model.EditMode == AccessMode.ReadOnly;
            return isReadOnly;
        }

        public static bool IsReadOnly(this CaseInputViewModel model, CaseSolutionFields caseTemplateFieldName)
        {
            var fieldSetting = model.CaseSolutionSettingModels.FirstOrDefault(x => x.CaseSolutionField == caseTemplateFieldName);
            var isLocalVisibility = (fieldSetting != null) && fieldSetting.CaseSolutionMode != CaseSolutionModes.Hide;
            if (!isLocalVisibility)
                return false;

            var isReadOnly = model.EditMode == AccessMode.ReadOnly || fieldSetting.CaseSolutionMode == CaseSolutionModes.ReadOnly;
            return isReadOnly;
        }

        public static bool IsRequired(this CaseInputViewModel model, GlobalEnums.TranslationCaseFields caseFieldName, CaseSolutionFields caseTemplateFieldName)
        {
            var isVisible = IsCaseFieldVisible(model, caseFieldName, caseTemplateFieldName);
            if (!isVisible)
                return false;

            var isRequired = model.caseFieldSettings.CaseFieldSettingRequiredCheck(caseFieldName.ToString(), model.IsCaseReopened) == 1;
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