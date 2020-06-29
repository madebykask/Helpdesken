using DH.Helpdesk.BusinessData.Models.Invoice;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.Domain;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Shared;

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
        public static InvoiceArticleProductAreaSelectedFilter MapToSelectedFilter(this InvoiceArticleProductAreaFilterJSModel articleAndProdAreaFilter)
        {
            var ret = new InvoiceArticleProductAreaSelectedFilter();
            ret.CustomerId = articleAndProdAreaFilter.CustomerId;
			var pa = new SelectedItems();
			pa.AddItems(articleAndProdAreaFilter.ProductAreas);
            ret.SelectedProductAreas.AddRange(pa);
			var ia = new SelectedItems();
			ia.AddItems(articleAndProdAreaFilter.InvoiceArticles);
            ret.SelectedInvoiceArticles.AddRange(ia);
            
            return ret;
        }

        
    }
}