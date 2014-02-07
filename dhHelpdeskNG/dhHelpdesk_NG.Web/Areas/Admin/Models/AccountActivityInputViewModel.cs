namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class AccountActivityInputViewModel
    {
        public AccountActivity AccountActivity { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> AccountActivityGroups { get; set; }
        public IList<SelectListItem> Cases { get; set; }
        public IList<SelectListItem> Documents { get; set; }
    }
} 