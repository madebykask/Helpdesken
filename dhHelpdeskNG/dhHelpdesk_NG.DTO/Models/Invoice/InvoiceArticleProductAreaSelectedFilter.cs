namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using System.Collections.Generic;

    public class InvoiceArticleProductAreaSelectedFilterJS
    {
        public InvoiceArticleProductAreaSelectedFilterJS()
        {
            
        }

        public int CustomerId { get; set; }

        public string SelectedProductAreas { get; set; }

        public string SelectedInvoiceArticles { get; set; }

        public int Order { get; set; }
        public string Dir { get; set; }

    }

    public class InvoiceArticleProductAreaSelectedFilter
    {
        public InvoiceArticleProductAreaSelectedFilter()
        {
            this.SelectedProductAreas = new SelectedItems();
            this.SelectedInvoiceArticles = new SelectedItems();
        }

        public int CustomerId { get; set; }

        public SelectedItems SelectedProductAreas { get; set; }

        public SelectedItems SelectedInvoiceArticles { get; set; }

		public int Order { get; set; }
		public string Dir { get; set; }

    }


    public static class FilterHelper
    {
        public static InvoiceArticleProductAreaSelectedFilter ToModel(this InvoiceArticleProductAreaSelectedFilterJS filter)
        {
            return new InvoiceArticleProductAreaSelectedFilter
            {
                CustomerId = filter.CustomerId,
                Dir = filter.Dir,
                Order = filter.Order,
                SelectedInvoiceArticles = new SelectedItems(filter.SelectedInvoiceArticles),
                SelectedProductAreas = new SelectedItems(filter.SelectedProductAreas)                
            };
        }
    }
}