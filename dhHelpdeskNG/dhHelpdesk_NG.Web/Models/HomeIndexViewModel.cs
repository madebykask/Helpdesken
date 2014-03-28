using DH.Helpdesk.BusinessData.Models.Faq.Output;
using DH.Helpdesk.Web.Models.Common;

namespace DH.Helpdesk.Web.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;

    public class HomeIndexViewModel
    {
        public IEnumerable<CustomerUser> CustomerUsers { get; set; }
        public IEnumerable<CustomerUserList> ForStartCaseCustomerUsers { get; set; }

        public IList<BulletinBoard> BulletinBoards { get; set; }
        public IList<Calendar> Calendars { get; set; }
        public IList<WorkingGroupEntity> WorkingGroups { get; set; }

        public IList<SelectListItem> Customers { get; set; }

        public IEnumerable<FaqInfoOverview> Faqs { get; set; }
        public CustomersInfoModel CustomersInfo { get; set; }
    }
}
