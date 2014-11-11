namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;

    public class AccountActivityIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<AccountActivity> AccountActivities { get; set; }
    }
}