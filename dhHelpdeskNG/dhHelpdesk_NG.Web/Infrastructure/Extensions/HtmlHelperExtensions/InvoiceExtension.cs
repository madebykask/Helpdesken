namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    using DH.Helpdesk.Web.Models.Invoice;

    public static class InvoiceExtension
    {
        public static MvcHtmlString CaseInvoiceArticles(
                        this HtmlHelper html,
                        CaseInvoiceArticlesModel caseArticles,
                        string productAreaSelector,
                        int caseId,
                        string caseInvoiceArticlesSelector,
                        int customerId,
                        string btnCaption,
                        string btnHint,
                        string caseKey,
                        string logKey)
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
            tag.MergeAttribute("data-invoice-articles-for-save", caseInvoiceArticlesSelector);
            tag.MergeAttribute("data-invoice-date-format", Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern.ToLower());
            tag.MergeAttribute("data-invoice-customerId", customerId.ToString(CultureInfo.InvariantCulture));
            tag.MergeAttribute("data-invoice-Caption", btnCaption);
            tag.MergeAttribute("data-invoice-Hint", btnHint);
            tag.MergeAttribute("data-invoice-case-key", caseKey);
            tag.MergeAttribute("data-invoice-log-key", logKey);

            result.Append(tag);
            return MvcHtmlString.Create(result.ToString());            
        }
    }
}