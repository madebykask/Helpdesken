using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DH.Helpdesk.SelfService.Models.FAQ
{
    public class FAQIndexViewModel
    {        
        public FAQIndexViewModel()
        {
            FAQCategories = new List<FAQCategoryViewModel>();
        }

        public string BaseFilePath { get; set; }

        public IList<FAQCategoryViewModel> FAQCategories { get; set; }
    }

    public sealed class FAQCategoryViewModel
    {
        public FAQCategoryViewModel()
        {
            FaqRows = new List<FAQRowModel>();
            SubCategories = new List<FAQCategoryViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IList<FAQRowModel> FaqRows { get; set; }

        public IList<FAQCategoryViewModel> SubCategories { get; set; }

    }
    
    public sealed class FAQRowModel
    {
        public FAQRowModel()
        {
            Files = new List<FAQFileModel>();
        }

        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string InternalAnswer { get; set; }

        public string Url1 { get; set; }

        public string Url2 { get; set; }

        public DateTime CreatedDate { get; set; }

        public IList<FAQFileModel> Files { get; set; }

    }

    public sealed class FAQFileModel
    {
        public int Id { get; set; }

        public int Faq_Id { get; set; }

        public string FileName { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public static class HtmlGenerator
    {
        public static string ConvertToHtml(this IList<FAQCategoryViewModel> faqCategories)
        {
            var html = new StringBuilder();
            foreach (var cat in faqCategories)
            {
                var hasChild = cat.SubCategories != null && cat.SubCategories.Any();

                html.Append(string.Format("<ul class='faq-unstyled'><li>"));
                
                html.Append(string.Format("<div>"));

                if (hasChild)
                {
                    html.Append(string.Format("<a id='node-{0}' class='faq-expand'  >", cat.Id));
                    html.Append(string.Format("<i class='fa fa-plus' aria-hidden='true'></i>"));
                    html.Append(string.Format("&nbsp;&nbsp;{0}</a>", cat.Name));

                    html.Append(string.Format("<a id='node-{0}' class='faq-collapse' style='display:none'>", cat.Id));
                    html.Append(string.Format("<i class='fa fa-minus' aria-hidden='true' ></i>"));
                    html.Append(string.Format("&nbsp;&nbsp;{0}</a>", cat.Name));
                }
                else
                {
                    html.Append(string.Format("<a id='node-{0}' class='faq-node' >", cat.Id));
                    html.Append(string.Format("<i class='fa fa-server' aria-hidden='true'></i>"));
                    html.Append(string.Format("&nbsp;&nbsp;{0}</a>", cat.Name));
                }

                html.Append(string.Format("</div>"));

                if (hasChild)
                {
                    html.Append(string.Format("<div style='display:none'>"));
                    html.Append(ConvertToHtml(cat.SubCategories));
                    html.Append(string.Format("</div>"));
                }

                html.Append(string.Format("</li> </ul>"));
            }

            return html.ToString();
        }
    }
}