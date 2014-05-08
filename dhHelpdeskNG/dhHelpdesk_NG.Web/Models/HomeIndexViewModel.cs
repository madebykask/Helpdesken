using System.Linq;
using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;
using DH.Helpdesk.BusinessData.Models.Calendar.Output;
using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
using DH.Helpdesk.BusinessData.Models.Faq.Output;
using DH.Helpdesk.BusinessData.Models.Link.Output;
using DH.Helpdesk.BusinessData.Models.OperationLog.Output;
using DH.Helpdesk.BusinessData.Models.Problem.Output;
using DH.Helpdesk.BusinessData.Models.Statistics.Output;
using DH.Helpdesk.BusinessData.Models.Users.Output;
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

        private CustomersInfoViewModel _customersInfo = new CustomersInfoViewModel();
        public CustomersInfoViewModel CustomersInfo
        {
            get { return _customersInfo; }
            set { _customersInfo = value; }
        }

        private IEnumerable<BulletinBoardOverview> _bulletinBoardOverviews = new BulletinBoardOverview[] {};        
        public IEnumerable<BulletinBoardOverview> BulletinBoardOverviews
        {
            get { return _bulletinBoardOverviews; }
            set { _bulletinBoardOverviews = value; }
        }

        private IEnumerable<CalendarOverview> _calendarOverviews = new CalendarOverview[] {};
        public IEnumerable<CalendarOverview> CalendarOverviews
        {
            get { return _calendarOverviews; }
            set { _calendarOverviews = value; }
        }

        private IEnumerable<FaqInfoOverview> _faqOverviews = new FaqInfoOverview[]{};
        public IEnumerable<FaqInfoOverview> FaqOverviews
        {
            get { return _faqOverviews; }
            set { _faqOverviews = value; }
        }

        private IEnumerable<OperationLogOverview> _operationLogOverviews = new OperationLogOverview[] {};
        public IEnumerable<OperationLogOverview> OperationLogOverviews
        {
            get { return _operationLogOverviews; }
            set { _operationLogOverviews = value; }
        }

        private IEnumerable<DailyReportOverview> _dailyReportOverviews = new DailyReportOverview[] {};
        public IEnumerable<DailyReportOverview> DailyReportOverviews
        {
            get { return _dailyReportOverviews; }
            set { _dailyReportOverviews = value; }
        }

        private LinksInfoViewModel _linksInfo = new LinksInfoViewModel();
        public LinksInfoViewModel LinksInfo
        {
            get { return _linksInfo; }
            set { _linksInfo = value; }
        }

        private IEnumerable<ProblemInfoOverview> _problemOverviews = new ProblemInfoOverview[] {};
        public IEnumerable<ProblemInfoOverview> ProblemOverviews
        {
            get { return _problemOverviews; }
            set { _problemOverviews = value; }
        }

        private StatisticsOverview _statisticsOverviews = new StatisticsOverview();
        public StatisticsOverview StatisticsOverviews
        {
            get { return _statisticsOverviews; }
            set { _statisticsOverviews = value; }
        }

        private IEnumerable<BusinessData.Models.Document.Output.DocumentOverview> _documentOverviews = new BusinessData.Models.Document.Output.DocumentOverview[] {};
        public IEnumerable<BusinessData.Models.Document.Output.DocumentOverview> DocumentOverviews
        {
            get { return _documentOverviews; }
            set { _documentOverviews = value; }
        }

        public IEnumerable<UserModuleOverview> UserModules { get; set; } 
        public int UserId { get; set; }
    }
}
