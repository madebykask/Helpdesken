namespace DH.Helpdesk.Web.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class MasterPageViewModel
    {
        public int SelectedCustomerId { get; set; }
        public int SelectedLanguageId { get; set; }

        public IList<Customer> Customers { get; set; }
        public IList<Language> Languages { get; set; }
    }
}