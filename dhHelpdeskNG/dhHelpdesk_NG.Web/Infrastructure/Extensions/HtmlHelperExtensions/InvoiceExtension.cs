namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Linq;

    using DH.Helpdesk.Web.Models.Invoice;
    using DH.Helpdesk.Common.Enums;

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

            var btnCaptionExtra = string.Empty;
            
            if (caseArticles.Invoices != null && caseArticles.Invoices.Length > 0 &&
                caseArticles.Invoices[0].Orders.Length > 0)
            {
                /* Today we have only one Invoice per case */
                var ordersCount = caseArticles.Invoices[0].Orders.Count();
                var sentCount = caseArticles.Invoices[0].Orders.Where(o=> o.OrderState == (int) InvoiceOrderStates.Sent).Count();

                btnCaptionExtra = string.Format("({0}/{1})", ordersCount, sentCount);
            }           
                
            tag.MergeAttribute("data-invoice", string.Empty);
            tag.MergeAttribute("data-invoice-case-articles", caseArticlesJson);
            tag.MergeAttribute("data-invoice-product-area", productAreaSelector);
            tag.MergeAttribute("data-invoice-case-id", caseId.ToString(CultureInfo.InvariantCulture));
            tag.MergeAttribute("data-invoice-articles-for-save", caseInvoiceArticlesSelector);
            tag.MergeAttribute("data-invoice-date-format", Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern.ToLower());
            tag.MergeAttribute("data-invoice-customerId", customerId.ToString(CultureInfo.InvariantCulture));
            tag.MergeAttribute("data-invoice-Caption", btnCaption);
            tag.MergeAttribute("data-invoice-CaptionExtra", btnCaptionExtra);
            tag.MergeAttribute("data-invoice-Hint", btnHint);
            tag.MergeAttribute("data-invoice-case-key", caseKey);
            tag.MergeAttribute("data-invoice-log-key", logKey);
            
            result.Append(tag);
            return MvcHtmlString.Create(result.ToString());            
        }
    }
}