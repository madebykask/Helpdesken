namespace DH.Helpdesk.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Users;
    using DH.Helpdesk.BusinessData.Models.Users.Input;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Link;
    using DH.Helpdesk.Web.Models;

    public class HomeController : BaseController
    {
        private readonly IBulletinBoardService bulletinBoardService;

        private readonly ICalendarService calendarService;

        private readonly ICaseService caseService;

        private readonly ICustomerUserService customerUserService;

        private readonly IUserService userService;

        private readonly IFaqService faqService;

        private readonly IOperationLogService operationLogService;

        private readonly IDailyReportService dailyReportService;

        private readonly ILinkService linkService;

        private readonly IProblemService problemService;

        private readonly IStatisticsService statisticsService;

        private readonly ILinkModelFactory linkModelFactory;

        private readonly IDocumentService documentService;

        private readonly IWorkContext workContext;

        private readonly ICaseModelFactory caseModelFactory;

        private readonly IModulesInfoFactory modulesInfoFactory;

        private readonly IChangeService changeService;

        public HomeController(
            IBulletinBoardService bulletinBoardService,
            ICalendarService calendarService,
            ICaseService caseService,
            ICustomerUserService customerUserService,
            IUserService userService,
            IMasterDataService masterDataService,
            IFaqService faqService,
            IOperationLogService operationLogService,
            IDailyReportService dailyReportService,
            ILinkService linkService,
            IProblemService problemService,
            IStatisticsService statisticsService,
            ILinkModelFactory linkModelFactory,
            IDocumentService documentService,
            IWorkContext workContext, 
            ICaseModelFactory caseModelFactory, 
            IModulesInfoFactory modulesInfoFactory, 
            IChangeService changeService)
            : base(masterDataService)
        {
            this.bulletinBoardService = bulletinBoardService;
            this.calendarService = calendarService;
            this.caseService = caseService;
            this.customerUserService = customerUserService;
            this.userService = userService;
            this.faqService = faqService;
            this.operationLogService = operationLogService;
            this.dailyReportService = dailyReportService;
            this.linkService = linkService;
            this.problemService = problemService;
            this.statisticsService = statisticsService;
            this.linkModelFactory = linkModelFactory;
            this.documentService = documentService;
            this.workContext = workContext;
            this.caseModelFactory = caseModelFactory;
            this.modulesInfoFactory = modulesInfoFactory;
            this.changeService = changeService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new HomeIndexViewModel();
            if (SessionFacade.CurrentUser != null)
            {
                model = this.IndexInputViewModel();
            }

            return this.View(model);
        }

        [HttpPost]
        public ActionResult UpdateUserModulePosition(int userId, int moduleId, int position)
        {
            var modules = this.userService.GetUserModules(userId)
                        .Select(m => new UserModule
                        {
                            Id = m.Id,
                            User_Id = m.User_Id,
                            Module_Id = m.Module_Id,
                            isVisible = m.isVisible,
                            NumberOfRows = m.NumberOfRows,
                            Position = m.Position
                        })
                        .ToArray();

            var changedModule = modules.First(m => m.Module_Id == moduleId);

            var fromPrevColumn = modules.Where(m => m.Position / 100 == changedModule.Position / 100);
            var fromCurrentColumn = modules.Where(m => m.Position / 100 == position / 100);

            foreach (var prev in fromPrevColumn)
            {
                if (prev.Position > changedModule.Position)
                {
                    prev.Position--;
                }
            }

            foreach (var cur in fromCurrentColumn)
            {
                if (cur.Position >= position)
                {
                    cur.Position++;
                }
            }

            changedModule.Position = position;

            this.userService.UpdateUserModules(modules);
            this.workContext.User.Refresh();
            return new EmptyResult();
        }

        private HomeIndexViewModel IndexInputViewModel()
        {
            var modules = this.workContext.User.Modules.ToArray();
            var model = new HomeIndexViewModel
            {
                UserModules = modules,
                UserId = SessionFacade.CurrentUser.Id
            };

            var customers = this.customerUserService.GetCustomerUsersForHomeIndexPage(SessionFacade.CurrentUser.Id);
            var customersIds = customers.Select(c => c.Customer.Customer_Id).ToArray();
            var customerSettings = this.workContext.Customer.Settings;
            foreach (var module in modules)
            {
                if (!customerSettings.IsModuleOn((Module)module.Module_Id))
                {
                    continue;
                }

                if (!module.isVisible)
                {
                    continue;
                }

                switch ((Module)module.Module_Id)
                {
                    case Module.BulletinBoard:
                        model.BulletinBoardOverviews = this.bulletinBoardService.GetBulletinBoardOverviews(customersIds, module.NumberOfRows, true);
                        break;
                    case Module.Calendar:
                        model.CalendarOverviews = this.calendarService.GetCalendarOverviews(customersIds, module.NumberOfRows, true, true);
                        break;
                    case Module.Customers:

                        var showedCustomers = !module.NumberOfRows.HasValue
                                                  ? customers.Select(c => c.Customer.Customer_Id).ToArray()
                                                  : customers.Take(module.NumberOfRows.Value).Select(c => c.Customer.Customer_Id).ToArray();
                        var customerCases = this.caseService.GetCustomersCases(showedCustomers, this.workContext.User.UserId);                        
                        model.CustomersInfo = this.caseModelFactory.CreateCustomerCases(customerCases);
                        break;
                    case Module.DailyReport:
                        model.DailyReportOverviews = this.dailyReportService.GetDailyReportOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.Documents:
                        model.DocumentOverviews = this.documentService.GetDocumentOverviews(customersIds, module.NumberOfRows, true);
                        break;
                    case Module.Faq:
                        model.FaqOverviews = this.faqService.GetFaqByCustomers(customersIds, module.NumberOfRows, true);
                        break;
                    case Module.OperationalLog:
                        model.OperationLogOverviews = this.operationLogService.GetOperationLogOverviews(customersIds, module.NumberOfRows, true);
                        break;
                    case Module.Problems:
                        model.ProblemOverviews = this.problemService.GetProblemOverviews(customersIds, module.NumberOfRows, true);
                        break;
                    case Module.QuickLinks:
                        model.LinksInfo = this.linkModelFactory.GetLinksViewModel(this.linkService.GetLinkOverviews(customersIds, module.NumberOfRows, true));
                        break;
                    case Module.Statistics:
                        model.StatisticsOverviews = this.statisticsService.GetStatistics(
                                                                        customersIds, 
                                                                        this.workContext.User.UserId);
                        break;
                    case Module.ChangeManagement:
                        var changesCustomers = customers.Where(c => c.Settings.IsModuleOn(Module.ChangeManagement));
                        if (module.NumberOfRows.HasValue)
                        {
                            changesCustomers = changesCustomers.Take(module.NumberOfRows.Value);
                        }

                        var customerChanges = this.changeService.GetCustomerChanges(
                                            changesCustomers.Select(c => c.Customer.Customer_Id).ToArray(),
                                            SessionFacade.CurrentUser.Id);

                        model.CustomerChanges = this.modulesInfoFactory.GetCustomerChangesModel(customerChanges);
                        break;
                    case Module.Cases:
                        var myCases = this.caseService.GetMyCases(this.workContext.User.UserId, module.NumberOfRows);                       
                        model.MyCases = this.modulesInfoFactory.GetMyCasesModel(myCases);
                      
                        break;
                }
            }

            return model;
        }
    }
}
