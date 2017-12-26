namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public static class DropDownWithSubmenusExtension
    {
        public static MvcHtmlString DropDownWithSubmenus(this HtmlHelper htmlHelper, string id, string name, bool allowEmpty, DropDownWithSubmenusContent dropDownWithSubmenusContent, bool disabled = false)
        {
            var htmlOutput = new StringBuilder();

            var buttonId = id + "_button";
            var valueContainerId = id + "_value_container";

            DrawScripts(htmlOutput, id, buttonId, valueContainerId);
            DrawHat(htmlOutput, id);
            DrawButton(htmlOutput, buttonId, dropDownWithSubmenusContent, disabled);
            DrawMenu(htmlOutput, allowEmpty, dropDownWithSubmenusContent.Items);
            DrawValueContainer(htmlOutput, valueContainerId, name, dropDownWithSubmenusContent.SelectedValue);
            DrawFooter(htmlOutput);

            return new MvcHtmlString(htmlOutput.ToString());
        }

        private static string TryCreateBreadcrumbsForBrunch(DropDownWithSubmenusItem item, string selectedValue)
        {
            if (item.Value == selectedValue)
            {
                return item.Name;
            }

            if (!item.Subitems.Any())
            {
                return null;
            }
            
            foreach (var subitem in item.Subitems)
            {
                var breadcrumbs = TryCreateBreadcrumbsForBrunch(subitem, selectedValue);
                if (breadcrumbs != null)
                {
                    return item.Name + "-" + breadcrumbs;
                }
            }

            return null;
        }

        private static void DrawButton(StringBuilder htmlOutput, string buttonId, DropDownWithSubmenusContent dropDownWithSubmenusContent, bool disabled = false)
        {
            var buttonText = string.Empty;

            if (!string.IsNullOrEmpty(dropDownWithSubmenusContent.SelectedValue))
            {
                foreach (var item in dropDownWithSubmenusContent.Items)
                {
                    var breadcrumbs = TryCreateBreadcrumbsForBrunch(item, dropDownWithSubmenusContent.SelectedValue);
                    
                    if (!string.IsNullOrEmpty(breadcrumbs))
                    {
                        buttonText = breadcrumbs;
                        break;
                    }
                }
            }
            if (disabled)
            {
                htmlOutput.AppendLine(
                    string.Format(
                        @"<button id=""{0}"" class=""btn dropdown-toggle"" type=""button"" data-toggle=""dropdown"" disabled=""disabled"">",
                        buttonId));
            }
            else
            {
                htmlOutput.AppendLine(
                    string.Format(
                        @"<button id=""{0}"" class=""btn dropdown-toggle"" type=""button"" data-toggle=""dropdown"">",
                        buttonId));
            }

            htmlOutput.AppendLine(buttonText);
            htmlOutput.AppendLine(@"<span class=""caret""></span>");
            htmlOutput.AppendLine(@"</button>");
        }

        private static void DrawHat(StringBuilder htmlOutput, string controlId)
        {
            htmlOutput.AppendLine(string.Format(@"<div id=""{0}"" class=""dropdown"">", controlId));
        }

        private static void DrawFooter(StringBuilder htmlOutput)
        {
            htmlOutput.AppendLine("</div>");
        }

        private static void DrawValueContainer(StringBuilder htmlOutput, string valueContainerId, string name, string initialValue)
        {
            string valueContainer;

            if (string.IsNullOrEmpty(initialValue))
            {
                valueContainer = string.Format(
                    @"<input type=""hidden"" id=""{0}"" name=""{1}"" />", valueContainerId, name);
            }
            else
            {
                valueContainer = string.Format(
                    @"<input type=""hidden"" id=""{0}"" name=""{1}"" value=""{2}"" />", valueContainerId, name, initialValue);
            }
            
            htmlOutput.AppendLine(valueContainer);
        }

        private static void DrawMenu(StringBuilder htmlOutput, bool allowEmpty, List<DropDownWithSubmenusItem> items)
        {
            htmlOutput.AppendLine(@"<ul class=""dropdown-menu"">");

            if (allowEmpty)
            {
                htmlOutput.AppendLine("<li>");
                htmlOutput.AppendLine(@"<a href=""javascript:void(0)"" class=""dropdown-empty-item""></a>");
                htmlOutput.AppendLine(@"<input type=""hidden"" />");
                htmlOutput.AppendLine("</li>");
            }

            foreach (var item in items)
            {
                var hasChild = item.Subitems.Any();
                
                htmlOutput.AppendLine(hasChild ? @"<li class=""dropdown-submenu"">" : "<li>");
                htmlOutput.AppendLine(string.Format(@"<a href=""javascript:void(0)"">{0}</a>", item.Name));
                htmlOutput.AppendLine(string.Format(@"<input type=""hidden"" value=""{0}"" />", item.Value));

                if (hasChild)
                {
                    DrawMenu(htmlOutput, false, item.Subitems);
                }

                htmlOutput.AppendLine("</li>");
            }

            htmlOutput.AppendLine("</ul>");
        }

        private static void DrawScripts(StringBuilder htmlOutput, string controlId, string buttonId, string valueContainerId)
        {
            htmlOutput.AppendLine(@"<script type=""text/javascript"">");
            DrawJsToSubscribeOnMenuItemClicked(htmlOutput, controlId, buttonId, valueContainerId);
            htmlOutput.AppendLine("</script>");
        }

        private static void DrawJsToSubscribeOnMenuItemClicked(
            StringBuilder htmlOutput, string controlId, string buttonId, string valueContainerId)
        {
            htmlOutput.AppendLine(string.Format(@"
                $(function() {{
                    $('#{0} a').click(function() {{
                        $('#{1}').val($(this).siblings('input').val());

                        var breadcrumbs = '';
                        var history = $(this).parents('li').children('a');

                        for (var i = 0; i < history.length; i++) {{
                            if (!breadcrumbs) {{
                                breadcrumbs = $(history[i]).text();
                            }}
                            else {{
                                breadcrumbs = $(history[i]).text() + '-' + breadcrumbs;
                            }}
                        }}
    
                        $('#{2}').html(breadcrumbs + ' <span class=""caret""></span>');
                    }});
                }});", controlId, valueContainerId, buttonId));
        }
    }
}