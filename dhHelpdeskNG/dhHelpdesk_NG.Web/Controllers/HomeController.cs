using System;
using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.Users.Input;
using DH.Helpdesk.Web.Infrastructure.ModelFactories.Link;
using DH.Helpdesk.Web.Infrastructure.WorkContext;
using DH.Helpdesk.Web.Models.Customers;

namespace DH.Helpdesk.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models;

    public class HomeController : BaseController
    {
        private readonly IBulletinBoardService _bulletinBoardService;
        private readonly ICalendarService _calendarService;
        private readonly ICaseService _caseService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IFaqService _faqService;
        private readonly IOperationLogService _operationLogService;
        private readonly IDailyReportService _dailyReportService;
        private readonly ILinkService _linkService;
        private readonly IProblemService _problemService;
        private readonly IStatisticsService _statisticsService;
        private readonly ILinkModelFactory _linkModelFactory;
        private readonly IDocumentService _documentService;
        private readonly IWorkContext _workContext;

        public HomeController(
            IBulletinBoardService bulletinBoardService,
            ICalendarService calendarService,
            ICaseService caseService,
            ICustomerService customerService,
            ICustomerUserService customerUserService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService,
            IFaqService faqService,
            IOperationLogService operationLogService,
            IDailyReportService dailyReportService,
            ILinkService linkService,
            IProblemService problemService,
            IStatisticsService statisticsService,
            ILinkModelFactory linkModelFactory,
            IDocumentService documentService,
            IWorkContext workContext)
            : base(masterDataService)
        {
            this._bulletinBoardService = bulletinBoardService;
            this._calendarService = calendarService;
            this._caseService = caseService;
            this._customerService = customerService;
            this._customerUserService = customerUserService;
            this._userService = userService;
            this._workingGroupService = workingGroupService;
            _faqService = faqService;
            _operationLogService = operationLogService;
            _dailyReportService = dailyReportService;
            _linkService = linkService;
            _problemService = problemService;
            _statisticsService = statisticsService;
            _linkModelFactory = linkModelFactory;
            _documentService = documentService;
            _workContext = workContext;
        }

        public ActionResult Index()
        {
            var model = new HomeIndexViewModel();
            if (SessionFacade.CurrentUser != null)
            {
                _userService.InitializeUserModules(_workContext.User.Modules);
                model = this.IndexInputViewModel();
            }
            return this.View(model);
        }

        private HomeIndexViewModel IndexInputViewModel()
        {
            var modules = _workContext.User.Modules;
            var model = new HomeIndexViewModel
            {
                CustomerUsers = this._userService.GetCustomerUserForUser(SessionFacade.CurrentUser.Id),
                ForStartCaseCustomerUsers = this._customerUserService.GetFinalListForCustomerUsersHomeIndexPage(SessionFacade.CurrentUser.Id),
                Customers = this._customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                UserModules = modules,
                UserId = SessionFacade.CurrentUser.Id
            };

            var customers = _customerUserService.GetCustomerUsersForHomeIndexPage(SessionFacade.CurrentUser.Id);
            var customersIds = customers.Select(c => c.Customer_Id).ToArray();
            foreach (var module in modules)
            {   
                if(!module.isVisible)
                    continue;
                switch ((Module)module.Module_Id)
                {
                    case Module.BulletinBoard:
                        model.BulletinBoardOverviews = _bulletinBoardService.GetBulletinBoardOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.Calendar:
                        model.CalendarOverviews = _calendarService.GetCalendarOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.Customers:                        
                        model.CustomersInfo = new CustomersInfoViewModel()
                                            {
                                                Cases = _caseService.GetCasesForStartPage(SessionFacade.CurrentCustomer.Id),
                                                CustomerUsersForStart = !module.NumberOfRows.HasValue ? 
                                                        customers : customers.Take(module.NumberOfRows.Value).ToArray()                                                                    
                                            };
                        break;
                    case Module.DailyReport:
                        model.DailyReportOverviews = _dailyReportService.GetDailyReportOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.Documents:
                        model.DocumentOverviews = _documentService.GetDocumentOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.Faq:
                        model.FaqOverviews = _faqService.GetFaqByCustomers(customersIds, module.NumberOfRows);
                        break;
                    case Module.OperationalLog:
                        model.OperationLogOverviews = _operationLogService.GetOperationLogOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.Problems:
                        model.ProblemOverviews = _problemService.GetProblemOverviews(customersIds, module.NumberOfRows);
                        break;
                    case Module.QuickLinks:
                        model.LinksInfo = _linkModelFactory.GetLinksViewModel(_linkService.GetLinkOverviews(customersIds, module.NumberOfRows));
                        break;
                    case Module.Statistics:
                        model.StatisticsOverviews = _statisticsService.GetStatistics(customersIds);
                        break;
                }
            }

            return model;
        }

        [HttpPost]
        public ActionResult UpdateUserModulePosition(int userId, int moduleId, int position)
        {
            var modules = _userService.GetUserModules(userId)
                        .Select(m => new UserModule()
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
                    prev.Position--;
            }

            foreach (var cur in fromCurrentColumn)
            {
                if (cur.Position >= position)
                    cur.Position++;
            }

            changedModule.Position = position;
            
            _userService.UpdateUserModules(modules);
            return new EmptyResult();
        }
    }
}
