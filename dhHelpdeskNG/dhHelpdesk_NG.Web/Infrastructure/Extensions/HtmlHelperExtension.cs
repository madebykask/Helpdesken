using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Web.Models;

namespace dhHelpdesk_NG.Web.Infrastructure.Extensions
{
    using System.Web.Mvc;

    using dhHelpdesk_NG.Web.Models.Faq;
    using dhHelpdesk_NG.Web.Models.Faq.Output;

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
                if (caseType.IsActive == 1) 
                {
                    bool hasChild = false;
                    if (caseType.SubCaseTypes != null)
                        if (caseType.SubCaseTypes.Count > 0)
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

        private static MvcHtmlString BuildProcuctAreaDropdownButton(IList<ProductArea> pal)
        {
            string htmlOutput = string.Empty;

            foreach (ProductArea pa in pal)
            {

                bool hasChild = false;
                if (pa.SubProductAreas != null)
                    if (pa.SubProductAreas.Count > 0)
                        hasChild = true;

                if (hasChild)
                    htmlOutput += "<li class='dropdown-submenu'>";
                else
                    htmlOutput += "<li>";

                htmlOutput += "<a href='#' value=" + pa.Id.ToString() + ">" + pa.Name + "</a>";
                if (hasChild)
                {
                    htmlOutput += "<ul class='dropdown-menu'>";
                    htmlOutput += BuildProcuctAreaDropdownButton(pa.SubProductAreas.ToList());
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

        #endregion
    }
}