namespace dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Text;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

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

        #endregion
    }
}