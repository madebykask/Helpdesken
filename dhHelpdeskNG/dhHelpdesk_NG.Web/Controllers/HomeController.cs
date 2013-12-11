using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Models;

namespace dhHelpdesk_NG.Web.Controllers
{
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
            _bulletinBoardService = bulletinBoardService;
            _calendarService = calendarService;
            _caseService = caseService;
            _customerService = customerService;
            _customerUserService = customerUserService;
            _userService = userService;
            _workingGroupService = workingGroupService;
        }

        public ActionResult Index()
        {
            var model = IndexInputViewModel();

            return View(model);
        }

        private HomeIndexViewModel IndexInputViewModel()
        {
            var user = _userService.GetUser(SessionFacade.CurrentUser.Id);

            var model = new HomeIndexViewModel
            {
                Cases = _caseService.GetCasesForStartPage(SessionFacade.CurrentCustomer.Id),
                CustomerUsers = _userService.GetCustomerUserForUser(SessionFacade.CurrentUser.Id),
                CustomerUsersForStart = _customerUserService.GetCustomerUsersForHomeIndexPage(SessionFacade.CurrentUser.Id),
                ForStartCaseCustomerUsers = _customerUserService.GetFinalListForCustomerUsersHomeIndexPage(SessionFacade.CurrentUser.Id),
                Customers = _customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}
