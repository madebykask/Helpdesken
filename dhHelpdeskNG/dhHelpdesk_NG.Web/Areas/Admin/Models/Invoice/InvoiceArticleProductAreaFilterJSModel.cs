using DH.Helpdesk.BusinessData.Models.Invoice;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.Domain;
using System.Collections.Generic;

namespace DH.Helpdesk.Web.Areas.Admin.Models.Invoice
{
   

    public class InvoiceArticleProductAreaFilterJSModel
    {

        public InvoiceArticleProductAreaFilterJSModel()
        {

        }
        public int CustomerId { get; set; }

        public string InvoiceArticles { get; set; }

        public string ProductAreas { get; set; }

    }

    public static class InvoiceArticleProductAreaFilterMapper
    {
        private static string _SEPARATOR = ",";
        public static InvoiceArticleProductAreaSelectedFilter MapToSelectedFilter(this InvoiceArticleProductAreaFilterJSModel articleAndProdAreaFilter)
        {
            var ret = new InvoiceArticleProductAreaSelectedFilter();
            ret.CustomerId = articleAndProdAreaFilter.CustomerId;
            ret.SelectedProductAreas.AddItems(articleAndProdAreaFilter.ProductAreas);
            ret.SelectedInvoiceArticles.AddItems(articleAndProdAreaFilter.InvoiceArticles);
            
            return ret;
        }

        
    }
}