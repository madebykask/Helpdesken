namespace DH.Helpdesk.SelfService.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Collections.Generic;
    using System;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    using DH.Helpdesk.SelfService.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public static class DropDownExtension
    {
        #region Public Methods and Operators

        public static MvcHtmlString DropDown(
            this HtmlHelper htmlHelper, string id, string name, bool allowEmpty, DropDownContent dropDownContent)
        {
            var htmlOutput = new StringBuilder();
            htmlOutput.Append(string.Format(@"<select id=""{0}"" name=""{1}"">", id, name));

            if (allowEmpty)
            {
                htmlOutput.Append("<option></option>");
            }

            foreach (var item in dropDownContent.Items)
            {
                htmlOutput.Append(
                    item.Value == dropDownContent.SelectedValue
                        ? string.Format(@"<option value=""{0}"" selected=""selected"">", item.Value)
                        : string.Format(@"<option value=""{0}"">", item.Value));

                htmlOutput.Append(item.Name);
                htmlOutput.Append("</option>");
            }

            htmlOutput.Append("</select>");
            return new MvcHtmlString(htmlOutput.ToString());
        }

        public static MvcHtmlString ReadonlyDropDownListFor<TModel, TProperty>(
          this HtmlHelper<TModel> html,
          System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression,
          IEnumerable<SelectListItem> selectList,
          object htmlAttributes,
          bool isReadonly,
          string id = null)
        {
            var propertyName = ExpressionHelper.GetExpressionText(expression);
            var clientId = string.IsNullOrEmpty(id) ? html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName) : id;
            var htmlAttributesAsDict = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            htmlAttributesAsDict.Add("Name", clientId);

            if (!isReadonly)
            {
                htmlAttributesAsDict.Add("id", clientId);
                return html.DropDownListFor<TModel, TProperty>(expression, selectList, htmlAttributesAsDict);
            }

            htmlAttributesAsDict.Add("id", clientId + "_disabled");
            htmlAttributesAsDict.Add("disabled", "disabled");

            //since asp.net doesn't submit disabled controls - generate hidden input with current value
            var hiddenFieldMarkup = html.HiddenFor<TModel, TProperty>(expression, new { id = clientId, Name = clientId });
            var selectMarkup = html.DropDownListFor<TModel, TProperty>(expression, selectList, htmlAttributesAsDict);

            return MvcHtmlString.Create(selectMarkup + Environment.NewLine + hiddenFieldMarkup);
        }

        #endregion
    }
}