namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class AccountActivityIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<AccountActivity> AccountActivities { get; set; }
    }
}