using DH.Helpdesk.Web.Models.Common;
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
            ILinkService linkService)
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
        }

        public ActionResult Index()
        {
            var model = new HomeIndexViewModel(); 
            if (SessionFacade.CurrentUser != null)
                model = this.IndexInputViewModel();
            return this.View(model);
        }

        private HomeIndexViewModel IndexInputViewModel()
        {
            var user = this._userService.GetUser(SessionFacade.CurrentUser.Id);
            var customers = _customerUserService.GetCustomerUsersForHomeIndexPage(SessionFacade.CurrentUser.Id);
            var customersId = customers.Select(c => c.Customer_Id).ToArray();
            var customersInfo = new CustomersInfoViewModel()
            {
                Cases = _caseService.GetCasesForStartPage(SessionFacade.CurrentCustomer.Id),
                CustomerUsersForStart = customers
            };

            //it's temporary
            const int numberOfInfos = 3;

            var model = new HomeIndexViewModel
            {
                CustomerUsers = this._userService.GetCustomerUserForUser(SessionFacade.CurrentUser.Id),
                ForStartCaseCustomerUsers = this._customerUserService.GetFinalListForCustomerUsersHomeIndexPage(SessionFacade.CurrentUser.Id),
                Customers = this._customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                CustomersInfo = customersInfo,
                BulletinBoardOverviews = _bulletinBoardService.GetBulletinBoardOverviews(customersId, numberOfInfos),
                CalendarOverviews = _calendarService.GetCalendarOverviews(customersId, numberOfInfos),
                FaqOverviews = _faqService.GetFaqByCustomers(customersId, numberOfInfos),
                OperationLogOverviews = _operationLogService.GetOperationLogOverviews(customersId, numberOfInfos),
                DailyReportOverviews = _dailyReportService.GetDailyReportOverviews(customersId, numberOfInfos),
//                LinksInfo = _linkService.GetLinkOverviews(customersId, numberOfInfos)
            };

            return model;
        }
    }
}
