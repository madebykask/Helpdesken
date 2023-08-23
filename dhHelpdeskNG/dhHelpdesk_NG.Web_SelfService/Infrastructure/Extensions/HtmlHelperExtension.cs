using DH.Helpdesk.BusinessData.Models.CaseSolution;

namespace DH.Helpdesk.SelfService.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;

    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.SelfService.Models;

    public static class HtmlHelperExtension
    {
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

        public static MvcHtmlString CaseTypeDropdownButtonString(this HtmlHelper helper, IList<CaseType> caseTypes)
        {
            if (caseTypes != null)
            {
                return BuildCaseTypeDropdownButton(caseTypes) ;
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString ProductAreaDropdownButtonString(this HtmlHelper helper, IList<ProductArea> pal)
        {
            if (pal != null)
            {
                return BuildProcuctAreaDropdownButton(pal);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        private static MvcHtmlString BuildProcuctAreaDropdownButton(IList<ProductArea> pal)
        {
            var pas = BuildProcuctAreaDropdownButtonString(pal);
            return new MvcHtmlString(pas);
        }

        public static MvcHtmlString CategoryDropdownButtonString(this HtmlHelper helper, IList<Category> cats)
        {
            if (cats != null)
            {
                return BuildCategoryDropdownButton(cats);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString FinishingCauseDropdownButtonString(this HtmlHelper helper, IList<FinishingCause> causes)
        {
            if (causes != null)
            {
                return BuildFinishingCauseDropdownButton(causes);
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

        public static MvcHtmlString OrderTypeTreeString(this HtmlHelper helper, IList<OrderType> orderTypes)
        {
            if (orderTypes != null)
            {
                return BuildOrderTypeTreeRow(orderTypes, 0);
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

        public static MvcHtmlString OUTreeString(this HtmlHelper helper, IList<OU> ous)
        {
            ous = ous.OrderBy(x => x.Department.DepartmentName).ToList();

            if (ous != null)
            {
                return BuildOUTreeRow(ous, 0);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString ProductAreaTreeString(this HtmlHelper helper, IList<ProductArea> productAreas)
        {
            if (productAreas != null)
            {
                return BuildProductAreaTreeRow(productAreas, 0);
            }
            else
                return new MvcHtmlString(string.Empty);
        }        

        public static MvcHtmlString GetCaseHistoryInfo(this CaseHistory cur, CaseHistory old, int customerId, int departmentFilterFormat, IList<CaseFieldSetting> cfs)
        {
            StringBuilder sb = new StringBuilder();
            const string bs = "<th>";
            const string be = "</th>";
            const string ey = "";
            const string from = " &rarr; ";

            var o = (old != null ? old : new CaseHistory());

            // Reported by
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.ReportedBy.ToString()).ShowOnStartPage == 1)
            {
                if (cur.ReportedBy != o.ReportedBy)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.ReportedBy.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append("<td>");
                    sb.Append(o.ReportedBy);
                    sb.Append(from);
                    sb.Append(cur.ReportedBy);
                    sb.Append("</td>");
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
                    sb.Append("<td>");
                    sb.Append(o.PersonsName);
                    sb.Append(from);
                    sb.Append(cur.PersonsName);
                    sb.Append("</td>");
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
                    sb.Append("<td>");
                    sb.Append(o.PersonsPhone);
                    sb.Append(from);
                    sb.Append(cur.PersonsPhone);
                    sb.Append("</td>");
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
                    sb.Append("<td>");
                    if (o.Department != null)
                        sb.Append(o.Department.DepartmentDescription(departmentFilterFormat));
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Department != null)
                        sb.Append(cur.Department.DepartmentDescription(departmentFilterFormat));
                    else
                        sb.Append(ey);
                    sb.Append("</td>");
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
                    sb.Append("<td>");
                    sb.Append(o.UserCode);
                    sb.Append(from);
                    sb.Append(cur.UserCode);
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
                    sb.Append("<td>");
                    if (o.CaseType != null)
                        sb.Append(o.CaseType.Name);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.CaseType != null)
                        sb.Append(cur.CaseType.Name);
                    else
                        sb.Append(ey);
                    sb.Append("</td>");
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
                    sb.Append("<td>");
                    if (o.ProductArea != null)
                        sb.Append(o.ProductArea.Name);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.ProductArea != null)
                        sb.Append(cur.ProductArea.Name);
                    else
                        sb.Append(ey);
                    sb.Append("</td>");
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
                    sb.Append("<td>");
                    sb.Append(o.ReferenceNumber);
                    sb.Append(from);
                    sb.Append(cur.ReferenceNumber);
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
                    sb.Append("<td>");
                    sb.Append(o.Caption);
                    sb.Append(from);
                    sb.Append(cur.Caption);
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
                    sb.Append("<td>");
                    if (o.UserPerformer != null)
                        sb.Append(o.UserPerformer.FirstName + " " + o.UserPerformer.SurName);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.UserPerformer != null)
                        sb.Append(cur.UserPerformer.FirstName + " " + cur.UserPerformer.SurName);
                    else
                        sb.Append(ey);
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
            }
            // Priority
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Priority_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Priority_Id != o.Priority_Id && cur.Priority_Id != null)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Priority_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append("<td>");
                    if (o.Priority != null)
                        sb.Append(o.Priority.Name);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Priority != null)
                        sb.Append(cur.Priority.Name);
                    else
                        sb.Append(ey);
                    sb.Append("</td>");
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
                    sb.Append("<td>");
                    if (o.WorkingGroup != null)
                        sb.Append(o.WorkingGroup.WorkingGroupName);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.WorkingGroup != null)
                        sb.Append(cur.WorkingGroup.WorkingGroupName);
                    else
                        sb.Append(ey);
                    sb.Append("</td>");
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
                    sb.Append("<td>");
                    if (o.StateSecondary != null)
                        sb.Append(o.StateSecondary.Name);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.StateSecondary != null)
                        sb.Append(cur.StateSecondary.Name);
                    else
                        sb.Append(ey);
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
            }
            // Status
            if (cfs.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.Status_Id.ToString()).ShowOnStartPage == 1)
            {
                if (cur.Status_Id != o.Status_Id && cur.Status_Id != null)
                {
                    sb.Append("<tr>");
                    sb.Append(bs + Translation.Get(GlobalEnums.TranslationCaseFields.Status_Id.ToString(), Enums.TranslationSource.CaseTranslation, customerId) + be);
                    sb.Append("<td>");
                    if (o.Status != null)
                        sb.Append(o.Status.Name);
                    else
                        sb.Append(ey);
                    sb.Append(from);
                    if (cur.Status != null)
                        sb.Append(cur.Status.Name);
                    else
                        sb.Append(ey);
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
            }
            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString CaseSolutionDropdownButtonString(this HtmlHelper helper, IList<CaseTemplateCategoryNode> categories, int customerId)
        {
            if (categories != null)
            {
                return BuildCaseSolutionCategoryDropdownButton(categories, customerId);
            }
            else
                return new MvcHtmlString(string.Empty);
        }

        private static MvcHtmlString BuildCaseTypeTreeRow(IList<CaseType> caseTypes, int iteration)
        {
            string htmlOutput = string.Empty;

            foreach (CaseType caseType in caseTypes)
            {
                htmlOutput += "<tr>";
                htmlOutput += "<td><a href='/admin/casetype/edit/" + caseType.Id + "' style='padding-left: " + iteration + "px'><i class='icon-resize-full icon-dh'></i>" + caseType.Name + "</a></td>";
                htmlOutput += "<td><a href='/admin/casetype/edit/" + caseType.Id + "'>" + caseType.IsDefault.TranslateBit() + "</a></td>";
                htmlOutput += "<td><a href='/admin/casetype/edit/" + caseType.Id + "'>" + caseType.RequireApproving.TranslateBit() + "</a></td>";
                htmlOutput += "</tr>";

                if (caseType.SubCaseTypes != null)
                    if (caseType.SubCaseTypes.Count > 0)
                        htmlOutput += BuildCaseTypeTreeRow(caseType.SubCaseTypes.ToList(), iteration + 20);
            }

            return new MvcHtmlString(htmlOutput);
        }

        private static MvcHtmlString BuildCaseTypeDropdownButton(IList<CaseType> caseTypes)
        {
            string htmlOutput = string.Empty;

            foreach (CaseType caseType in caseTypes)
            {
                if (caseType.IsActive != 0 && caseType.ShowOnExternalPage != 0) 
                {
                    bool hasChild = false;
                    if (caseType.SubCaseTypes != null)
                        if (caseType.SubCaseTypes.Where(s=> s.IsActive != 0 && s.ShowOnExternalPage != 0).Any())
                            hasChild = true;

                    if (hasChild)
                        htmlOutput += "<li class='dropdown-submenu'>";
                    else
                        htmlOutput += "<li>";

                    htmlOutput += "<a href='#' value=" + caseType.Id.ToString() + ">" + Translation.Get(caseType.Name, Enums.TranslationSource.TextTranslation) + "</a>";
                    if (hasChild)
                    {
                        htmlOutput += "<ul class='dropdown-menu'>";
                        htmlOutput += BuildCaseTypeDropdownButton(caseType.SubCaseTypes.ToList());
                        htmlOutput += "</ul>";
                    }
                    htmlOutput += "</li>";
                }
            }

            return new MvcHtmlString(htmlOutput);
        }

        private static MvcHtmlString BuildFinishingCauseDropdownButton(IList<FinishingCause> causes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (FinishingCause f in causes)
            {
                if (f.IsActive == 1)
                {
                    bool hasChild = false;
                    if (f.SubFinishingCauses != null)
                        if (f.SubFinishingCauses.Count > 0)
                            hasChild = true;

                    if (hasChild)
                        sb.Append("<li class='dropdown-submenu'>");
                    else
                        sb.Append("<li>");

                    sb.Append("<a href='#' value=" + f.Id.ToString() + ">" + Translation.Get(f.Name, Enums.TranslationSource.TextTranslation) + "</a>");
                    if (hasChild)
                    {
                        sb.Append("<ul class='dropdown-menu'>");
                        sb.Append(BuildFinishingCauseDropdownButton(f.SubFinishingCauses.ToList()));
                        sb.Append("</ul>");
                    }
                    sb.Append("</li>");
                }
            }

            return new MvcHtmlString(sb.ToString());
        }

//        private static MvcHtmlString BuildProcuctAreaDropdownButton(IList<ProductArea> pal)
//        {
//            string htmlOutput = string.Empty;
//
//            foreach (ProductArea pa in pal)
//            {
//
//                bool hasChild = false;
//                if (pa.SubProductAreas != null)
//                    if (pa.SubProductAreas.Where(s => s.IsActive != 0 && s.ShowOnExternalPage != 0).ToList().Count > 0)
//                        hasChild = true;
//
//                if (hasChild)
//                    htmlOutput += "<li class='dropdown-submenu'>";
//                else
//                    htmlOutput += "<li>";
//
//                htmlOutput += "<a href='#' value=" + pa.Id.ToString() + ">" + pa.Name + "</a>";
//                if (hasChild)
//                {
//                    htmlOutput += "<ul class='dropdown-menu'>";
//                    htmlOutput += BuildProcuctAreaDropdownButton(pa.SubProductAreas.Where(s => s.IsActive != 0 && s.ShowOnExternalPage != 0).ToList());
//                    htmlOutput += "</ul>";
//                }
//                htmlOutput += "</li>";
//            }
//
//            return new MvcHtmlString(htmlOutput);
//        }

        private static MvcHtmlString BuildCategoryDropdownButton(IList<Category> cats)
        {
            string htmlOutput = string.Empty;

            foreach (Category ca in cats)
            {

                bool hasChild = false;
                if (ca.SubCategories != null)
                    if (ca.SubCategories.Where(s => s.IsActive != 0).ToList().Count > 0)
                        hasChild = true;

                if (hasChild)
                    htmlOutput += "<li class='dropdown-submenu'>";
                else
                    htmlOutput += "<li>";

                htmlOutput += "<a href='#' value=" + ca.Id.ToString() + ">" + ca.Name + "</a>";
                if (hasChild)
                {
                    htmlOutput += "<ul class='dropdown-menu'>";
                    htmlOutput += BuildCategoryDropdownButton(ca.SubCategories.Where(s => s.IsActive != 0).ToList());
                    htmlOutput += "</ul>";
                }
                htmlOutput += "</li>";
            }

            return new MvcHtmlString(htmlOutput);
        }

        private static MvcHtmlString BuildFinishingCauseTreeRow(IList<FinishingCause> finishingCauses, int iteration)
        {
            string htmlOutput = string.Empty;

            foreach (FinishingCause finishingCause in finishingCauses)
            {
                htmlOutput += "<tr>";
                htmlOutput += "<td><a href='/admin/finishingcause/edit/" + finishingCause.Id + "' style='padding-left: " + iteration + "px'><i class='icon-resize-full icon-dh'></i>" + finishingCause.Name + "</a></td>";
                htmlOutput += "<td><a href='/admin/finishingcause/edit/" + finishingCause.Id + "'>" + finishingCause.IsActive.TranslateBit() + "</a></td>";
                htmlOutput += "</tr>";

                if (finishingCause.SubFinishingCauses != null)
                    if (finishingCause.SubFinishingCauses.Count > 0)
                        htmlOutput += BuildFinishingCauseTreeRow(finishingCause.SubFinishingCauses.ToList(), iteration + 20);
            }

            return new MvcHtmlString(htmlOutput);
        }

        private static MvcHtmlString BuildOrderTypeTreeRow(IList<OrderType> orderTypes, int iteration)
        {
            string htmlOutput = string.Empty;

            foreach (OrderType orderType in orderTypes)
            {
                htmlOutput += "<tr>";
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
                        htmlOutput += BuildOrderTypeTreeRow(orderType.SubOrderTypes.ToList(), iteration + 20);
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

        private static MvcHtmlString BuildOUTreeRow(IList<OU> ous, int iteration)
        {
            string htmlOutput = string.Empty;

            foreach (OU ou in ous)
            {
                htmlOutput += "<tr>";
                htmlOutput += "<td><a href='/admin/ou/edit/" + ou.Id + "?customerId=" + ou.Department.Customer_Id + "'>" + ou.Department.DepartmentName + "</a></td>";
                htmlOutput += "<td><a href='/admin/ou/edit/" + ou.Id + "?customerId=" + ou.Department.Customer_Id + "' style='padding-left: " + iteration + "px'><i class='icon-resize-full icon-dh'></i>" + ou.OUId + " (" + ou.Name + ")</a></td>";
                htmlOutput += "<td><a href='/admin/ou/edit/" + ou.Id + "?customerId=" + ou.Department.Customer_Id + "'>" + ou.IsActive.TranslateBit() + "</a></td>";
                htmlOutput += "</tr>";

                if (ou.SubOUs != null)
                    if (ou.SubOUs.Count > 0)
                        htmlOutput += BuildOUTreeRow(ou.SubOUs.ToList(), iteration + 20);
            }

            return new MvcHtmlString(htmlOutput);
        }

        private static MvcHtmlString BuildProductAreaTreeRow(IList<ProductArea> productAreas, int iteration)
        {
            string htmlOutput = string.Empty;

            foreach (ProductArea productArea in productAreas)
            {
                htmlOutput += "<tr>";
                htmlOutput += "<td><a href='/admin/productarea/edit/" + productArea.Id + "' style='padding-left: " + iteration + "px'><i class='icon-resize-full icon-dh'></i>" + productArea.Name + "</a></td>";
                htmlOutput += "<td><a href='/admin/productarea/edit/" + productArea.Id + "'>" + productArea.IsActive.TranslateBit() + "</a></td>";
                htmlOutput += "</tr>";

                if (productArea.SubProductAreas != null)
                    if (productArea.SubProductAreas.Count > 0)
                        htmlOutput += BuildProductAreaTreeRow(productArea.SubProductAreas.ToList(), iteration + 20);
            }

            return new MvcHtmlString(htmlOutput);
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
                    sb.Append("<a href='cases/new/?customerId=" + customerId.ToString() + "&templateId=" + f.CategoryId.ToString() + "' value=" + f.CategoryId.ToString() + ">" +
                           Translation.Get(f.CategoryName, Enums.TranslationSource.TextTranslation) + "</a>");
                else                
                   sb.Append("<a href='#' value=" + f.CategoryId.ToString() + ">" +
                           Translation.Get(f.CategoryName, Enums.TranslationSource.TextTranslation) + "</a>");

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
                sb.Append("<a href='cases/new/?customerId=" + customerId.ToString() + "&templateId=" + f.CaseTemplateId.ToString() + "' value=" + f.CaseTemplateId.ToString() + ">" +
                          Translation.Get(f.CaseTemplateName, Enums.TranslationSource.TextTranslation) + "</a>");                
                sb.Append("</li>");
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static string ProductAreaDropdownString(IList<ProductArea> pal, bool isTakeOnlyActive = true)
        {
            return pal != null ? BuildProcuctAreaDropdownButtonString(pal) : string.Empty;
        }

        private static string BuildProcuctAreaDropdownButtonString(IList<ProductArea> pal)
        {
            var htmlOutput = string.Empty;

            foreach (var pa in pal)
            {

                var hasChild = pa.SubProductAreas?.Count(s => s.IsActive != 0 && s.ShowOnExternalPage != 0) > 0;

                if (hasChild)
                    htmlOutput += "<li id='" + pa.Id + "' class='dropdown-submenu DynamicDropDown_Up'>";
                else
                    htmlOutput += "<li>";

                htmlOutput += "<a href='#' value=" + pa.Id + ">" + pa.Name + "</a>";
                if (hasChild)
                {
                    htmlOutput += "<ul id='subDropDownMenu_"+ pa.Id +"' class='dropdown-menu subddMenu'>";
                    htmlOutput += BuildProcuctAreaDropdownButton(pa.SubProductAreas.Where(s => s.IsActive != 0 && s.ShowOnExternalPage != 0).OrderBy(s => s.Name).ToList());
                    htmlOutput += "</ul>";
                }
                htmlOutput += "</li>";
            }

            return htmlOutput;
        }

        #endregion
    }
}