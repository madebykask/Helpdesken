namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public static class TreeExtension   
    {
        public static MvcHtmlString Tree(this HtmlHelper htmlHelper, string id, bool expandAll, TreeContent treeContent)
        {
            var htmlOutput = new StringBuilder();
            DrawScripts(htmlOutput, id, expandAll);
            DrawTree(htmlOutput, id, expandAll, treeContent);
            return new MvcHtmlString(htmlOutput.ToString());
        }

        public static MvcHtmlString CustomDocumentTree(this HtmlHelper htmlHelper, string id, bool expandAll, TreeContent treeContent,string selectedNode)
        {
            var htmlOutput = new StringBuilder();
            DrawScripts(htmlOutput, id, expandAll);
            DrawCustomDocumentTree(htmlOutput, id, expandAll, treeContent, selectedNode);
            return new MvcHtmlString(htmlOutput.ToString());
        }

        private static void DrawCustomDocumentTree(StringBuilder htmlOutput, string controlId, bool expandAll, TreeContent treeContent, string selectedNode)
        {
            htmlOutput.AppendLine(string.Format(@"<ul id=""{0}"">", controlId));

            foreach (var item in treeContent.Items)
            {
                DrawCustomDocumentBranch(htmlOutput, item, treeContent.SelectedValue, expandAll, "Root", selectedNode);
            }

            htmlOutput.AppendLine("</ul>");
        }

        private static void DrawCustomDocumentBranch(StringBuilder htmlOutput, TreeItem item, string selectedValue, bool expandAll, string nodeType, string selectedNode)
        {
            htmlOutput.AppendLine("<li>");

            if (item.Children.Any())
            {
                if (expandAll)
                {
                    htmlOutput.AppendLine(
                        @"<a class=""expand-tree-item"" style=""display: none;""><i class=""icon-folder-close icon-dh""></i></a>");                        
                    htmlOutput.AppendLine(
                        @"<a class=""collapse-tree-item""><i class=""icon-folder-open icon-dh""></i></a>");
                }
                else
                {
                    switch (nodeType)
                    {
                        case "Root":
                            htmlOutput.AppendLine(
                                @"<a class=""expand-tree-item""><i class=""icon-folder-close icon-dh""></i></a>");

                            htmlOutput.AppendLine(
                                @"<a class=""collapse-tree-item"" style=""display: none;""><i class=""icon-folder-open icon-dh""></i></a>");
                            break;

                        default:
                            htmlOutput.AppendLine(
                                @"<a class=""expand-tree-item""><i class=""icon-folder-open icon-dh""></i></a>");

                            htmlOutput.AppendLine(
                                @"<a class=""collapse-tree-item"" style=""display: none;""><i class=""icon-folder-open icon-dh""></i></a>");

                            break;
                    }
                }

                htmlOutput.AppendLine(
                    item.Value == selectedNode
                        ? string.Format(@"<a class=""tree-selected-node"" href=""javascript:void(0)"">{0}</a>", item.Name)
                        : string.Format(@"<a class=""tree-node"" href=""javascript:void(0)"">{0}</a>", item.Name));

                htmlOutput.AppendLine(string.Format(@"<input type=""hidden"" value=""{0}"" />", nodeType + "," + item.Value + "," + item.UniqueId));
                
                htmlOutput.AppendLine("<ul>");
                
                foreach (var child in item.Children)
                {
                    DrawCustomDocumentBranch(htmlOutput, child, selectedValue, expandAll,
                        (nodeType == "Root") ? "Category" : "Document",selectedNode);
                }

                htmlOutput.AppendLine("</ul>");
            }
            else
            {
                switch (nodeType)
                {
                    case "Category":
                        htmlOutput.AppendLine(
                            item.Value == selectedNode
                                ? string.Format(@"<a class=""tree-selected-node"" href=""javascript:void(0)""><i class=""icon-folder-open icon-dh""></i>{0}</a>", " " + item.Name)
                                : string.Format(
                                    @"<a class=""tree-node"" href=""javascript:void(0)""><i class=""icon-folder-open icon-dh""></i>{0}</a>",
                                    " " + item.Name));
                        break;

                    case "Document":
                        htmlOutput.AppendLine(
                            item.Value == selectedNode
                                ? string.Format(@"<a class=""tree-selected-node"" href=""javascript:void(0)""><i class=""icon-folder-open icon-dh""></i>{0}</a>", " " + item.Name)
                                : string.Format(
                                    @"<a class=""tree-node"" href=""javascript:void(0)""><i class=""icon-list-alt icon-dh""></i>{0}</a>",
                                    " " + item.Name));
                        break;
                }

                htmlOutput.AppendLine(string.Format(@"<input type=""hidden"" value=""{0}"" />", nodeType + ","  + item.Value));
            }

            htmlOutput.AppendLine("</li>");
        }

        private static void DrawTree(StringBuilder htmlOutput, string controlId, bool expandAll, TreeContent treeContent)
        {
            htmlOutput.AppendLine(string.Format(@"<ul id=""{0}"">", controlId));

            foreach (var item in treeContent.Items)
            {
                DrawBrunch(htmlOutput, item, treeContent.SelectedValue, expandAll);
            }

            htmlOutput.AppendLine("</ul>");
        }

        private static void DrawBrunch(StringBuilder htmlOutput, TreeItem item, string selectedValue, bool expandAll)
        {
            htmlOutput.AppendLine("<li>");

            if (item.Children.Any())
            {
                if (expandAll)
                {
                    htmlOutput.AppendLine(
                        @"<a class=""expand-tree-item"" style=""display: none;""><i class=""icon-folder-close icon-dh""></i></a>");

                    htmlOutput.AppendLine(@"<a class=""collapse-tree-item""><i class=""icon-folder-open icon-dh""></i></a>");
                }
                else
                {
                    htmlOutput.AppendLine(@"<a class=""expand-tree-item""><i class=""icon-folder-close icon-dh""></i></a>");

                    htmlOutput.AppendLine(
                        @"<a class=""collapse-tree-item"" style=""display: none;""><i class=""icon-folder-open icon-dh""></i></a>");
                }

                htmlOutput.AppendLine(
                    item.Value == selectedValue
                        ? string.Format(@"<a class=""tree-selected-node"" href=""javascript:void(0)"">{0}</a>", item.Name)
                        : string.Format(@"<a class=""tree-node"" href=""javascript:void(0)"">{0}</a>", item.Name));

                htmlOutput.AppendLine(string.Format(@"<input type=""hidden"" value=""{0}"" />", item.Value));
                
                htmlOutput.AppendLine("<ul>");

                foreach (var child in item.Children)
                {
                    DrawBrunch(htmlOutput, child, selectedValue, expandAll);
                }

                htmlOutput.AppendLine("</ul>");
            }
            else
            {
                htmlOutput.AppendLine(
                    item.Value == selectedValue
                        ? string.Format(@"<a class=""tree-selected-node"" href=""javascript:void(0)"">{0}</a>", item.Name)
                        : string.Format(@"<a class=""tree-node"" href=""javascript:void(0)"">{0}</a>", item.Name));

                htmlOutput.AppendLine(string.Format(@"<input type=""hidden"" value=""{0}"" />", item.Value));
            }

            htmlOutput.AppendLine("</li>");
        }

        private static void DrawScripts(StringBuilder htmlOutput, string controlId, bool expandAll)
        {
            htmlOutput.AppendLine(@"<script type=""text/javascript"">");
            htmlOutput.AppendLine("$(function() {");

            if (!expandAll)
            {
                htmlOutput.AppendLine(string.Format(@"$('#{0} ul').hide();", controlId));
            }

            htmlOutput.AppendLine(string.Format(@"
                $('a[class=""tree-node""], a[class=""tree-selected-node""]', '#{0}').click(function() {{
                    $('#{0} a[class=""tree-selected-node""]').attr('class', 'tree-node');
                    $(this).attr('class', 'tree-selected-node');
                    
                }});", controlId));

            htmlOutput.AppendLine(string.Format(@"
                $('#{0} a[class=""expand-tree-item""]').click(function() {{                                        
                    $(this).hide();
                    $(this).siblings('a').show();                    
                    $(this).siblings('ul').show();
                    
                }});", controlId));

            htmlOutput.AppendLine(string.Format(@"
                $('#{0} a[class=""collapse-tree-item""]').click(function() {{                                                            
                    $(this).hide();
                    $(this).siblings('a').show();                    
                    $(this).siblings('ul').hide();
                    
                }});", controlId));

            htmlOutput.AppendLine("});");
            htmlOutput.AppendLine("</script>");
        }

    }


}