using System.Collections;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Statistics.Output;
using DH.Helpdesk.Common.Enums;

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
    using System;

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

        private readonly ILinkModelFactory linkModelFactory;

        private readonly IDocumentService documentService;

        private readonly IWorkContext workContext;

        private readonly ICaseModelFactory caseModelFactory;

        private readonly IModulesInfoFactory modulesInfoFactory;

        private readonly IChangeService changeService;

        private readonly ILanguageService languageService;

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
            ILinkModelFactory linkModelFactory,
            IDocumentService documentService,
            IWorkContext workContext, 
            ICaseModelFactory caseModelFactory, 
            IModulesInfoFactory modulesInfoFactory, 
            IChangeService changeService,
            ILanguageService languageService)
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
            this.linkModelFactory = linkModelFactory;
            this.documentService = documentService;
            this.workContext = workContext;
            this.caseModelFactory = caseModelFactory;
            this.modulesInfoFactory = modulesInfoFactory;
            this.changeService = changeService;
            this.languageService = languageService;
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
            var customerIdsAll = customers.Select(c => c.Customer.Customer_Id).ToArray();


            if (!customers.Any())
            {
                return model;
            }
            //Todo - Ta första bästa kund om det finns flera kunder
            var customersSettings = this.userService.GetUserCustomersSettings(SessionFacade.CurrentUser.Id);
            var currentCustomerSettings = customersSettings.FirstOrDefault(s => s.CustomerId == this.workContext.Customer.CustomerId);
            var bulletinBoardWGRestriction = currentCustomerSettings.BulletinBoardWGRestriction;
            var calendarWGRestriction = currentCustomerSettings.CalendarWGRestriction;
            var currentUser = this.userService.GetUser(SessionFacade.CurrentUser.Id);

            if (string.IsNullOrEmpty(SessionFacade.CurrentLanguageCode))
            {
                var curLanguageId = 0;
                if (SessionFacade.CurrentLanguageId > 0)
                    curLanguageId = SessionFacade.CurrentLanguageId;
                else                     
                    curLanguageId = SessionFacade.CurrentUser != null? SessionFacade.CurrentUser.LanguageId : 0;

                if (curLanguageId == 0)
                    throw new ArgumentNullException("Session timeout: Please refresh the page and try again!");
                else
                {
                    var language = this.languageService.GetLanguage(curLanguageId);
                    SessionFacade.CurrentLanguageCode = language.LanguageID;
                }                   
            }

            foreach (var module in modules)
            {
                if (!module.isVisible)
                {
                    continue;
                }

                if (!customersSettings.Any(s => s.IsModuleOn((Module)module.Module_Id)))
                {
                    continue;
                }

                var customersIds = module.NumberOfRows.HasValue
                                                  ? customers.Take(module.NumberOfRows.Value).Select(c => c.Customer.Customer_Id).ToArray()
                                                  : customers.Select(c => c.Customer.Customer_Id).ToArray();
                var queryRowsNumber = module.NumberOfRows + 1 ?? module.NumberOfRows;

                switch ((Module)module.Module_Id)
                {
                    case Module.BulletinBoard:
                        if (SessionFacade.CurrentUser.UserGroupId == (int)BusinessData.Enums.Admin.Users.UserGroup.User ||
                            SessionFacade.CurrentUser.UserGroupId == (int)BusinessData.Enums.Admin.Users.UserGroup.Administrator)
                            model.BulletinBoardModel.Items = 
                                this.bulletinBoardService.GetBulletinBoardOverviews(customerIdsAll, queryRowsNumber, true, bulletinBoardWGRestriction).ToArray();
                        else
                            model.BulletinBoardModel.Items = 
                                this.bulletinBoardService.GetBulletinBoardOverviews(customerIdsAll, queryRowsNumber, true).ToArray();
                        if (module.NumberOfRows.HasValue)
                        {
                            model.BulletinBoardModel.ShowMore = model.BulletinBoardModel.Items.Count() > module.NumberOfRows.Value;
                            model.BulletinBoardModel.Items = model.BulletinBoardModel.Items.Take(module.NumberOfRows.Value);
                        }
                        break;
                    case Module.Calendar:
                        if (SessionFacade.CurrentUser.UserGroupId == (int)BusinessData.Enums.Admin.Users.UserGroup.User ||
                            SessionFacade.CurrentUser.UserGroupId == (int)BusinessData.Enums.Admin.Users.UserGroup.Administrator)
                            model.CalendarModel.Items = this.calendarService.GetCalendarOverviews(customerIdsAll, queryRowsNumber, true, true, true, calendarWGRestriction)
                                                      .OrderByDescending(c => c.CalendarDate)
                                                      .ToArray();
                        else
                            model.CalendarModel.Items = this.calendarService.GetCalendarOverviews(customerIdsAll, queryRowsNumber, true, true)
                                                      .OrderByDescending(c => c.CalendarDate)
                                                      .ToArray();
                        if (module.NumberOfRows.HasValue)
                        {
                            model.CalendarModel.ShowMore = model.CalendarModel.Items.Count() > module.NumberOfRows.Value;
                            model.CalendarModel.Items = model.CalendarModel.Items.Take(module.NumberOfRows.Value);
                        }
                        break;
                    case Module.Customers:
                        var customerCases = this.caseService.GetCustomersCases(customersIds, this.workContext.User.UserId);
                        model.CustomersInfo = this.caseModelFactory.CreateCustomerCases(customerCases);
                        break;
                    case Module.DailyReport:
                        model.DailyReportModel.Items = this.dailyReportService.GetDailyReportOverviews(customerIdsAll, queryRowsNumber)
                            .OrderByDescending(d => d.CreatedDate)
                            .ToArray();
                        if (module.NumberOfRows.HasValue)
                        {
                            model.DailyReportModel.ShowMore = model.DailyReportModel.Items.Count() > module.NumberOfRows.Value;
                            model.DailyReportModel.Items = model.DailyReportModel.Items.Take(module.NumberOfRows.Value);
                        }
                        break;
                    case Module.Documents:
                        model.DocumentsModel.Items = this.documentService.GetDocumentOverviews(customerIdsAll, queryRowsNumber, true)
                            .OrderByDescending(d => d.ChangedDate)
                            .ToArray();
                        if (module.NumberOfRows.HasValue)
                        {
                            model.DocumentsModel.ShowMore = model.DocumentsModel.Items.Count() > module.NumberOfRows.Value;
                            model.DocumentsModel.Items = model.DocumentsModel.Items.Take(module.NumberOfRows.Value);
                        }
                        break;
                    case Module.Faq:
                        model.FaqModel.Items = this.faqService.GetFaqByCustomers(customerIdsAll, queryRowsNumber, true).ToArray();
                        if (module.NumberOfRows.HasValue)
                        {
                            model.FaqModel.ShowMore = model.FaqModel.Items.Count() > module.NumberOfRows.Value;
                            model.FaqModel.Items = model.FaqModel.Items.Take(module.NumberOfRows.Value);
                        }
                        break;
                    case Module.OperationalLog:
                        model.OperationLogModel.Items = this.operationLogService.GetOperationLogOverviews(customerIdsAll, queryRowsNumber, true).ToArray();
                        if (module.NumberOfRows.HasValue)
                        {
                            model.OperationLogModel.ShowMore = model.OperationLogModel.Items.Count() > module.NumberOfRows.Value;
                            model.OperationLogModel.Items = model.OperationLogModel.Items.Take(module.NumberOfRows.Value);
                        }
                        break;
                    case Module.Problems:
                        model.ProblemModel.Items = this.problemService.GetProblemOverviews(customerIdsAll, queryRowsNumber, true).ToArray();
                        if (module.NumberOfRows.HasValue)
                        {
                            model.ProblemModel.ShowMore = model.ProblemModel.Items.Count() > module.NumberOfRows.Value;
                            model.ProblemModel.Items = model.ProblemModel.Items.Take(module.NumberOfRows.Value);
                        }
                        break;
                    case Module.QuickLinks:
                        if (SessionFacade.CurrentUser.UserGroupId == (int)BusinessData.Enums.Admin.Users.UserGroup.User ||
                            SessionFacade.CurrentUser.UserGroupId == (int)BusinessData.Enums.Admin.Users.UserGroup.Administrator)
                            model.LinksInfo = this.linkModelFactory.GetLinksViewModel(this.linkService.GetLinkOverviewsForStartPage(customerIdsAll, module.NumberOfRows, true, 
                                customersSettings.Where(c => c.QuickLinkWGRestriction).Select(c => c.CustomerId).ToArray()));
                        else
                            model.LinksInfo = this.linkModelFactory.GetLinksViewModel(this.linkService.GetLinkOverviewsForStartPage(customerIdsAll, module.NumberOfRows, true));
                        break;
                    case Module.Statistics:
                        model.StatisticsOverviews = new StatisticsOverview();
                        break;
                    case Module.ChangeManagement:
                        var customerChanges = this.changeService.GetCustomerChanges(customersIds, SessionFacade.CurrentUser.Id);
                        var showIcon = (currentCustomerSettings != null) && currentCustomerSettings.IsModuleOn(Module.ChangeManagement);
                        model.CustomerChanges = this.modulesInfoFactory.GetCustomerChangesModel(customerChanges, showIcon);
                        break;
                    case Module.Cases:
                        var myCases = this.caseService.GetMyCases(this.workContext.User.UserId, queryRowsNumber);
                        model.MyCases = this.modulesInfoFactory.GetMyCasesModel(myCases);
                        if (module.NumberOfRows.HasValue)
                        {
                            model.MyCases.ShowMore = model.MyCases.Cases.Count() > module.NumberOfRows.Value;
                            model.MyCases.Cases = model.MyCases.Cases.Take(module.NumberOfRows.Value).ToArray();
                        }
                        break;
                }
            }

            return model;
        }

        //diagnostic action
        public JsonResult _Ctx()
        {
            var winIdentity = WindowsIdentity.GetCurrent();

            return Json(new
            {
                HttpContextUserName = ControllerContext.RequestContext.HttpContext.User.Identity.Name,
                HttpContextUserType = ControllerContext.RequestContext.HttpContext.User.Identity.GetType().Name,
                WinIdentity = winIdentity != null ? $"{winIdentity.Name} | {winIdentity.AuthenticationType}" : "None",
                ThreadUser = Thread.CurrentPrincipal?.Identity?.Name ?? "None"
            }, JsonRequestBehavior.AllowGet);
            
        }
    }
}
