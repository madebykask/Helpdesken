namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Globalization;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    using DH.Helpdesk.Web.Models.Invoice;

    public static class InvoiceExtension
    {
        public static MvcHtmlString CaseInvoiceArticles(
                        this HtmlHelper html,
                        CaseInvoiceArticlesModel caseArticles,
                        string productAreaSelector,
                        int caseId)
        {
            var result = new StringBuilder();
            var tag = new TagBuilder("input");
            tag.MergeAttribute("type", "hidden");

            var serializer = new JavaScriptSerializer();
            var caseArticlesJson = serializer.Serialize(caseArticles);

            tag.MergeAttribute("data-invoice", string.Empty);
            tag.MergeAttribute("data-invoice-case-articles", caseArticlesJson);
            tag.MergeAttribute("data-invoice-product-area", productAreaSelector);
            tag.MergeAttribute("data-invoice-case-id", caseId.ToString(CultureInfo.InvariantCulture));
            result.Append(tag);
            return MvcHtmlString.Create(result.ToString());            
        }
    }
}