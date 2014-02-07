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

        public HomeController(
            IBulletinBoardService bulletinBoardService,
            ICalendarService calendarService,
            ICaseService caseService,
            ICustomerService customerService,
            ICustomerUserService customerUserService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._bulletinBoardService = bulletinBoardService;
            this._calendarService = calendarService;
            this._caseService = caseService;
            this._customerService = customerService;
            this._customerUserService = customerUserService;
            this._userService = userService;
            this._workingGroupService = workingGroupService;
        }

        public ActionResult Index()
        {
            var model = this.IndexInputViewModel();

            return this.View(model);
        }

        private HomeIndexViewModel IndexInputViewModel()
        {
            var user = this._userService.GetUser(SessionFacade.CurrentUser.Id);

            var model = new HomeIndexViewModel
            {
                Cases = this._caseService.GetCasesForStartPage(SessionFacade.CurrentCustomer.Id),
                CustomerUsers = this._userService.GetCustomerUserForUser(SessionFacade.CurrentUser.Id),
                CustomerUsersForStart = this._customerUserService.GetCustomerUsersForHomeIndexPage(SessionFacade.CurrentUser.Id),
                ForStartCaseCustomerUsers = this._customerUserService.GetFinalListForCustomerUsersHomeIndexPage(SessionFacade.CurrentUser.Id),
                Customers = this._customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}
