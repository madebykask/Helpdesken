namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using System.Collections.Generic;

    public class InvoiceArticleProductAreaSelectedFilter
    {
        public InvoiceArticleProductAreaSelectedFilter()
        { 
            this.SelectedProductAreas = new SelectedItems();
            this.SelectedInvoiceArticles = new SelectedItems();
           
        }

        public SelectedItems SelectedProductAreas { get; set; }

        public SelectedItems SelectedInvoiceArticles { get; set; }

    }
}