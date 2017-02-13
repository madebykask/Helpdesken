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

        public int CustomerId { get; set; }

        public List<int> SelectedProductAreas { get; set; }

        public List<int> SelectedInvoiceArticles { get; set; }

		public int Order { get; set; }
		public string Dir { get; set; }

    }
}