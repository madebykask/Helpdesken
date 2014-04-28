// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Controllers
{
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Users;
    using DH.Helpdesk.BusinessData.Models.Users.Input;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Link;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Customers;

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        /// The bulletin board service.
        /// </summary>
        private readonly IBulletinBoardService bulletinBoardService;

        /// <summary>
        /// The _calendar service.
        /// </summary>
        private readonly ICalendarService calendarService;

        /// <summary>
        /// The case service.
        /// </summary>
        private readonly ICaseService caseService;

        /// <summary>
        /// The customer service.
        /// </summary>
        private readonly ICustomerService customerService;

        /// <summary>
        /// The customer user service.
        /// </summary>
        private readonly ICustomerUserService customerUserService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// The service.
        /// </summary>
        private readonly IFaqService faqService;

        /// <summary>
        /// The _operation log service.
        /// </summary>
        private readonly IOperationLogService operationLogService;

        /// <summary>
        /// The daily report service.
        /// </summary>
        private readonly IDailyReportService dailyReportService;

        /// <summary>
        /// The link service.
        /// </summary>
        private readonly ILinkService linkService;

        /// <summary>
        /// The problem service.
        /// </summary>
        private readonly IProblemService problemService;

        /// <summary>
        /// The statistics service.
        /// </summary>
        private readonly IStatisticsService statisticsService;

        /// <summary>
        /// The link model factory.
        /// </summary>
        private readonly ILinkModelFactory linkModelFactory;

        /// <summary>
        /// The document service.
        /// </summary>
        private readonly IDocumentService documentService;

        /// <summary>
        /// The work context.
        /// </summary>
        private readonly IWorkContext workContext;

        /// <summary>
        /// The customer settings service.
        /// </summary>
        private readonly ISettingService customerSettingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="bulletinBoardService">
        /// The bulletin board service.
        /// </param>
        /// <param name="calendarService">
        /// The calendar service.
        /// </param>
        /// <param name="caseService">
        /// The case service.
        /// </param>
        /// <param name="customerService">
        /// The customer service.
        /// </param>
        /// <param name="customerUserService">
        /// The customer user service.
        /// </param>
        /// <param name="userService">
        /// The user service.
        /// </param>
        /// <param name="masterDataService">
        /// The master data service.
        /// </param>
        /// <param name="faqService">
        /// The FAQ service.
        /// </param>
        /// <param name="operationLogService">
        /// The operation log service.
        /// </param>
        /// <param name="dailyReportService">
        /// The daily report service.
        /// </param>
        /// <param name="linkService">
        /// The link service.
        /// </param>
        /// <param name="problemService">
        /// The problem service.
        /// </param>
        /// <param name="statisticsService">
        /// The statistics service.
        /// </param>
        /// <param name="linkModelFactory">
        /// The link model factory.
        /// </param>
        /// <param name="documentService">
        /// The document service.
        /// </param>
        /// <param name="workContext">
        /// The work context.
        /// </param>
        /// <param name="customerSettingsService">
        /// The customer settings service.
        /// </param>
        public HomeController(
            IBulletinBoardService bulletinBoardService,
            ICalendarService calendarService,
            ICaseService caseService,
            ICustomerService customerService,
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
            ISettingService customerSettingsService)
            : base(masterDataService)
        {
            this.bulletinBoardService = bulletinBoardService;
            this.calendarService = calendarService;
            this.caseService = caseService;
            this.customerService = customerService;
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
            this.customerSettingsService = customerSettingsService;
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Index()
        {
            var model = new HomeIndexViewModel();
            if (SessionFacade.CurrentUser != null)
            {
                model = this.IndexInputViewModel();
            }

            return this.View(model);
        }

        /// <summary>
        /// The update user module position.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
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

        /// <summary>
        /// The index input view model.
        /// </summary>
        /// <returns>
        /// The <see cref="HomeIndexViewModel"/>.
        /// </returns>
        private HomeIndexViewModel IndexInputViewModel()
        {
            var modules = this.workContext.User.Modules.ToArray();
            var model = new HomeIndexViewModel
            {
                CustomerUsers = this.userService.GetCustomerUserForUser(SessionFacade.CurrentUser.Id),
                ForStartCaseCustomerUsers = this.customerUserService.GetFinalListForCustomerUsersHomeIndexPage(SessionFacade.CurrentUser.Id),
                Customers = this.customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(CultureInfo.InvariantCulture)
                }).ToList(),
                UserModules = modules,
                UserId = SessionFacade.CurrentUser.Id
            };

            var customers = this.customerUserService.GetCustomerUsersForHomeIndexPage(SessionFacade.CurrentUser.Id);
            var customersIds = customers.Select(c => c.Customer_Id).ToArray();
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
                        model.BulletinBoardOverviews = this.bulletinBoardService.GetBulletinBoardOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.Calendar:
                        model.CalendarOverviews = this.calendarService.GetCalendarOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.Customers:                        
                        model.CustomersInfo = new CustomersInfoViewModel
                                            {
                                                Cases = this.caseService.GetCasesForStartPage(SessionFacade.CurrentCustomer.Id),
                                                CustomerUsersForStart = !module.NumberOfRows.HasValue ? 
                                                        customers : customers.Take(module.NumberOfRows.Value).ToArray()                                                                    
                                            };
                        break;
                    case Module.DailyReport:
                        model.DailyReportOverviews = this.dailyReportService.GetDailyReportOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.Documents:
                        model.DocumentOverviews = this.documentService.GetDocumentOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.Faq:
                        model.FaqOverviews = this.faqService.GetFaqByCustomers(customersIds, module.NumberOfRows);
                        break;
                    case Module.OperationalLog:
                        model.OperationLogOverviews = this.operationLogService.GetOperationLogOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.Problems:
                        model.ProblemOverviews = this.problemService.GetProblemOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.QuickLinks:
                        model.LinksInfo = this.linkModelFactory.GetLinksViewModel(this.linkService.GetLinkOverviews(customersIds, module.NumberOfRows));
                        break;
                    case Module.Statistics:
                        model.StatisticsOverviews = this.statisticsService.GetStatistics(customersIds);
                        break;
                }
            }

            return model;
        }
    }
}
