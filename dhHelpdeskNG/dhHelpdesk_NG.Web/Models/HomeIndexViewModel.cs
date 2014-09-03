namespace DH.Helpdesk.Web.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;
    using DH.Helpdesk.BusinessData.Models.Calendar.Output;
    using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.BusinessData.Models.OperationLog.Output;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.BusinessData.Models.Statistics.Output;
    using DH.Helpdesk.BusinessData.Models.Users.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Customers;
    using DH.Helpdesk.Web.Models.Link;

    public class HomeIndexViewModel
    {
        private IEnumerable<BulletinBoardOverview> bulletinBoardOverviews = new BulletinBoardOverview[] { };

        private IEnumerable<CalendarOverview> calendarOverviews = new CalendarOverview[] { };

        private IEnumerable<FaqInfoOverview> faqOverviews = new FaqInfoOverview[] { };

        private IEnumerable<OperationLogOverview> operationLogOverviews = new OperationLogOverview[] { };

        private IEnumerable<DailyReportOverview> dailyReportOverviews = new DailyReportOverview[] { };

        private LinksInfoViewModel linksInfo = new LinksInfoViewModel();

        private IEnumerable<ProblemInfoOverview> problemOverviews = new ProblemInfoOverview[] { };

        private StatisticsOverview statisticsOverviews = new StatisticsOverview();

        private IEnumerable<BusinessData.Models.Document.Output.DocumentOverview> documentOverviews = new BusinessData.Models.Document.Output.DocumentOverview[] { };

        private CustomerChangesModel customerChanges = new CustomerChangesModel();

        private MyCasesModel myCases = new MyCasesModel();

        private CustomersInfoViewModel customersInfo = new CustomersInfoViewModel();

        public IEnumerable<CustomerUser> CustomerUsers { get; set; }

        public IEnumerable<CustomerUserList> ForStartCaseCustomerUsers { get; set; }

        public IList<BulletinBoard> BulletinBoards { get; set; }
        
        public IList<Calendar> Calendars { get; set; }

        public IList<WorkingGroupEntity> WorkingGroups { get; set; }

        public IList<SelectListItem> Customers { get; set; }

        public CustomersInfoViewModel CustomersInfo
        {
            get
            {
                return this.customersInfo;
            }

            set 
            { 
                this.customersInfo = value;
            }
        }

        public CustomerChangesModel CustomerChanges
        {
            get
            {
                return this.customerChanges;
            }

            set
            {
                this.customerChanges = value;
            }
        }

        public MyCasesModel MyCases
        {
            get
            {
                return this.myCases;
            }

            set
            {
                this.myCases = value;
            }
        }

        public IEnumerable<BulletinBoardOverview> BulletinBoardOverviews
        {
            get { return this.bulletinBoardOverviews; }
            set { this.bulletinBoardOverviews = value; }
        }

        public IEnumerable<CalendarOverview> CalendarOverviews
        {
            get { return this.calendarOverviews; }
            set { this.calendarOverviews = value; }
        }
        
        public IEnumerable<FaqInfoOverview> FaqOverviews
        {
            get { return this.faqOverviews; }
            set { this.faqOverviews = value; }
        }

        public IEnumerable<OperationLogOverview> OperationLogOverviews
        {
            get { return this.operationLogOverviews; }
            set { this.operationLogOverviews = value; }
        }
        
        public IEnumerable<DailyReportOverview> DailyReportOverviews
        {
            get { return this.dailyReportOverviews; }
            set { this.dailyReportOverviews = value; }
        }

        public LinksInfoViewModel LinksInfo
        {
            get { return this.linksInfo; }
            set { this.linksInfo = value; }
        }

        public IEnumerable<ProblemInfoOverview> ProblemOverviews
        {
            get { return this.problemOverviews; }
            set { this.problemOverviews = value; }
        }

        public StatisticsOverview StatisticsOverviews
        {
            get { return this.statisticsOverviews; }
            set { this.statisticsOverviews = value; }
        }

        public IEnumerable<BusinessData.Models.Document.Output.DocumentOverview> DocumentOverviews
        {
            get { return this.documentOverviews; }
            set { this.documentOverviews = value; }
        }

        public IEnumerable<UserModuleOverview> UserModules { get; set; } 

        public int UserId { get; set; }
    }
}
