using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
using DH.Helpdesk.BusinessData.Models.Link.Output;
using DH.Helpdesk.Web.Models.Common;
using DH.Helpdesk.Web.Models.Customers;
using DH.Helpdesk.Web.Models.Link;

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
            IStatisticsService statisticsService)
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
                LinksInfo = GetLinksViewModel(_linkService.GetLinkOverviews(customersId, numberOfInfos)),
                ProblemOverviews = _problemService.GetProblemOverviews(customersId, numberOfInfos),
                StatisticsOverviews = _statisticsService.GetStatistics(customersId)
            };

            return model;
        }

        private LinksInfoViewModel GetLinksViewModel(IEnumerable<LinkOverview> linkOverviews)
        {
            var model = new LinksInfoViewModel();

            var customerGroups = linkOverviews.GroupBy(l => l.Customer_Id);
            foreach (var customerGroup in customerGroups)
            {
                var customer = new LinkCustomerGroupViewModel();
                var c = customerGroup.First();
                customer.CustomerId = c.Customer_Id;
                customer.CustomerName = c.CustomerName;

                var categoryNames = linkOverviews
                                    .Where(l => l.Customer_Id == customer.CustomerId)
                                    .Select(l => l.LinkGroupName)
                                    .Distinct();
                foreach (var categoryName in categoryNames)
                {
                    var category = new LinkCategoryGroupViewModel();
                    category.CategoryName = categoryName;
                    category.Links.AddRange(linkOverviews
                                            .Where(l => l.Customer_Id == customer.CustomerId && l.LinkGroupName == categoryName)
                                            .OrderBy(l => l.URLName));
                    customer.Categories.Add(category);
                }

                model.CustomerGroups.Add(customer);
            }

            return model;
        }
    }
}
