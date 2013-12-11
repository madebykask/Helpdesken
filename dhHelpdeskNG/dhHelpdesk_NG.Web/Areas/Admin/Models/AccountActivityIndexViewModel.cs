using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class AccountActivityIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<AccountActivity> AccountActivities { get; set; }
    }
}