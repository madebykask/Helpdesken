using DH.Helpdesk.BusinessData.Models.Customer.Input;

namespace DH.Helpdesk.SelfService.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class MasterPageViewModel
    {
        public int SelectedCustomerId { get; set; }
        public int SelectedLanguageId { get; set; }

        public Setting CustomerSetting { get; set; }
        public IList<CustomerOverview> Customers { get; set; }
        public IList<Language> Languages { get; set; }
    }
}