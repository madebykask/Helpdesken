using DH.Helpdesk.Web.Models.Widgets;

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
    using DH.Helpdesk.Web.Models.Link;

    public class HomeIndexViewModel
    {

        public CustomerCasesModel CustomersInfo { get; set; } = new CustomerCasesModel();

        public CustomerChangesModel CustomerChanges { get; set; } = new CustomerChangesModel();

        public MyCasesModel MyCases // User is Responsible 
        { get; set; } = new MyCasesModel();

        public MyCasesModel AdminCases // User is Adminstrator
        { get; set; } = new MyCasesModel();

        public WidgetModel<BulletinBoardOverview> BulletinBoardModel { get; set; } = new WidgetModel<BulletinBoardOverview>();

        public WidgetModel<CalendarOverview> CalendarModel { get; set; } = new WidgetModel<CalendarOverview>();

        public WidgetModel<FaqInfoOverview> FaqModel { get; set; } = new WidgetModel<FaqInfoOverview>();

        public WidgetModel<OperationLogOverview> OperationLogModel { get; set; } = new WidgetModel<OperationLogOverview>();

        public IEnumerable<DailyReportOverview> DailyReportOverviews { get; set; } = new DailyReportOverview[] { };

        public LinksInfoViewModel LinksInfo { get; set; } = new LinksInfoViewModel();

        public WidgetModel<ProblemInfoOverview> ProblemModel { get; set; } = new WidgetModel<ProblemInfoOverview>();

        public StatisticsOverview StatisticsOverviews { get; set; } = new StatisticsOverview();

        public WidgetModel<BusinessData.Models.Document.Output.DocumentOverview> DocumentsModel { get; set; } = new WidgetModel<BusinessData.Models.Document.Output.DocumentOverview>();

        public IEnumerable<UserModuleOverview> UserModules { get; set; } 

        public int UserId { get; set; }
    }
}
