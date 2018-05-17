using System.Collections.Generic;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Models.Gdpr
{
    public class DataPrivacyHistoryViewModel
    {
        public IList<SelectListItem> Customers { get; set; }
        public int SelectedCustomerId { get; set; }
    }
}