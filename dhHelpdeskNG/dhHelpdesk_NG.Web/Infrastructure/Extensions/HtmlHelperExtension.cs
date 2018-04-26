


using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.BusinessData.Models.FinishingCause;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Web.Infrastructure.Case;

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

        public static MvcHtmlString CaseTypeDropdownButtonString(this HtmlHelper helper, IList<CaseTypeOverview> caseTypes, bool isTakeOnlyActive = true)
        {
            if (caseTypes != null)
            {
                return BuildCaseTypeDropdownButton(caseTypes, isTakeOnlyActive);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString ProductAreaDropdownButtonString(this HtmlHelper helper, IList<ProductAreaOverview> pal, bool isTakeOnlyActive = true, int? productAreaIdToInclude = null)
        {
            if (pal != null)
            {
                return BuildProcuctAreaDropdownButton(pal, isTakeOnlyActive, null, productAreaIdToInclude);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static string ProductAreaDropdownString(IList<ProductAreaOverview> pal, bool isTakeOnlyActive = true, int? productAreaIdToInclude = null)
        {
            var output = pal != null
                ? BuildProductAreaDropdownButtonString(pal, isTakeOnlyActive, null, productAreaIdToInclude)
                : string.Empty;
            return output;
        }

        public static MvcHtmlString CategoryDropdownButtonString(this HtmlHelper helper, IList<CategoryOverview> cats, bool isTakeOnlyActive = true)
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

        public static MvcHtmlString FinishingCauseDropdownButtonString(this HtmlHelper helper, IList<FinishingCauseOverview> causes, bool isTakeOnlyActive = true)
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
            var s = CaseHistoryBuilder.GetCaseHistoryInfo(cur, old, customerId, departmentFilterFormat, cfs, outFormatter);
            return new MvcHtmlString(s);
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

        private static MvcHtmlString BuildCaseTypeDropdownButton(IList<CaseTypeOverview> caseTypes, bool isTakeOnlyActive = true)
        {
            var res = new StringBuilder();
            
            foreach (var caseType in caseTypes.OrderBy(c => Translation.GetMasterDataTranslation(c.Name)))
            {
                var childs = caseType.SubCaseTypes.Where(p => !isTakeOnlyActive || (p.IsActive != 0 && p.Selectable != 0)).ToList();
                
                var cls = caseType.IsActive == 1 ? string.Empty : "inactive";

                if (caseType.SubCaseTypes.Count > 0)
                {
                    res.Append("<li class='dropdown-submenu " + cls + "'>");
                }
                else
                {
                    res.Append("<li class='" + cls + "'>");
                }

                res.AppendFormat("<a href='#' value='{0}'>{1}</a>", caseType.Id, Translation.GetMasterDataTranslation(caseType.Name));
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

        [Obsolete("Use BuildCaseTypeDropdownButton")]
        private static MvcHtmlString BuildCaseTypeDropdownButtonOld(IList<CaseType> caseTypes, bool isTakeOnlyActive = true)
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
                    res.Append(BuildCaseTypeDropdownButtonOld(childs.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList(), isTakeOnlyActive));
                    res.Append("</ul>");
                }

                res.Append("</li>");
           }

            return new MvcHtmlString(res.ToString());
        }

        private static MvcHtmlString BuildFinishingCauseDropdownButton(IList<FinishingCauseOverview> causes, bool isTakeOnlyActive = true)
        {
            var sb = new StringBuilder();

            foreach (var cause in causes.OrderBy(c => Translation.GetMasterDataTranslation(c.Name)))
            {
                if (!isTakeOnlyActive || cause.IsActive != 0)
                {
                    var hasChild = cause.ChildFinishingCauses?.Count > 0 &&
                                   (!isTakeOnlyActive || cause.ChildFinishingCauses.Any(sc => sc.IsActive != 0));

                    var cls = cause.IsActive == 1 ? string.Empty : "inactive";

                    if (hasChild)
                        sb.Append("<li class='dropdown-submenu " + cls + "'>");
                    else
                        sb.Append("<li class='" + cls + "'>");

                    sb.Append("<a href='#' value=" + cause.Id.ToString() + ">" + Translation.GetMasterDataTranslation(cause.Name) + "</a>");
                    if (hasChild)
                    {
                        sb.Append("<ul class='dropdown-menu'>");
                        sb.Append(BuildFinishingCauseDropdownButton(cause.ChildFinishingCauses.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList(), isTakeOnlyActive));
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
            IList<ProductAreaOverview> pal,
            bool isTakeOnlyActive = true,
            Dictionary<int, bool> userGroupDictionary = null,
            int? productAreaIdToInclude = null)
        {
            var pas = BuildProductAreaDropdownButtonString(pal, isTakeOnlyActive, userGroupDictionary, productAreaIdToInclude);
            return new MvcHtmlString(pas);
        }

        private static string BuildProductAreaDropdownButtonString(
            IList<ProductAreaOverview> pal,
            bool isTakeOnlyActive = true,
            Dictionary<int, bool> userGroupDictionary = null,
            int? productAreaIdToInclude = null)
        {
            var strBld = new StringBuilder();
            var user = SessionFacade.CurrentUser;

            if (userGroupDictionary == null)
            {
                userGroupDictionary = user.UserWorkingGroups.Where(it => it.UserRole == WorkingGroupUserPermission.ADMINSTRATOR).ToDictionary(it => it.WorkingGroup_Id, it => true);
            }

            foreach (var pa in pal.Where(x => !isTakeOnlyActive || x.IsActive > 0))
            {
                var childs = new List<ProductAreaOverview>();
                if (pa.SubProductAreas != null)
                {
                    childs = pa.SubProductAreas.Where(p => !isTakeOnlyActive || p.IsActive >= 0).ToList();

                    if (user.UserGroupId < (int)UserGroup.CustomerAdministrator)
                    {
                        childs =
                            childs.Where(
                                it =>
                                    it.WorkingGroups.Count == 0
                                    || it.WorkingGroups.Any(wg => userGroupDictionary.ContainsKey(wg.Id))
                                    || (productAreaIdToInclude.HasValue && it.Id == productAreaIdToInclude.Value)).ToList();
                    }
                }

                var cls = pa.IsActive == 1 ? string.Empty : "inactive";
                if (childs.Any())
                {
                    strBld.AppendFormat("<li class=\"dropdown-submenu {0} {1}\" id=\"{2}\">", cls, "DynamicDropDown_Up", pa.Id);
                }
                else
                {
                    strBld.AppendFormat("<li class=\"{0} \" >", cls);
                }

                strBld.AppendFormat("<a href='#' value=\"{0}\">{1}</a>", pa.Id, Translation.GetMasterDataTranslation(pa.Name));

                if (childs.Any())
                {
                    var sortedChilds = childs.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList();
                    strBld.AppendFormat("<ul class='dropdown-menu subddMenu' id=\"subDropDownMenu_{0}\" >", pa.Id);
                    strBld.Append(BuildProductAreaDropdownButtonString(sortedChilds, isTakeOnlyActive, userGroupDictionary));
                    strBld.Append("</ul>");
                }

                strBld.Append("</li>");
            }

            var htmlOutput = strBld.ToString();
            return htmlOutput;
        }

        private static MvcHtmlString BuildCategoryDropdownButton(IList<CategoryOverview> cats, bool activeOnly = true)
        {
            var htmlOutput = new StringBuilder();

            foreach (var ca in cats)
            {
                var childList = new List<CategoryOverview>();
                if (ca.SubCategories != null)
                {
                    var childs = activeOnly
                                 ? ca.SubCategories.Where(p => p.IsActive != 0)
                                 : ca.SubCategories;

                    childList = childs.ToList();
                }

                var cls = ca.IsActive == 1 ? string.Empty : "inactive";
                if (childList.Any())
                {
                    htmlOutput.AppendFormat("<li class=\"dropdown-submenu {0} {1}\" id=\"{2}\">", cls, "DynamicDropDown_Up", ca.Id);
                }
                else
                {
                    htmlOutput.AppendFormat("<li class=\"{0} \" >", cls);
                }

                htmlOutput.AppendFormat("<a href='#' value=\"{0}\">{1}</a>", ca.Id, Translation.GetMasterDataTranslation(ca.Name));

                if (childList.Any())
                {
                    htmlOutput.AppendFormat("<ul class='dropdown-menu' id=\"subDropDownMenu_{0}\" >", ca.Id);

                    var sortedChilds = childList.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList();
                    var output = BuildCategoryDropdownButton(sortedChilds, activeOnly);
                    htmlOutput.Append(output);

                    htmlOutput.AppendFormat("</ul>");
                }

                htmlOutput.AppendFormat("</li>");
            }

            return new MvcHtmlString(htmlOutput.ToString());
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
                    if (productArea.SubProductAreas.Count > 0 && ((isShowOnlyActive && !isInactive) || !isShowOnlyActive))
                    {
                        var subProducts = productArea.SubProductAreas.OrderBy(x => x.Name).ToList();
                        htmlOutput += BuildProductAreaTreeRow(subProducts, iteration + 20, isShowOnlyActive, isInactive);
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
            var sb = new StringBuilder();

            foreach (CaseTemplateCategoryNode f in categories)
            {
                var hasChild = f.CaseTemplates != null && f.CaseTemplates.Count > 0;

                if (hasChild)
                    sb.AppendFormat("<li class='dropdown-submenu DynamicDropDown_Up' id='tpl_{0}'>", f.CategoryId);
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
                    sb.AppendFormat("<ul class='dropdown-menu subddMenu' id='subDropDownMenu_tpl_{0}'>", f.CategoryId);
                    sb.Append(BuildCaseSolutionJSDropdownButton(f.CaseTemplates.ToList(), customerId));
                    sb.Append("</ul>");
                }
                sb.Append("</li>");
            }

            return new MvcHtmlString(sb.ToString());
        }
        private static MvcHtmlString BuildCaseSolutionCategoryDropdownButton(IList<CaseTemplateCategoryNode> categories, int customerId)
        {
            var sb = new StringBuilder();

            foreach (CaseTemplateCategoryNode f in categories)
            {
                var hasChild = f.CaseTemplates != null && f.CaseTemplates.Any();
                if (hasChild)
                    sb.AppendFormat("<li class='dropdown-submenu DynamicDropDown_Up' id='tpl_{0}'>", f.CategoryId);
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
                    sb.AppendFormat("<ul class='dropdown-menu subddMenu' id='subDropDownMenu_tpl_{0}'>", f.CategoryId);
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