using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Models
{
    public class MasterPageViewModel
    {
        public int SelectedCustomerId { get; set; }
        public int SelectedLanguageId { get; set; }

        public IList<Customer> Customers { get; set; }
        public IList<Language> Languages { get; set; }
    }
}