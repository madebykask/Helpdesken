using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{

    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;

    using DH.Helpdesk.BusinessData.Enums.Users;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.ChidCase;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.CaseType;
    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.CaseSolution;
    using DH.Helpdesk.Web.Infrastructure.CaseOverview;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Web.Models.Case.ChildCase;

    using UserGroup = DH.Helpdesk.BusinessData.Enums.Admin.Users.UserGroup;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using System;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;
    using Models.Case;
    using System.ComponentModel;
    using System.Data;
    using System.Web;

    public static class HtmlHelperExtension
    {
        /// <summary>
        /// The default offset.
        /// </summary>
        private const int DefaultOffset = 20;

        #region MasterPage

        public static MasterPageViewModel MasterModel(this HtmlHelper helper)
        {
            return (MasterPageViewModel)helper.ViewData[Constants.ViewData.MasterViewData];
        }

        #endregion

        #region Special Checkbox

        public static MvcHtmlString CheckBoxWithOffValue(this HtmlHelper helper, string name, int value, int offValue, bool isChecked)
        {
            return CheckBoxWithOffValue(helper, name, value, offValue, isChecked, null);
        }

        public static MvcHtmlString CheckBoxWithOffValue(this HtmlHelper helper, string name, int value, int offValue, bool isChecked, object htmlAttributes)
        {
            // Create tag builder
            var builder = new TagBuilder("input");

            // Add attributes
            builder.MergeAttribute("name", name);

            if (isChecked)
                builder.MergeAttribute("checked", "checked");

            builder.MergeAttribute("value", value.ToString());
            builder.MergeAttribute("type", "checkbox");
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var hidden = new TagBuilder("input");

            // Add attributes
            hidden.MergeAttribute("name", name);

            hidden.MergeAttribute("value", offValue.ToString());
            hidden.MergeAttribute("type", "hidden");

            // Render tag
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing) + hidden.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SpecialCheckBox(this HtmlHelper helper, string id, int value, int defaultValue)
        {
            return SpecialCheckBox(helper, id, value, defaultValue, null);
        }

        public static MvcHtmlString SpecialCheckBox(this HtmlHelper helper, string id, int value, int defaultValue, object htmlAttributes)
        {
            // Create tag builder
            var builder = new TagBuilder("input");

            // Add attributes
            builder.MergeAttribute("name", id);

            if (value == defaultValue)
                builder.MergeAttribute("checked", "checked");

            builder.MergeAttribute("value", defaultValue.ToString());
            builder.MergeAttribute("type", "checkbox");
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            // Render tag
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString SpecialLabel(this HtmlHelper helper, string id, int value, string label)
        {
            // Create tag builder
            var builder = new TagBuilder("label");

            if (value == 1)
                builder.MergeAttribute("class", "cb-enable selected");
            else
                builder.MergeAttribute("class", "cb-enable");

            // Render tag
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.StartTag) + "<span>" + label + "</span></label>");
        }

        public static MvcHtmlString SpecialLabelOff(this HtmlHelper helper, string id, int value, string label)
        {
            // Create tag builder
            var builder = new TagBuilder("label");

            if (value == 0)
                builder.MergeAttribute("class", "cb-disable selected");
            else
                builder.MergeAttribute("class", "cb-disable");

            // Render tag
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.StartTag) + "<span>" + label + "</span></label>");
        }

        #endregion

        #region Active Tab

        private const string DefaultCssClass = "selected";

        public static string ActiveTab(this HtmlHelper helper, string activeController)
        {
            return helper.ActiveTab(new string[] { activeController }, DefaultCssClass);
        }

        public static string ActiveTab(this HtmlHelper helper, string[] activeControllers)
        {
            return helper.ActiveTab(activeControllers, DefaultCssClass);
        }

        public static string ActiveTab(this HtmlHelper helper, string[] activeControllers, string cssClass)
        {
            string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();

            return activeControllers.Select(x => x.ToUpper()).Contains(currentController.ToUpper()) ? cssClass : string.Empty;
        }

        public static string ActiveTab(this HtmlHelper helper, string[] activeControllers, string[] activeActions)
        {
            return helper.ActiveTab(activeControllers, activeActions, DefaultCssClass);
        }

        public static string ActiveTab(this HtmlHelper helper, string[] activeControllers, string[] activeActions, string cssClass)
        {
            string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();

            return (activeControllers.Select(x => x.ToUpper()).Contains(currentController.ToUpper())
                && activeActions.Select(x => x.ToUpper()).Contains(currentAction.ToUpper())) ? cssClass : string.Empty;
        }

        #endregion

        #region TreeString

        public static MvcHtmlString CaseTypeDropdownButtonString(this HtmlHelper helper, IList<CaseType> caseTypes, bool isTakeOnlyActive = true)
        {
            if (caseTypes != null)
            {
                return BuildCaseTypeDropdownButton(caseTypes, isTakeOnlyActive);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString ProductAreaDropdownButtonString(this HtmlHelper helper, IList<ProductArea> pal, bool isTakeOnlyActive = true, int? productAreaIdToInclude = null)
        {
            if (pal != null)
            {
                return BuildProcuctAreaDropdownButton(pal, isTakeOnlyActive, null, productAreaIdToInclude);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static string ProductAreaDropdownString(IList<ProductArea> pal, bool isTakeOnlyActive = true, int? productAreaIdToInclude = null)
        {
            return pal != null ? BuildProcuctAreaDropdownButtonString(pal, isTakeOnlyActive, null, productAreaIdToInclude) : string.Empty;
        }

        public static MvcHtmlString CategoryDropdownButtonString(this HtmlHelper helper, IList<Category> cats, bool isTakeOnlyActive = true)
        {
            if (cats != null)
            {
                return BuildCategoryDropdownButton(cats, isTakeOnlyActive);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString ProductAreasList(this HtmlHelper helper, IEnumerable<ProductAreaItem> productAreas)
        {
            return productAreas == null ? MvcHtmlString.Empty : BuildProductAreasList(productAreas);
        }

        public static MvcHtmlString CaseTypesList(this HtmlHelper helper, IEnumerable<CaseTypeItem> caseTypes)
        {
            return caseTypes == null ? MvcHtmlString.Empty : BuildCaseTypesList(caseTypes);
        }

        public static MvcHtmlString FinishingCauseDropdownButtonString(this HtmlHelper helper, IList<FinishingCause> causes, bool isTakeOnlyActive = true)
        {
            if (causes != null)
            {
                return BuildFinishingCauseDropdownButton(causes, isTakeOnlyActive);
            }
            else
                return new MvcHtmlString(string.Empty);
        }
         
        public static MvcHtmlString CaseTypeTreeString(this HtmlHelper helper, IList<CaseType> caseTypes)
        {
            if (caseTypes != null)
            {
                return BuildCaseTypeTreeRow(caseTypes, 0);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString FinishingCauseTreeString(this HtmlHelper helper, IList<FinishingCause> finishingCauses)
        {
            if (finishingCauses != null)
            {
                return BuildFinishingCauseTreeRow(finishingCauses, 0);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString OrderTypeTreeString(this HtmlHelper helper, IList<OrderType> orderTypes, bool isShowOnlyActive = false)
        {
            if (orderTypes != null)
            {
                return BuildOrderTypeTreeRow(orderTypes, 0, isShowOnlyActive);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString OrderTypeForMailTemplateTreeString(this HtmlHelper helper, IList<OrderType> orderTypes)
        {
            if (orderTypes != null)
            {
                return BuildOrderTypeForMailTemplateTreeRow(orderTypes, 0, "");
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString OUTreeString(this HtmlHelper helper, IList<OU> ous, 
                                                 bool isShowOnlyActive = true,
                                                 bool isParentInactive = false)
        {
            ous = ous.OrderBy(x => x.Department.DepartmentName).ToList();

            if (ous != null)
            {
                return BuildOUTreeRow(ous, 0, isShowOnlyActive, isParentInactive);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString ProductAreaTreeString(this HtmlHelper helper, IList<ProductArea> productAreas, bool isShowOnlyActive = false)
        {
            if (productAreas != null)
            {
                return BuildProductAreaTreeRow(productAreas, 0, isShowOnlyActive);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString CategoryTreeString(this HtmlHelper helper, IList<Category> categories, int customerId, bool isShowOnlyActive = false)
        {
            if (categories != null)
            {
                return BuildCategoryTreeRow(categories, 0, customerId, isShowOnlyActive);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        /// <summary>
        /// The causing parts tree.
        /// </summary>
        /// <param name="html">
        /// The html.
        /// </param>
        /// <param name="causingParts">
        /// The causing parts.
        /// </param>
        /// <returns>
        /// The <see cref="MvcHtmlString"/>.
        /// </returns>
        public static MvcHtmlString CausingPartsTree(
            this HtmlHelper html,
            IEnumerable<CausingPartOverview> causingParts,
            bool isShowOnlyActive = false)
        {
            if (causingParts == null)
            {
                return MvcHtmlString.Empty;
            }
            return CausingPartsTreeRow(causingParts, 0, isShowOnlyActive);
        }

        public static string RemoveHTMLTags(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return Regex.Replace(value, @"<[^>]*>", "<HTMLTAG>");
        }

        public static MvcHtmlString GetCaseHistoryInfo(
            this CaseHistoryOverview cur, 
            CaseHistoryOverview old, 
            int customerId, 
            int departmentFilterFormat, 
            IList<CaseFieldSetting> cfs,
            OutputFormatter outFormatter)
        {
            StringBuilder sb = new StringBuilder();
            const string bs = "<th>";
            const string be = "</th>";
            const string ey = "";
            const string from = " &rarr; ";
            const string tdOpenMarkup = "<td style=\"width:70%\">";
            const string tdCloseMarkup = "</td>";

            var o = (old != null ? old : new CaseHistoryOverview());

            // Reported by
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ReportedBy.ToString()).ShowOnStartPage == 1)
            {
                if (cur.ReportedBy != o.ReportedBy)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.ReportedBy.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(o.ReportedBy.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.ReportedBy.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Persons name
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Persons_Name.ToString()).ShowOnStartPage == 1)
            {
                if (cur.PersonsName != o.PersonsName)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Persons_Name.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(o.PersonsName.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.PersonsName.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Persons_phone 
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Persons_Phone.ToString()).ShowOnStartPage == 1)
            {
                if (cur.PersonsPhone != o.PersonsPhone)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Persons_Phone.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(o.PersonsPhone.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.PersonsPhone.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Department
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Department_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Department_Id != o.Department_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Department_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.Department != null)
                        sb.Append(o.Department.DepartmentDescription(departmentFilterFormat).RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Department != null)
                        sb.Append(cur.Department.DepartmentDescription(departmentFilterFormat).RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // UserCode
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.UserCode.ToString()).ShowOnStartPage == 1)
            {
                if (cur.UserCode != o.UserCode)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.UserCode.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(o.UserCode.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.UserCode.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);//
                    sb.Append("</tr>");
                }
            }

            // Registration Source
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString()).ShowOnStartPage == 1)
            {
                if (cur.RegistrationSourceCustomer_Id != o.RegistrationSourceCustomer_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.RegistrationSourceCustomer != null)
                        sb.Append(Translation.Get(o.RegistrationSourceCustomer.SourceName.RemoveHTMLTags(), Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.RegistrationSourceCustomer != null)
                        sb.Append(Translation.Get(cur.RegistrationSourceCustomer.SourceName.RemoveHTMLTags(), Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }

            // CaseType
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.CaseType_Id != o.CaseType_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    
                    if (o.CaseType != null)
                        sb.Append(Translation.Get(o.CaseType.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.CaseType != null)
                        sb.Append(Translation.Get(cur.CaseType.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // ProductArea
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.ProductArea_Id != o.ProductArea_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.ProductArea != null)
                        sb.Append(Translation.Get(o.ProductArea.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.ProductArea != null)
                        sb.Append(Translation.Get(cur.ProductArea.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Category
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Category_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Category_Id != o.Category_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Category_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.Category != null)
                        sb.Append(Translation.Get(o.Category.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Category != null)
                        sb.Append(Translation.Get(cur.Category.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // ReferenceNumber
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString()).ShowOnStartPage == 1)
            {
                if (cur.ReferenceNumber != o.ReferenceNumber)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(o.ReferenceNumber.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.ReferenceNumber.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);//
                    sb.Append("</tr>");
                }
            }
            // Caption
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Caption.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Caption != o.Caption)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Caption.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(o.Caption.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.Caption.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup); //
                    sb.Append("</tr>");
                }
            }
            // Responsible User
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.CaseResponsibleUser_Id != o.CaseResponsibleUser_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.UserResponsible != null)
                        sb.Append(o.UserResponsible.FirstName + " " + o.UserResponsible.SurName);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.UserResponsible != null)
                        sb.Append(cur.UserResponsible.FirstName + " " + cur.UserResponsible.SurName);
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Performer User
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Performer_User_Id != o.Performer_User_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.UserPerformer != null)
                        sb.Append(o.UserPerformer.FirstName + " " + o.UserPerformer.SurName);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.UserPerformer != null)
                        sb.Append(cur.UserPerformer.FirstName + " " + cur.UserPerformer.SurName);
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Priority
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Priority_Id != o.Priority_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Priority_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.Priority != null)
                        sb.Append(Translation.Get(o.Priority.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Priority != null)
                        sb.Append(Translation.Get(cur.Priority.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // WorkingGroup
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.WorkingGroup_Id != o.WorkingGroup_Id)
                {
                    //string value = cur.WorkingGroup != null ? cur.WorkingGroup.WorkingGroupName : ey + from + cur.WorkingGroup != null ? cur.WorkingGroup.WorkingGroupName : ey;
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.WorkingGroup != null)
                        sb.Append(Translation.Get(o.WorkingGroup.WorkingGroupName, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.WorkingGroup != null)
                        sb.Append(Translation.Get(cur.WorkingGroup.WorkingGroupName, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // StateSecondary
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.StateSecondary_Id != o.StateSecondary_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.StateSecondary != null)
                        sb.Append(Translation.Get(o.StateSecondary.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.StateSecondary != null)
                        sb.Append(Translation.Get(cur.StateSecondary.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Status
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Status_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Status_Id != o.Status_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Status_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.Status != null)
                        sb.Append(Translation.Get(o.Status.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Status != null)
                        sb.Append(Translation.Get(cur.Status.Name, Enums.TranslationSource.TextTranslation, customerId));
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // Watchdate
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.WatchDate.ToString()).ShowOnStartPage == 1)
            {
                if (cur.WatchDate != o.WatchDate)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.WatchDate.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    if (o.WatchDate == null)
                    {
                        sb.Append(tdOpenMarkup);
                        sb.Append(from);
                        sb.Append(outFormatter.FormatDate(cur.WatchDate.Value));
                    }
                    else
                    {
                        if (cur.WatchDate == null)
                        {
                            sb.Append(tdOpenMarkup);
                            sb.Append(outFormatter.FormatDate(o.WatchDate.Value));
                            sb.Append(from);
                            sb.Append("");
                        }
                        else
                        {
                            sb.Append(tdOpenMarkup);
                            sb.Append(outFormatter.FormatDate(o.WatchDate.Value));
                            sb.Append(from);
                            sb.Append(outFormatter.FormatDate(cur.WatchDate.Value));
                        }

                        sb.Append(tdCloseMarkup);
                        sb.Append("</tr>");
                    }
                }
            }
            // IsAbout user
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString()).ShowOnStartPage == 1)
            {
                if (cur.IsAbout_ReportedBy != o.IsAbout_ReportedBy)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(o.IsAbout_ReportedBy.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.IsAbout_ReportedBy.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            //IsAbout Persons name
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString()).ShowOnStartPage == 1)
            {
                if (cur.IsAbout_Persons_Name != o.IsAbout_Persons_Name)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(o.IsAbout_Persons_Name.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.IsAbout_Persons_Name.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // IsAbout Persons_phone 
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString()).ShowOnStartPage == 1)
            {
                if (cur.IsAbout_Persons_Phone != o.IsAbout_Persons_Phone)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(o.IsAbout_Persons_Phone.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.IsAbout_Persons_Phone.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // IsAbout Department
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.IsAbout_Department_Id != o.IsAbout_Department_Id)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.IsAbout_Department != null)
                        sb.Append(o.IsAbout_Department.DepartmentDescription(departmentFilterFormat).RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.IsAbout_Department != null)
                        sb.Append(cur.IsAbout_Department.DepartmentDescription(departmentFilterFormat).RemoveHTMLTags());
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }
            // IsAbout UserCode
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString()).ShowOnStartPage == 1)
            {
                if (cur.IsAbout_UserCode != o.IsAbout_UserCode)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(o.IsAbout_UserCode.RemoveHTMLTags());
                    sb.Append(from);
                    sb.Append(cur.IsAbout_UserCode.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);//
                    sb.Append("</tr>");
                }
            }
            // CaseFile
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Filename.ToString()).ShowOnStartPage == 1)
            {                 
                if (cur.CaseFile != o.CaseFile && !string.IsNullOrEmpty(cur.CaseFile))
                {
                    sb.Append("<tr>");
                    var caption = string.Empty;
                    if (!string.IsNullOrEmpty(cur.CaseFile))
                    {
                        if (cur.CaseFile.StartsWith(StringTags.Add))
                        {
                            sb.Append(bs + Translation.Get("Lägg till") + " " + Translation.Get(GlobalEnums.TranslationCaseFields.Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                            caption = cur.CaseFile.Substring(StringTags.Add.Length);
                        }
                        else
                            if (cur.CaseFile.StartsWith(StringTags.Delete))
                            {                                
                                sb.Append(bs + Translation.Get("Ta bort") + " " + Translation.Get(GlobalEnums.TranslationCaseFields.Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);                            
                                caption = cur.CaseFile.Substring(StringTags.Delete.Length);                                
                            }
                            else
                                caption = cur.CaseFile;
                    }
                    else
                        caption = ey;

                    sb.Append(tdOpenMarkup);
                    sb.Append(caption);
                    sb.Append(tdCloseMarkup);                                                      
                    sb.Append("</tr>");
                }
            }

            // LogFile
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString()).ShowOnStartPage == 1)
            {                              
                if (cur.LogFile != o.LogFile && !string.IsNullOrEmpty(cur.LogFile))             
                {
                    sb.Append("<tr>");
                    var caption = string.Empty;                    
                    if (!string.IsNullOrEmpty(cur.LogFile))
                    {                        
                        if (cur.LogFile.StartsWith(StringTags.Add))
                        {                            
                            sb.Append(bs + Translation.Get("Lägg till") + " " + Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);                            
                            caption = cur.LogFile.Substring(StringTags.Add.Length);                            
                        }
                        else
                            if (cur.LogFile.StartsWith(StringTags.Delete))
                            {                                                                
                                sb.Append(bs + Translation.Get("Ta bort") + " " + Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);                                

                                caption = cur.LogFile.Substring(StringTags.Delete.Length);                                
                            }
                            else
                            {                                 
                                sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);                                
                                caption = cur.LogFile;                                
                            }                        
                    }
                    else                    
                        caption = ey;

                    sb.Append(tdOpenMarkup); 
                    sb.Append(caption.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);                  
                    sb.Append("</tr>");
                }
            }


            // CaseLog
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString()).ShowOnStartPage == 1 ||
                cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal.ToString()).ShowOnStartPage == 1)
            {                
                if (cur.CaseLog != o.CaseLog && !string.IsNullOrEmpty(cur.CaseLog))
                {
                    var caption = string.Empty;
                    sb.Append("<tr>");
                    if (!string.IsNullOrEmpty(cur.CaseLog))
                    {
                        if (cur.CaseLog.StartsWith(StringTags.Add))
                        {
                            sb.Append(bs + Translation.Get("Lägg till") + " " + Translation.Get("Ärendelogg") + be);
                            caption = cur.CaseLog.Substring(StringTags.Add.Length)
                                                 .Replace(StringTags.ExternalLog,
                                                          Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ")
                                                 .Replace(StringTags.InternalLog, "<br />" +
                                                          Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ");
                        }
                        else                        
                            if (cur.CaseLog.StartsWith(StringTags.Delete))
                            {
                                sb.Append(bs + Translation.Get("Ta bort") + " " + Translation.Get("Ärendelogg") + be);
                                caption = cur.CaseLog.Substring(StringTags.Delete.Length)
                                                     .Replace(StringTags.ExternalLog,
                                                              Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ")
                                                     .Replace(StringTags.InternalLog, "<br />" +
                                                              Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ")
                                                     .Replace(StringTags.LogFile, "<br />" +
                                                              Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": <br /> &nbsp; - ")
                                                     .Replace(StringTags.Seperator, "<br /> &nbsp; - ");
                            }
                            else
                            {
                                sb.Append(bs + Translation.Get("Ärendelogg") + be);
                                caption = cur.CaseLog.Replace(StringTags.ExternalLog,
                                                              Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ")
                                                     .Replace(StringTags.InternalLog, "<br />" +
                                                              Translation.Get(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + ": ");
                            }                        
                    }
                    else
                        caption = ey;

                    sb.Append(tdOpenMarkup);
                    sb.Append(caption.RemoveHTMLTags());
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }

            // Closing Reason
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ClosingReason.ToString()).ShowOnStartPage == 1)
            {
                if (cur.ClosingReason != o.ClosingReason && cur.ClosingReason != null)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.ClosingReason.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append(tdOpenMarkup);
                    if (o.ClosingReason != null)
                        sb.Append(o.ClosingReason);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.ClosingReason != null)
                        sb.Append(cur.ClosingReason);
                    else
                        sb.Append(ey);
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }

            // Closing Date
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.FinishingDate.ToString()).ShowOnStartPage == 1)
            {
                if (cur.FinishingDate != o.FinishingDate)
                {
                        sb.Append("<tr>");
                        if (o.FinishingDate == null)
                        {
                            sb.Append(bs + Translation.Get("Ärendet avslutat") + be);
                            sb.Append(tdOpenMarkup);
                            sb.Append(from);
                            sb.Append(outFormatter.FormatDate(cur.FinishingDate.Value));
                        }
                        else
                        {
                            if (cur.FinishingDate == null)
                            {
                                sb.Append(bs + Translation.Get("Ärendet aktiverat") + be);
                                sb.Append(tdOpenMarkup);                                
                                sb.Append(ey);                                
                            }
                            else
                            {
                                sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.FinishingDate.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                                sb.Append(tdOpenMarkup);
                                sb.Append(outFormatter.FormatDate(o.FinishingDate.Value));
                                sb.Append(from);
                                sb.Append(outFormatter.FormatDate(cur.FinishingDate.Value));
                            }
                        }

                        sb.Append(tdCloseMarkup);
                        sb.Append("</tr>");
                }
            }

            // Case extra followers
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString()).ShowOnStartPage == 1)
            {
                if (!cur.CaseExtraFollowers.Equals(o.CaseExtraFollowers))
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.GetCoreTextTranslation("Följare") + be);
                    sb.Append(tdOpenMarkup);
                    sb.Append(o.CaseExtraFollowers.Replace(BRConstItem.Email_Separator, BRConstItem.Email_Separator + " "));
                    sb.Append(from);
                    sb.Append(cur.CaseExtraFollowers.Replace(BRConstItem.Email_Separator, BRConstItem.Email_Separator + " "));
                    sb.Append(tdCloseMarkup);
                    sb.Append("</tr>");
                }
            }

            return new MvcHtmlString(sb.ToString());
        }


        public static MvcHtmlString CaseSolutionDropdownButtonString(this HtmlHelper helper, IList<CaseTemplateCategoryNode> categories, int customerId, string isJS = "")
        {
            if (categories != null)
            {
                if (isJS == "")
                    return BuildCaseSolutionCategoryDropdownButton(categories, customerId);
                else
                   return BuildCaseSolutionCategoryJSDropdownButton(categories, customerId);                   
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString JsonToHtmlTable<T>(this string jsonText, string classType = "")
        {
            //var jsonData = jsonText.Replace("\"", "'");
            var dataList = JsonConvert.DeserializeObject<List<T>>(jsonText);
            var dt = dataList.ToDataTable();
            return dt.ConvertDataTableToHTML(classType);
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
            TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static MvcHtmlString ConvertDataTableToHTML(this DataTable dt, string classType = "")
        {
            var html = $"<table border='1' class='{classType}'>";            
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += (i == 0 ? "<th>" : "<td>") + dt.Rows[i][j].ToString() + (i == 0 ? "</th>" : "</td>");
                html += "</tr>";
            }
            html += "</table>";
            return new MvcHtmlString(html);
        }

        private static MvcHtmlString BuildCaseTypeTreeRow(IList<CaseType> caseTypes, 
            int iteration, 
            bool isShowOnlyActive = true,
            bool isParentInactive = false)
        {
            string htmlOutput = string.Empty;
            var caseTypeToDisplay = isShowOnlyActive ? caseTypes.Where(it => it.IsActive == 1) : caseTypes;

            foreach (CaseType caseType in caseTypes)
            {
                var isInactive = caseType.IsActive != 1 || isParentInactive;
                var admin = string.Empty;
                if (caseType.Administrator != null)
                    admin = string.Format("{0} {1}", caseType.Administrator.FirstName, caseType.Administrator.SurName);
                //htmlOutput += "<tr>";
                htmlOutput += string.Format("<tr class=\"{0}\">", isInactive ? "inactive" : string.Empty);
                htmlOutput += "<td><a href='/admin/casetype/edit/" + caseType.Id + "' style='padding-left: " + iteration + "px'><i class='icon-resize-full icon-dh'></i>" + caseType.Name + "</a></td>";
                htmlOutput += "<td><a href='/admin/casetype/edit/" + caseType.Id + "'>" + caseType.IsDefault.TranslateBit() + "</a></td>";
                htmlOutput += "<td><a href='/admin/casetype/edit/" + caseType.Id + "'>" + caseType.RequireApproving.TranslateBit() + "</a></td>";
                htmlOutput += "<td><a href='/admin/casetype/edit/" + caseType.Id + "'>" + admin + "</a></td>";
                htmlOutput += "<td><a href='/admin/casetype/edit/" + caseType.Id + "'>" + caseType.IsActive.TranslateBit() + "</a></td>";
                htmlOutput += "</tr>";

                if (caseType.SubCaseTypes != null)
                {
                    if (caseType.SubCaseTypes.Count > 0 && (
                        (isShowOnlyActive && !isInactive)
                        || !isShowOnlyActive))
                    {
                        htmlOutput += BuildCaseTypeTreeRow(
                            caseType.SubCaseTypes.OrderBy(x => x.Name).ToList(),
                            iteration + 20,
                            isShowOnlyActive,
                            isInactive);
                    }
                }

                //if (caseType.SubCaseTypes != null)
                //    if (caseType.SubCaseTypes.Count > 0)
                //        htmlOutput += BuildCaseTypeTreeRow(caseType.SubCaseTypes.ToList(), iteration + 20);
            }

            return new MvcHtmlString(htmlOutput);
        }

        private static MvcHtmlString BuildCaseTypeDropdownButton(IList<CaseType> caseTypes, bool isTakeOnlyActive = true)
        {
            var res = new StringBuilder();

            foreach (CaseType item in caseTypes)
            {
                var childs = new List<CaseType>();
                if (item.SubCaseTypes != null)
                {
                    childs = isTakeOnlyActive
                                    ? item.SubCaseTypes.Where(p => p.IsActive != 0 && p.Selectable != 0).ToList()
                                    : item.SubCaseTypes.ToList();
                }

                var cls = item.IsActive == 1 ? string.Empty : "inactive";

                if (childs.Count > 0)
                {
                    res.Append("<li class='dropdown-submenu " + cls + "'>");
                }
                else
                {
                    res.Append("<li class='" + cls + "'>");
                }

                res.AppendFormat("<a href='#' value='{0}'>{1}</a>", item.Id.ToString(), Translation.GetMasterDataTranslation(item.Name));
                if (childs.Count > 0)
                {
                    res.Append("<ul class='dropdown-menu'>");
                    res.Append(BuildCaseTypeDropdownButton(childs.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList(), isTakeOnlyActive));
                    res.Append("</ul>");
                }

                res.Append("</li>");
           }

            return new MvcHtmlString(res.ToString());
        }

        private static MvcHtmlString BuildFinishingCauseDropdownButton(IList<FinishingCause> causes, bool isTakeOnlyActive = true)
        {
            StringBuilder sb = new StringBuilder();

            foreach (FinishingCause f in causes)
            {
                if (!isTakeOnlyActive || (isTakeOnlyActive && f.IsActive != 0))
                {
                    bool hasChild = false;
                    if (f.SubFinishingCauses != null)
                        if (f.SubFinishingCauses.Count > 0 && 
                            (!isTakeOnlyActive || (isTakeOnlyActive && 
                                                   f.SubFinishingCauses.Any(sc=> sc.IsActive != 0))))
                            hasChild = true;

                    var cls = f.IsActive == 1 ? string.Empty : "inactive";

                    if (hasChild)
                        sb.Append("<li class='dropdown-submenu " + cls + "'>");
                    else
                        sb.Append("<li class='" + cls + "'>");

                    sb.Append("<a href='#' value=" + f.Id.ToString() + ">" + Translation.GetMasterDataTranslation(f.Name) + "</a>");
                    if (hasChild)
                    {
                        sb.Append("<ul class='dropdown-menu'>");
                        sb.Append(BuildFinishingCauseDropdownButton(f.SubFinishingCauses.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList(), isTakeOnlyActive));
                        sb.Append("</ul>");
                    }
                    sb.Append("</li>");
                }
            }

            return new MvcHtmlString(sb.ToString());
        }

        private static MvcHtmlString BuildProductAreasList(IEnumerable<ProductAreaItem> productAreas)
        {
            var result = new StringBuilder();

            foreach (var productArea in productAreas)
            {
                var hasChild = productArea.Children != null && productArea.Children.Any();

                result.Append(hasChild ? "<li class='dropdown-submenu'>" : "<li>");
                result.AppendFormat("<a href='#' value='{0}'>{1}</a>", productArea.Id, Translation.GetMasterDataTranslation(productArea.Name));
                if (hasChild)
                {
                    result.Append("<ul class='dropdown-menu'>");
                    result.Append(BuildProductAreasList(productArea.Children.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList()));
                    result.Append("</ul>");
                }

                result.Append("</li>");
            }

            return MvcHtmlString.Create(result.ToString());
        }

        private static MvcHtmlString BuildCaseTypesList(IEnumerable<CaseTypeItem> caseTypes)
        {
            var result = new StringBuilder();

            foreach (var caseType in caseTypes)
            {
                var hasChild = caseType.Children != null && caseType.Children.Any();

                result.Append(hasChild ? "<li class='dropdown-submenu'>" : "<li>");

                result.Append("<a href='#' value=" + caseType.Id + ">" + Translation.GetMasterDataTranslation(caseType.Name) + "</a>");
                if (hasChild)
                {
                    result.Append("<ul class='dropdown-menu'>");
                    result.Append(BuildCaseTypesList(caseType.Children.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList()));
                    result.Append("</ul>");
                }

                result.Append("</li>");
            }

            return MvcHtmlString.Create(result.ToString());
        }
        
        private static MvcHtmlString BuildProcuctAreaDropdownButton(
            IList<ProductArea> pal,            
            bool isTakeOnlyActive = true,
            Dictionary<int, bool> userGroupDictionary = null,
            int? productAreaIdToInclude = null)
        {
            var pas = BuildProcuctAreaDropdownButtonString(pal, isTakeOnlyActive, userGroupDictionary, productAreaIdToInclude);
            return new MvcHtmlString(pas);
        }

        private static string BuildProcuctAreaDropdownButtonString(
            IList<ProductArea> pal,
            bool isTakeOnlyActive = true,
            Dictionary<int, bool> userGroupDictionary = null,
            int? productAreaIdToInclude = null)
        {
            string htmlOutput = string.Empty;
            var user = SessionFacade.CurrentUser;

            if (userGroupDictionary == null)
            {
                userGroupDictionary = user.UserWorkingGroups.Where(it => it.UserRole == WorkingGroupUserPermission.ADMINSTRATOR).ToDictionary(it => it.WorkingGroup_Id, it => true);
            }

            foreach (ProductArea pa in pal)
            {
                List<ProductArea> childList = null;
                if (pa.SubProductAreas != null)
                {
                    var childs = isTakeOnlyActive
                                 ? pa.SubProductAreas.Where(p => p.IsActive != 0)
                                 : pa.SubProductAreas;

                    if (user.UserGroupId < (int)UserGroup.CustomerAdministrator)
                    {
                        childs =
                            childs.Where(
                                it =>
                                it.WorkingGroups.Count == 0
                                || it.WorkingGroups.Any(
                                    productAreaWorkingGroup =>
                                    userGroupDictionary.ContainsKey(productAreaWorkingGroup.Id))
                                || (productAreaIdToInclude.HasValue && it.Id == productAreaIdToInclude.Value));
                    }

                    childList = childs.ToList();
                }

                var cls = pa.IsActive == 1 ? string.Empty : "inactive";
                if (childList != null && childList.Count > 0)
                {
                    htmlOutput += string.Format("<li class=\"dropdown-submenu {0} {1}\" id=\"{2}\">", cls, "DynamicDropDown_Up", pa.Id);
                }
                else
                {
                    htmlOutput += string.Format("<li class=\"{0} \" >", cls);
                }

                htmlOutput +=
                    string.Format(
                        "<a href='#' value=\"{0}\">{1}</a>",
                        pa.Id,
                        Translation.GetMasterDataTranslation(pa.Name));

                if (childList != null && childList.Count > 0)
                {
                    htmlOutput += string.Format("<ul class='dropdown-menu' id=\"subDropDownMenu_{0}\" >", pa.Id);
                    htmlOutput += BuildProcuctAreaDropdownButtonString(childList.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList(), isTakeOnlyActive, userGroupDictionary);
                    htmlOutput += "</ul>";
                }

                htmlOutput += "</li>";
            }
            return htmlOutput;
        }

        private static MvcHtmlString BuildCategoryDropdownButton(
            IList<Category> cats,
            bool isTakeOnlyActive = true)
        {
            string htmlOutput = string.Empty;

            foreach (Category ca in cats)
            {
                List<Category> childList = null;
                if (ca.SubCategories != null)
                {
                    var childs = isTakeOnlyActive
                                 ? ca.SubCategories.Where(p => p.IsActive != 0)
                                 : ca.SubCategories;

                    childList = childs.ToList();
                }

                var cls = ca.IsActive == 1 ? string.Empty : "inactive";
                if (childList != null && childList.Count > 0)
                {
                    htmlOutput += string.Format("<li class=\"dropdown-submenu {0} {1}\" id=\"{2}\">", cls, "DynamicDropDown_Up", ca.Id);
                }
                else
                {
                    htmlOutput += string.Format("<li class=\"{0} \" >", cls);
                }

                htmlOutput +=
                    string.Format(
                        "<a href='#' value=\"{0}\">{1}</a>",
                        ca.Id,
                        Translation.GetMasterDataTranslation(ca.Name));

                if (childList != null && childList.Count > 0)
                {
                    htmlOutput += string.Format("<ul class='dropdown-menu' id=\"subDropDownMenu_{0}\" >", ca.Id);
                    htmlOutput += BuildCategoryDropdownButton(childList.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList(), isTakeOnlyActive);
                    htmlOutput += "</ul>";
                }

                htmlOutput += "</li>";
            }

            return new MvcHtmlString(htmlOutput);
        }

        private static MvcHtmlString BuildFinishingCauseTreeRow(IList<FinishingCause> finishingCauses, int iteration, 
                bool isShowOnlyActive = true,
                bool isParentInactive = false)
        {
            string htmlOutput = string.Empty;
            var finishingCauseToDisplay = isShowOnlyActive ? finishingCauses.Where(it => it.IsActive == 1) : finishingCauses;

            foreach (FinishingCause finishingCause in finishingCauses)
            {
                var isInactive = finishingCause.IsActive != 1 || isParentInactive;
                //htmlOutput += "<tr>";
                htmlOutput += string.Format("<tr class=\"{0}\">", isInactive ? "inactive" : string.Empty);
                htmlOutput += "<td><a href='/admin/finishingcause/edit/" + finishingCause.Id + "' style='padding-left: " + iteration + "px'><i class='icon-resize-full icon-dh'></i>" + finishingCause.Name + "</a></td>";
                htmlOutput += "<td><a href='/admin/finishingcause/edit/" + finishingCause.Id + "'>" + finishingCause.IsActive.TranslateBit() + "</a></td>";
                htmlOutput += "</tr>";


                if (finishingCause.SubFinishingCauses != null)
                {
                    if (finishingCause.SubFinishingCauses.Count > 0 && (
                        (isShowOnlyActive && !isInactive)
                        || !isShowOnlyActive))
                    {
                        htmlOutput += BuildFinishingCauseTreeRow(
                            finishingCause.SubFinishingCauses.OrderBy(x => x.Name).ToList(),
                            iteration + 20,
                            isShowOnlyActive,
                            isInactive);
                    }
                }

                //if (finishingCause.SubFinishingCauses != null)
                //    if (finishingCause.SubFinishingCauses.Count > 0)
                //        htmlOutput += BuildFinishingCauseTreeRow(finishingCause.SubFinishingCauses.ToList(), iteration + 20);
            }

            return new MvcHtmlString(htmlOutput);
        }

        private static MvcHtmlString BuildOrderTypeTreeRow(IList<OrderType> orderTypes, int iteration, bool isShowOnlyActive = true, bool isParentInactive = false)
        {
            string htmlOutput = string.Empty;

            var orderTypesToDisplay = isShowOnlyActive ? orderTypes.Where(it => it.IsActive == 1) : orderTypes;

            foreach (OrderType orderType in orderTypesToDisplay)
            {
                var isInactive = orderType.IsActive != 1 || isParentInactive;
                htmlOutput += string.Format("<tr class=\"{0}\">", isInactive ? "inactive" : string.Empty);
                htmlOutput += "<td><a href='/admin/ordertype/edit/" + orderType.Id + "' style='padding-left: " + iteration + "px'><i class='icon-resize-full icon-dh'></i>" + orderType.Name + "</a></td>";
                htmlOutput += "<td><a href='/admin/ordertype/edit/" + orderType.Id + "'>" + orderType.IsDefault.TranslateBit() + "</a></td>";

                if (orderType.Document != null)
                    htmlOutput += "<td><a href='/admin/ordertype/edit/" + orderType.Id + "'>" + orderType.Document.Name + "</a></td>";
                else
                    htmlOutput += "<td></td>";
                htmlOutput += "<td><a href='/admin/finishingcause/edit/" + orderType.Id + "'>" + orderType.IsActive.TranslateBit() + "</a></td>";
                htmlOutput += "</tr>";

                if (orderType.SubOrderTypes != null)
                    if (orderType.SubOrderTypes.Count > 0)
                        htmlOutput += BuildOrderTypeTreeRow(orderType.SubOrderTypes.OrderBy(x => x.Name).ToList(), iteration + 20, isShowOnlyActive, isInactive);

            }

            return new MvcHtmlString(htmlOutput);
        }

        private static MvcHtmlString BuildOrderTypeForMailTemplateTreeRow(IList<OrderType> orderTypes, int iteration, string label)
        {
            string htmlOutput = string.Empty;
            string name = string.Empty;

            foreach (OrderType orderType in orderTypes)
            {
                name = label != string.Empty ? label + " - " + orderType.Name : orderType.Name;
                htmlOutput += "<tr>";
                htmlOutput += "<td>" + @Translation.Get("E-post mall: Beställning - ", Enums.TranslationSource.TextTranslation) + "</td>";
                htmlOutput += "<td><a href='/admin/mailtemplate/edit/" + orderType.Id + "' ";
                htmlOutput += "style='padding-left: " + iteration + "px'>" + name + "</a></td><br />";
                htmlOutput += "</tr>";

                if (orderType.SubOrderTypes != null)
                    if (orderType.SubOrderTypes.Count > 0)
                        htmlOutput += BuildOrderTypeForMailTemplateTreeRow(orderType.SubOrderTypes.ToList(), iteration + 0, name);
            }

            return new MvcHtmlString(htmlOutput);
        }

        private static MvcHtmlString BuildOUTreeRow(IList<OU> ous, int iteration, bool isShowOnlyActive = true,
            bool isParentInactive = false)
        {
            string htmlOutput = string.Empty;
            var oUToDisplay = isShowOnlyActive ? ous.Where(it => it.IsActive == 1) : ous;


            foreach (OU ou in ous)
            {
                var isInactive = ou.IsActive != 1 || isParentInactive;

                htmlOutput += string.Format("<tr class=\"{0}\">", isInactive ? "inactive" : string.Empty);
                htmlOutput += "<td><a href='/admin/ou/edit/" + ou.Id + "?customerId=" + ou.Department.Customer_Id + "'>" + ou.Department.DepartmentName + "</a></td>";
                htmlOutput += "<td><a href='/admin/ou/edit/" + ou.Id + "?customerId=" + ou.Department.Customer_Id + "' style='padding-left: " + iteration + "px'><i class='icon-resize-full icon-dh'></i>" + ou.OUId + " (" + ou.Name + ")</a></td>";
                htmlOutput += "<td><a href='/admin/ou/edit/" + ou.Id + "?customerId=" + ou.Department.Customer_Id + "'>" + ou.IsActive.TranslateBit() + "</a></td>";
                htmlOutput += "</tr>";              

                if (ou.SubOUs != null)
                {
                    if (ou.SubOUs.Count() > 0 && (
                        (isShowOnlyActive && !isInactive)
                        || !isShowOnlyActive))
                    {
                        htmlOutput += (BuildOUTreeRow(ou.SubOUs.ToList(), iteration + DefaultOffset, isShowOnlyActive, isInactive));
                    }
                }

            }

            return new MvcHtmlString(htmlOutput);
        }

        private static string getCaseTypeParentPath(this CaseType o, string separator = " - ")
        {
            string ret = string.Empty;

            if (o.ParentCaseType == null)
                ret += o.Name;
            else
                ret += getCaseTypeParentPath(o.ParentCaseType, separator) + separator + o.Name;

            return ret;
        }

        private static MvcHtmlString BuildProductAreaTreeRow(
            IList<ProductArea> productAreas,
            int iteration,
            bool isShowOnlyActive = true,
            bool isParentInactive = false)
        {
            string htmlOutput = string.Empty;
            var productAreaToDisplay = isShowOnlyActive ? productAreas.Where(it => it.IsActive == 1) : productAreas;

            foreach (ProductArea productArea in productAreaToDisplay)
            {
                var wgName = productArea.WorkingGroup !=null ? productArea.WorkingGroup.WorkingGroupName : string.Empty;
                var prioName = productArea.Priority != null ? productArea.Priority.Name : string.Empty;
                var caseType = productArea.CaseTypeProductAreas.FirstOrDefault();
                var caseTypeName = string.Empty;
                if (caseType != null)
                    caseTypeName = getCaseTypeParentPath(caseType.CaseType);

                var isInactive = productArea.IsActive != 1 || isParentInactive;
                htmlOutput += string.Format("<tr class=\"{0}\">", isInactive ? "inactive" : string.Empty);
                htmlOutput += "<td><a href='/admin/productarea/edit/" + productArea.Id + "' style='padding-left: " + iteration + "px'><i class='icon-resize-full icon-dh'></i>" + productArea.Name + "</a></td>";
                htmlOutput += "<td><a href='/admin/productarea/edit/" + productArea.Id + "'>" + wgName + "</a></td>";
                htmlOutput += "<td><a href='/admin/productarea/edit/" + productArea.Id + "'>" + prioName + "</a></td>";
                htmlOutput += "<td><a href='/admin/productarea/edit/" + productArea.Id + "'>" + caseTypeName + "</a></td>";
                htmlOutput += "<td><a href='/admin/productarea/edit/" + productArea.Id + "'>" + productArea.IsActive.TranslateBit() + "</a></td>";
                htmlOutput += "</tr>";

                if (productArea.SubProductAreas != null)
                {
                    if (productArea.SubProductAreas.Count > 0 && (
                        (isShowOnlyActive && !isInactive) 
                        || !isShowOnlyActive))
                    {
                        htmlOutput += BuildProductAreaTreeRow(
                            productArea.SubProductAreas.OrderBy(x => x.Name).ToList(),
                            iteration + 20,
                            isShowOnlyActive,
                            isInactive);
                    }
                }
            }

            return new MvcHtmlString(htmlOutput);
        }

        private static MvcHtmlString BuildCategoryTreeRow(
            IList<Category> categories,
            int iteration,
            int customerId,
            bool isShowOnlyActive = true,
            bool isParentInactive = false)
        {
            string htmlOutput = string.Empty;
            var categoriesToDisplay = isShowOnlyActive ? categories.Where(it => it.IsActive == 1) : categories;

            foreach (Category category in categoriesToDisplay)
            {
                var isInactive = category.IsActive != 1 || isParentInactive;
                htmlOutput += string.Format("<tr class=\"{0}\">", isInactive ? "inactive" : string.Empty);
                htmlOutput += "<td><a href='/admin/category/edit/" + category.Id + "?customerId=" + customerId + "' style='padding-left: " + iteration + "px'><i class='icon-resize-full icon-dh'></i>" + category.Name + "</a></td>";
                htmlOutput += "<td><a href='/admin/category/edit/" + category.Id + "?customerId=" + customerId + "'>" + category.IsActive.TranslateBit() + "</a></td>";
                htmlOutput += "</tr>";

                if (category.SubCategories != null)
                {
                    if (category.SubCategories.Count > 0 && (
                        (isShowOnlyActive && !isInactive)
                        || !isShowOnlyActive))
                    {
                        htmlOutput += BuildCategoryTreeRow(
                            category.SubCategories.OrderBy(x => x.Name).ToList(),
                            iteration + 20,
                            customerId,
                            isShowOnlyActive,
                            isInactive);
                    }
                }
            }

            return new MvcHtmlString(htmlOutput);
        }

        /// <summary>
        /// The causing parts tree row.
        /// </summary>
        /// <param name="causingParts">
        /// The causing parts.
        /// </param>
        /// <param name="iteration">
        /// The iteration.
        /// </param>
        /// <returns>
        /// The <see cref="MvcHtmlString"/>.
        /// </returns>
        private static MvcHtmlString CausingPartsTreeRow(IEnumerable<CausingPartOverview> causingParts, int iteration, bool isShowOnlyActive = true,
            bool isParentInactive = false)
        {
            var result = new StringBuilder();

            var causingPartToDisplay = isShowOnlyActive ? causingParts.Where(it => it.IsActive == true) : causingParts;

            foreach (var causingPart in causingParts)
            {

                var isInactive = causingPart.IsActive != true || isParentInactive;

                result.AppendFormat("<tr class=\"{0}\">", isInactive ? "inactive" : string.Empty);
                result.AppendFormat("<td><a href='/admin/causingpart/edit/{0}' style='padding-left: {1}px'><i class='icon-resize-full icon-dh'></i>{2}</a></td>", causingPart.Id, iteration, causingPart.Name);
                result.AppendFormat("<td><a href='/admin/causingpart/edit/{0}'>{1}</a></td>", causingPart.Id, causingPart.IsActive.BoolToYesNo());
                result.Append("</tr>");

                //if (causingPart.Children != null)
                //{
                //    result.Append(CausingPartsTreeRow(causingPart.Children.ToList(), iteration + DefaultOffset));                    
                //}


                if (causingPart.Children != null)
                {
                    if (causingPart.Children.Count() > 0 && (
                        (isShowOnlyActive && !isInactive)
                        || !isShowOnlyActive))
                    {
                        result.Append(CausingPartsTreeRow(causingPart.Children.ToList(), iteration + DefaultOffset,isShowOnlyActive,isInactive)); 
                    }
                }

            }
            return new MvcHtmlString(result.ToString());
        }

        private static MvcHtmlString BuildCaseSolutionCategoryJSDropdownButton(IList<CaseTemplateCategoryNode> categories, int customerId)
        {
            StringBuilder sb = new StringBuilder();

            foreach (CaseTemplateCategoryNode f in categories)
            {

                bool hasChild = false;
                if (f.CaseTemplates != null)
                    if (f.CaseTemplates.Count > 0)
                        hasChild = true;

                if (hasChild)
                    sb.Append("<li class='dropdown-submenu'>");
                else
                    sb.Append("<li>");

                if (f.IsRootTemplate)
                    sb.Append("<a href='#' onclick = 'LoadTemplate(" + f.CategoryId.ToString()  + ")'" + customerId.ToString() + "&templateId=" + f.CategoryId.ToString() + "&templateistrue=1" + "' value=" + f.CategoryId.ToString() + ">" +
                           Translation.Get(f.CategoryName, Enums.TranslationSource.TextTranslation) + "</a>");
                else
                    sb.Append("<a href='#' class='category' value=" + f.CategoryId.ToString() + ">" +
                            Translation.Get(f.CategoryName, Enums.TranslationSource.TextTranslation) + "</a>");

                if (hasChild)
                {
                    sb.Append("<ul class='dropdown-menu'>");
                    sb.Append(BuildCaseSolutionJSDropdownButton(f.CaseTemplates.ToList(), customerId));
                    sb.Append("</ul>");
                }
                sb.Append("</li>");

            }

            return new MvcHtmlString(sb.ToString());
        }
        private static MvcHtmlString BuildCaseSolutionCategoryDropdownButton(IList<CaseTemplateCategoryNode> categories, int customerId)
        {
            StringBuilder sb = new StringBuilder();

            foreach (CaseTemplateCategoryNode f in categories)
            {

                bool hasChild = false;
                if (f.CaseTemplates != null)
                    if (f.CaseTemplates.Count > 0)
                        hasChild = true;

                if (hasChild)
                    sb.Append("<li class='dropdown-submenu'>");
                else
                    sb.Append("<li>");

                if (f.IsRootTemplate)
                {
                    sb.AppendFormat(
                        "<a href='cases/new/?customerId={0}&templateId={1}&templateistrue=1' value={1}>{2}</a>",
                        customerId,
                        f.CategoryId,
                        Translation.Get(f.CategoryName));
                }
                else
                {
                    // separator
                    if (f.CategoryId == 0)
                    {
                        sb.AppendFormat("<li class='divider'></li>");
                    }
                    else
                    {
                        sb.Append(
                            "<a href='#' class='category' value=" + f.CategoryId.ToString() + ">"
                            + Translation.Get(f.CategoryName, Enums.TranslationSource.TextTranslation) + "</a>");
                    }
                }

                if (hasChild)
                {
                    sb.Append("<ul class='dropdown-menu'>");
                    sb.Append(BuildCaseSolutionDropdownButton(f.CaseTemplates.ToList(), customerId));
                    sb.Append("</ul>");
                }

                sb.Append("</li>");
            }

            return new MvcHtmlString(sb.ToString());
        }

        private static MvcHtmlString BuildCaseSolutionDropdownButton(IList<CaseTemplateNode> caseTemplate, int customerId)
        {
            StringBuilder sb = new StringBuilder();

            foreach (CaseTemplateNode f in caseTemplate)
            {
                sb.Append("<li>");
                sb.Append("<a href='cases/new/?customerId=" + customerId.ToString() + "&templateId=" + f.CaseTemplateId.ToString() + "&templateistrue=1" + "' value=" + f.CaseTemplateId.ToString() + ">" +
                          Translation.Get(f.CaseTemplateName, Enums.TranslationSource.TextTranslation) + "</a>");                
                sb.Append("</li>");
            }

            return new MvcHtmlString(sb.ToString());
        }

        private static MvcHtmlString BuildCaseSolutionJSDropdownButton(IList<CaseTemplateNode> caseTemplate, int customerId)
        {
            StringBuilder sb = new StringBuilder();

            foreach (CaseTemplateNode f in caseTemplate)
            {
                sb.Append("<li>");
                sb.Append("<a href='#' onclick = 'LoadTemplate(" + f.CaseTemplateId.ToString() + ")' value=" + f.CaseTemplateId.ToString() + ">" +
                          Translation.Get(f.CaseTemplateName, Enums.TranslationSource.TextTranslation) + "</a>");
                sb.Append("</li>");
            }

            return new MvcHtmlString(sb.ToString());
        }

        #endregion

        public static MvcHtmlString GetSortIcon(this HtmlHelper helper, UserSort sorting)
        {
            if (sorting.IsAsc)
            {
                return new MvcHtmlString("<i class=\"icon-chevron-up\"></i>");
            }
            return new MvcHtmlString("<i class=\"icon-chevron-down\"></i>");
        }

        public static MvcHtmlString ImgForCase(this HtmlHelper helper, ChildCaseOverview ci)
        {
            var icoCode = GetCaseIcon(ci);
            var icoTitle = GetCaseIconTitle(icoCode);
            var content = string.Format("<img title='{0}' alt='{0}' src='/Content/icons/{1}' />", icoTitle, GetCaseIconSrc(icoCode));

            return new MvcHtmlString(content);
        }

        private static GlobalEnums.CaseIcon GetCaseIcon(ChildCaseOverview ci)
        {
            var ret = GlobalEnums.CaseIcon.Normal;
            // TODO Hantera icon för urgent
            if (ci.ClosingDate != null)
                if (ci.IsRequriedToApprive && ci.ApprovedDate == null)
                    ret = GlobalEnums.CaseIcon.FinishedNotApproved;
                else
                    ret = GlobalEnums.CaseIcon.Finished;

            return ret;
        }

        private static string GetCaseIconSrc(GlobalEnums.CaseIcon value)
        {
            var ret = "case.png";
        
            if (value == GlobalEnums.CaseIcon.Finished)
                ret = "case_close.png";
            else if (value == GlobalEnums.CaseIcon.FinishedNotApproved)
                ret = "case_close_notapproved.png";
            else if (value == GlobalEnums.CaseIcon.Urgent)
                ret = "case_Log_urgent.png";
        
            return ret;
        }

        private static string GetCaseIconTitle(GlobalEnums.CaseIcon value)
        {
            string ret;

            if (value == GlobalEnums.CaseIcon.Finished)
                ret = Translation.Get("Avslutat ärende", Enums.TranslationSource.TextTranslation);
            else if (value == GlobalEnums.CaseIcon.FinishedNotApproved)
                ret = Translation.Get("Åtgärdat ärende, ej godkänt", Enums.TranslationSource.TextTranslation);
            else if (value == GlobalEnums.CaseIcon.Urgent)
                ret = Translation.Get("Ärende", Enums.TranslationSource.TextTranslation) + " (" + Translation.Get("akut", Enums.TranslationSource.TextTranslation) + ")";
            else
                ret = Translation.Get("Ärende", Enums.TranslationSource.TextTranslation);

            return ret;
        }
    }
}