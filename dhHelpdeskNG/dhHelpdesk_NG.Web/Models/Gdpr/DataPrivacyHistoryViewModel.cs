using System.Collections.Generic;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Gdpr;

namespace DH.Helpdesk.Web.Models.Gdpr
{
    public class DataPrivacyHistoryViewModel
    {
        public IList<SelectListItem> Customers { get; set; }
        public int SelectedCustomerId { get; set; }

        public IList<GdprOperationsAuditOverview> Data { get; set; }
    }
}