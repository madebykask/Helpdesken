using System.Collections.Generic;

namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Text;
    using System.Web.Mvc;

    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public static class DropDownExtension
    {
        #region Public Methods and Operators

        public static MvcHtmlString DropDown(this HtmlHelper htmlHelper,
            string id,
            string name,
            bool allowEmpty,
            IList<SelectListItem> items,
            bool required = false,
            bool disabled = false)
        {
            var dropDownContent = new DropDownContent(items);
            return DropDown(htmlHelper, id, name, allowEmpty, dropDownContent, required, disabled);
        }

        public static MvcHtmlString DropDown(
            this HtmlHelper htmlHelper,
            string id,
            string name,
            bool allowEmpty,
            DropDownContent dropDownContent,
            bool required = false,
            bool disabled = false)
        {
            var htmlOutput = new StringBuilder();
            var validation = new StringBuilder();
            var disable = new StringBuilder();
            if (required)
            {
                validation.Append(@"data-val='true' data-val-required='The field is required.'");
            }
            if (disabled)
            {
                disable.Append(@"disabled");
            }

            htmlOutput.Append(string.Format(@"<select id=""{0}"" name=""{1}"" {2} {3}>", id, name, validation, disable));

            if (allowEmpty)
            {
                htmlOutput.Append("<option></option>");
            }

            //if (dropDownContent !=null)
            //{
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
            //}
            return new MvcHtmlString(htmlOutput.ToString());
        }

        #endregion
    }
}