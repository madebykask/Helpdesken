namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    using DH.Helpdesk.Web.Models.Invoice;

    public static class InvoiceExtension
    {
        public static MvcHtmlString CaseInvoiceArticles(
                        this HtmlHelper html,
                        CaseInvoiceArticlesModel caseArticles)
        {
            var result = new StringBuilder();
            var tag = new TagBuilder("input");
            tag.MergeAttribute("type", "hidden");

            var serializer = new JavaScriptSerializer();
            var caseArticlesJson = serializer.Serialize(caseArticles);

            tag.MergeAttribute("data-invoice", string.Empty);
            tag.MergeAttribute("data-invoice-case-articles", caseArticlesJson);
            result.Append(tag);
            return MvcHtmlString.Create(result.ToString());            
        }
    }
}