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

            var buttonId = id + "_button";
            var listId = id + "_list";
            var valueContainerId = id + "_value_container";

            DrawScripts(htmlOutput, buttonId, listId, valueContainerId);
            DrawHat(htmlOutput, id);
            DrawButton(htmlOutput, buttonId, dropDownContent);
            DrawList(htmlOutput, listId, allowEmpty, dropDownContent);
            DrawValueContainer(htmlOutput, valueContainerId, name, dropDownContent.SelectedValue);
            DrawFooter(htmlOutput);

            return new MvcHtmlString(htmlOutput.ToString());
        }

        #endregion

        #region Methods

        private static void DrawButton(StringBuilder htmlOutput, string buttonId, DropDownContent dropDownContent)
        {
            var buttonText = string.IsNullOrEmpty(dropDownContent.SelectedValue)
                                 ? string.Empty
                                 : dropDownContent.Items.Find(i => i.Value == dropDownContent.SelectedValue).Name;

            htmlOutput.AppendLine(
                string.Format(
                    @"<button id=""{0}"" class=""btn dropdown-toggle sr-only"" type=""button"" data-toggle=""dropdown"">", 
                    buttonId));

            htmlOutput.AppendLine(buttonText);
            htmlOutput.AppendLine(@"<span class=""caret""></span>");
            htmlOutput.AppendLine("</button>");
        }

        private static void DrawFooter(StringBuilder htmlOutput)
        {
            htmlOutput.AppendLine("</div>");
        }

        private static void DrawHat(StringBuilder htmlOutput, string controlId)
        {
            htmlOutput.AppendLine(string.Format(@"<div id=""{0}"" class=""dropdown"">", controlId));
        }

        private static void DrawJsToSubscribeOnDropDownItemClicked(
            StringBuilder htmlOutput, string buttonId, string listId, string valueContainerId)
        {
            htmlOutput.AppendLine(string.Format(@"
                $(function() {{
                    $('#{0} a').click(function() {{
                        $('#{1}').text($(this).text());
                        var prevValue = $('#{2}').val();
                        $('#{2}').val($(this).siblings('input').val());
                        if (prevValue != $('#{2}').val()) {{
                                $(this).trigger({{ type: 'valueChanged', oldValue: prevValue, newValue: $('#{2}').val() }});
                        }}
                    }});
                }});", listId, buttonId, valueContainerId));
        }

        private static void DrawList(StringBuilder htmlOutput, string listId, bool allowEmpty, DropDownContent dropDownContent)
        {
            htmlOutput.AppendLine(string.Format(@"<ul id=""{0}"" class=""dropdown-menu"">", listId));

            if (allowEmpty)
            {
                htmlOutput.AppendLine("<li>");
                htmlOutput.AppendLine(@"<a href=""javascript:void(0)"" class=""dropdown-empty-item""></a>");
                htmlOutput.AppendLine(@"<input type=""hidden"" />");
                htmlOutput.AppendLine("</li>");
            }

            foreach (var item in dropDownContent.Items)
            {
                htmlOutput.AppendLine("<li>");
                htmlOutput.AppendLine(@"<a href=""javascript:void(0)"">");
                htmlOutput.AppendLine(item.Name);
                htmlOutput.AppendLine("</a>");
                htmlOutput.AppendLine(string.Format(@"<input type=""hidden"" value=""{0}"" />", item.Value));
                htmlOutput.AppendLine("</li>");
            }

            htmlOutput.AppendLine("</ul>");
        }

        private static void DrawScripts(
            StringBuilder htmlOutput, string buttonId, string listId, string valueContainerId)
        {
            htmlOutput.AppendLine(@"<script type=""text/javascript"">");
            DrawJsToSubscribeOnDropDownItemClicked(htmlOutput, buttonId, listId, valueContainerId);
            htmlOutput.AppendLine("</script>");
        }

        private static void DrawValueContainer(
            StringBuilder htmlOutput, string valueContainerId, string name, string initialValue)
        {
            if (string.IsNullOrEmpty(initialValue))
            {
                htmlOutput.AppendLine(
                    string.Format(@"<input type=""hidden"" id=""{0}"" name=""{1}"" />", valueContainerId, name));
            }
            else
            {
                htmlOutput.AppendLine(
                    string.Format(
                        @"<input type=""hidden"" id=""{0}"" name=""{1}"" value=""{2}"" />", 
                        valueContainerId, 
                        name, 
                        initialValue));
            }
        }

        #endregion
    }
}