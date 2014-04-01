using System.Linq;
using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;
using DH.Helpdesk.BusinessData.Models.Calendar.Output;
using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
using DH.Helpdesk.BusinessData.Models.Faq.Output;
using DH.Helpdesk.BusinessData.Models.Link.Output;
using DH.Helpdesk.BusinessData.Models.OperationLog.Output;
using DH.Helpdesk.Web.Models.Common;
using DH.Helpdesk.Web.Models.Customers;
using DH.Helpdesk.Web.Models.Link;

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

        public CustomersInfoViewModel CustomersInfo { get; set; }
        public IEnumerable<BulletinBoardOverview> BulletinBoardOverviews { get; set; }
        public IEnumerable<CalendarOverview> CalendarOverviews { get; set; }
        public IEnumerable<FaqInfoOverview> FaqOverviews { get; set; }
        public IEnumerable<OperationLogOverview> OperationLogOverviews { get; set; }
        public IEnumerable<DailyReportOverview> DailyReportOverviews { get; set; }
        public LinksInfoViewModel LinksInfo { get; set; }
    }
}
