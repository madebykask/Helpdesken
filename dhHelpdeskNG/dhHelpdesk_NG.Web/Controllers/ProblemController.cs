using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.FinishingCause;
    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Infrastructure.Filters.Problems;
    using DH.Helpdesk.Web.Models.Problem;

    public class ProblemsController : BaseController
    {
        private readonly IProblemService problemService;
        private readonly IProblemLogService problemLogService;
        private readonly IFinishingCauseService finishingCauseService;
        private readonly ISettingService _settingService;
        private readonly ICaseService caseService;
        private readonly IMasterDataService _masterDataService;
        private readonly ICustomerService _customerService;

        private readonly IUserService userService;

        public ProblemsController(
                IProblemService problemService,
                IMasterDataService masterDataService,
                IUserService userService,
                IProblemLogService problemLogService,
                ICaseService caseService,
                ISettingService settingService,
                IFinishingCauseService finishingCauseService,
                ICustomerService customerService)
            : base(masterDataService)
        {
            this.problemService = problemService;
            this.userService = userService;
            this.problemLogService = problemLogService;
            this.caseService = caseService;
            this.finishingCauseService = finishingCauseService;
            this._settingService = settingService;
            _masterDataService = masterDataService;
            _customerService = customerService;
        }

        // todo refactor
        public DropDownWithSubmenusItem CauseToDropDownItem(FinishingCauseOverview causes)
        {
            var item = new DropDownWithSubmenusItem(
                causes.Name, causes.Id.ToString(CultureInfo.InvariantCulture));

            if (causes.ChildFinishingCauses.Any())
            {
                var subitems =
                    causes.ChildFinishingCauses.Select(
                        this.CauseToDropDownItem).ToList();

                item.Subitems.AddRange(subitems);
            }

            return item;
        }

        public ProblemOutputModel MapProblemOverviewToOutputModel(ProblemOverview problemOverview, bool isFirstNameFirst)
        {
            return new ProblemOutputModel
                       {
                           Id = problemOverview.Id,
                           Name = problemOverview.Name,
                           Description = problemOverview.Description,
                           ProblemNumber = problemOverview.ProblemNumber,
                           ResponsibleUserName = (isFirstNameFirst? string.Format("{0} {1}", problemOverview.ResponsibleUserName, problemOverview.ResponsibleUserSurName): 
                                                                    string.Format("{0} {1}", problemOverview.ResponsibleUserSurName, problemOverview.ResponsibleUserName)),
                           State = problemOverview.FinishingDate.HasValue ? "Finished" : "Active"
                       };
        }

        public static ProblemEditModel MapProblemOverviewToEditOutputModel(ProblemOverview problemOverview)
        {
            return new ProblemEditModel
            {
                Id = problemOverview.Id,
                Name = problemOverview.Name,
                Description = problemOverview.Description,
                ProblemNumber = problemOverview.ProblemNumber,
                InventoryNumber = problemOverview.InventoryNumber,
                ResponsibleUserId = problemOverview.ResponsibleUserId,
                ShowOnStartPage = problemOverview.ShowOnStartPage,
                IsFinished = problemOverview.FinishingDate.HasValue,
                IsExistConnectedCases = problemOverview.IsExistConnectedCases
            };
        }

        public static LogOutputModel MapLogs(ProblemLogOverview arg, bool isFirstName)
        {
            return new LogOutputModel
                       {
                           Id = arg.Id,
                           Date = DateTime.SpecifyKind(arg.ChangedDate, DateTimeKind.Utc).ToShortDateString(),
                           LogNote = arg.LogText,
                           RegistratedBy = (isFirstName? string.Format("{0} {1}", arg.ChangedByUserName, arg.ChangedByUserSurName) : string.Format("{0} {1}", arg.ChangedByUserSurName, arg.ChangedByUserName))
                       };
        }

        public static LogEditModel MapLogs(NewProblemLogDto arg)
        {
            return new LogEditModel
            {
                Id = arg.Id,
                FinishingDate = arg.FinishingDate,
                LogText = arg.LogText,
                InternNotering = arg.ShowOnCase == 1,
                ExternNotering = arg.ShowOnCase == 2,
                FinishConnectedCases = arg.FinishConnectedCases == 1,
                ProblemId = arg.ProblemId,
                FinishingCauseId = arg.FinishingCauseId
            };
        }

        public static CaseOutputModel MapCase(Case arg)
        {
            var subStateName = string.Empty;

            if (arg.StateSecondary != null)
                subStateName = arg.StateSecondary.Name;

            return new CaseOutputModel
            {
                Id = arg.Id,
                CaseNumber = arg.CaseNumber.ToString(),
                Caption = arg.Caption,
                RegistrationDate = arg.RegTime,
                WatchDate = arg.WatchDate,
                CaseType = arg.CaseType.Name,
                SubState = subStateName,
            };
        }

        public ActionResult Index()
        {
            var filter = SessionFacade.FindPageFilters<ProblemFilter>(PageName.Problems) ?? new ProblemFilter { EntityStatus = EntityStatus.Active };
            var problems = this.problemService.GetCustomerProblems(SessionFacade.CurrentCustomer.Id, filter.EntityStatus);
            var customerId = SessionFacade.CurrentCustomer.Id;
            var settings = this._settingService.GetCustomerSetting(customerId);

            var problemOutputModels = problems.Select(t=> MapProblemOverviewToOutputModel(t, settings.IsUserFirstLastNameRepresentation==1)).ToList();

            // ToDo Artem: do not use ViewModel naming in classic MVC applications.
            var viewModel = new ProblemOutputViewModel { Problems = problemOutputModels, Show = (Enums.Show)filter.EntityStatus };

            return this.View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Search(EntityStatus show)
        {
            var problems = this.problemService.GetCustomerProblems(SessionFacade.CurrentCustomer.Id, show);
            var settings = this._settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            var problemOutputModels = problems.Select(t=> MapProblemOverviewToOutputModel(t, settings.IsUserFirstLastNameRepresentation == 1)).ToList();

            SessionFacade.SavePageFilters(PageName.Problems, new ProblemFilter { EntityStatus = show });

            return this.PartialView("ProblemGrid", problemOutputModels);
        }

        public ActionResult Problem(int id)
        {
            SessionFacade.ActiveTab = "#fragment-1";
            var vm = this.CreateProblemEditViewModel(id);

            return this.View(vm);
        }

        public ActionResult ProblemActiveLog(int id)
        {
            SessionFacade.ActiveTab = "#fragment-2";
            var vm = this.CreateProblemEditViewModel(id);

            return this.View("Problem", vm);
        }

        public ActionResult NewProblem()
        {
            var users = this.userService.GetUsers(SessionFacade.CurrentCustomer.Id);

            var userOutputModels = users.Select(x => new SelectListItem { Text = string.Format("{0} {1}", x.FirstName, x.SurName), Value = x.Id.ToString(CultureInfo.InvariantCulture) }).ToList();

            var viewModel = new ProblemEditViewModel { Problem = new ProblemEditModel(), Users = userOutputModels };

            return this.View(viewModel);
        }

        [HttpPost]
        public ActionResult AddProblemWithLog(ProblemEditModel problem, LogEditModel log)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            int showInCaseLog;

            if (log.ExternNotering == false && log.InternNotering == false)
            {
                showInCaseLog = 0;
            }
            else
            {
                showInCaseLog = log.InternNotering ? 1 : 2;
            }

            var problemDto = new NewProblemDto(
                problem.Name,
                problem.Description,
                problem.ResponsibleUserId,
                problem.InventoryNumber,
                problem.ShowOnStartPage,
                SessionFacade.CurrentCustomer.Id,
                null);

            var logDto = new NewProblemLogDto(
                SessionFacade.CurrentUser.Id,
                log.LogText,
                showInCaseLog,
                log.FinishingCauseId,
                log.FinishingDate,
                log.FinishConnectedCases ? 1 : 0);

            this.problemService.AddProblem(problemDto, logDto);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Add(ProblemEditModel problem)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var problemDto = new NewProblemDto(
                problem.Name,
                problem.Description,
                problem.ResponsibleUserId,
                problem.InventoryNumber,
                problem.ShowOnStartPage,
                SessionFacade.CurrentCustomer.Id,
                null);

            this.problemService.AddProblem(problemDto);

            return this.RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            this.problemService.DeleteProblem(id);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Save(ProblemEditModel problem)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var problemDto = new NewProblemDto(problem.Name, problem.Description, problem.ResponsibleUserId, problem.InventoryNumber, problem.ShowOnStartPage, SessionFacade.CurrentCustomer.Id, null)
                                 {
                                     Id
                                         =
                                         problem
                                         .Id
                                 };

            this.problemService.UpdateProblem(problemDto);

            return this.RedirectToAction("Index");
        }

        public ActionResult Activate(int id)
        {
            this.problemService.ActivateProblem(id);
            return this.RedirectToAction("Problem", new { id });
        }

        public PartialViewResult LogForNewProblem()
        {
            var causes = this.finishingCauseService.GetFinishingCausesWithChilds(SessionFacade.CurrentCustomer.Id).Select(this.CauseToDropDownItem).ToList();
            var causesTree = new DropDownWithSubmenusContent(causes, null);
            return this.PartialView("_InputLog", new LogEditModel { FinishingCauses = causesTree });
        }

        public PartialViewResult Log(int id)
        {
            var log = MapLogs(this.problemLogService.GetProblemLog(id));
            var causes = this.finishingCauseService.GetFinishingCausesWithChilds(SessionFacade.CurrentCustomer.Id).Select(this.CauseToDropDownItem).OrderBy(x => x.Name).ToList();
            var finishingCauseId = log.FinishingCauseId.HasValue
                                       ? log.FinishingCauseId.ToString()
                                       : null;
            var causesTree = new DropDownWithSubmenusContent(causes, finishingCauseId);
            log.FinishingCauses = causesTree;
            return this.PartialView("EditLog", log);
        }

        public PartialViewResult NewLog(int problemId)
        {
            var causes = this.finishingCauseService.GetFinishingCausesWithChilds(SessionFacade.CurrentCustomer.Id).Select(this.CauseToDropDownItem).ToList();
            var causesTree = new DropDownWithSubmenusContent(causes, null);
            return this.PartialView("NewLog", new LogEditModel { FinishingCauses = causesTree });
        }

        [HttpPost]
        public ActionResult AddLog(LogEditModel log)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            int showInCaseLog;
            if (log.ExternNotering == false && log.InternNotering == false)
            {
                showInCaseLog = 0;
            }
            else
            {
                showInCaseLog = log.InternNotering ? 1 : 2;
            }

            var logDto = new NewProblemLogDto(
                SessionFacade.CurrentUser.Id,
                log.LogText,
                showInCaseLog,
                log.FinishingCauseId,
                log.FinishingDate,
                log.FinishConnectedCases ? 1 : 0) { ProblemId = log.ProblemId };

            this.problemLogService.AddLog(logDto);
//            SendProblemLogEmail(log);
            return this.RedirectToAction("ProblemActiveLog", new { id = log.ProblemId });
        }

        [HttpPost]
        public ActionResult SaveLog(LogEditModel log)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            int showInCaseLog;
            if (log.ExternNotering == false && log.InternNotering == false)
            {
                showInCaseLog = 0;
            }
            else
            {
                showInCaseLog = log.InternNotering ? 1 : 2;
            }

            var logDto = new NewProblemLogDto(
                SessionFacade.CurrentUser.Id,
                log.LogText,
                showInCaseLog,
                log.FinishingCauseId,
                log.FinishingDate,
                log.FinishConnectedCases ? 1 : 0) { ProblemId = log.ProblemId, Id = log.Id };

            this.problemLogService.UpdateLog(logDto);
//            SendProblemLogEmail(log);
            return this.RedirectToAction("ProblemActiveLog", new { id = log.ProblemId });
        }

        public ActionResult DeleteLog(int problemId, int logId)
        {
            this.problemLogService.DeleteLog(logId);
            return this.RedirectToAction("ProblemActiveLog", new { id = problemId });
        }

        public ActionResult ResetLog(string s)
        {
            return null;
        }

        #region Private

        private ProblemEditViewModel CreateProblemEditViewModel(int id)
        {
            var problem = this.problemService.GetProblem(id);
            var logs = this.problemLogService.GetProblemLogs(id);

            // todo!!!
            var cases = this.caseService.GetProblemCases(SessionFacade.CurrentCustomer.Id, id);
            var setting = this._settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            var isFirstName = (setting.IsUserFirstLastNameRepresentation == 1);

            var users = this.userService.GetUsers(SessionFacade.CurrentCustomer.Id);
            var problemOutputModel = MapProblemOverviewToEditOutputModel(problem);

            var outputLogs = logs.Select(x=> MapLogs(x, isFirstName)).OrderBy(x => x.Date).ToList();

            var outputCases = cases.Select(MapCase).ToList();
            
            
            var userOutputModels = users.Select(x => new SelectListItem 
                { 
                    Text = (isFirstName? string.Format("{0} {1}", x.FirstName, x.SurName) : string.Format("{0} {1}", x.SurName, x.FirstName)), 
                    Value = x.Id.ToString(CultureInfo.InvariantCulture) 
                }).OrderBy(x=> x.Text).ToList();

            return new ProblemEditViewModel { Problem = problemOutputModel, Users = userOutputModels, Logs = outputLogs, Cases = outputCases };
        }

        private void SendProblemLogEmail(LogEditModel log)
        {
            if (log.ExternNotering || log.FinishConnectedCases)
            {
                var cases = caseService.GetProblemCases(log.ProblemId);
                foreach (var c in cases)
                {
                    var userTimeZone = TimeZoneInfo.Local;
                    var basePath = _masterDataService.GetFilePath(c.Customer_Id);
                    var caseHistoryId = problemService.GetCaseHistoryId(c.Id, log.ProblemId);

                    var customer = _customerService.GetCustomer(c.Customer_Id);
                    var caseMailSetting = new CaseMailSetting(string.Empty, customer.HelpdeskEmail, RequestExtension.GetAbsoluteUrl(), 1)
                    {
                        DontSendMailToNotifier = false,
                    };

                    var mailSenders = new MailSenders { SystemEmail = caseMailSetting.HelpdeskMailFromAdress };
                    caseMailSetting.CustomeMailFromAddress = mailSenders;

                    var caseLog = new CaseLog
                    {
                        CaseId = c.Id,
                        FinishingDate = log.FinishConnectedCases ? (log.FinishingDate.HasValue ? log.FinishingDate : DateTime.Now) : null,
                        FinishingType = log.FinishConnectedCases ? log.FinishingCauseId : null,
                        SendMailAboutLog = true,
                        CaseHistoryId = caseHistoryId
                    };
                    if (log.ExternNotering)
                        caseLog.TextExternal = log.LogText;
                    if (log.InternNotering)
                        caseLog.TextInternal = log.LogText;

                    caseService.SendCaseEmail(c.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone, null, caseLog);
                }
            }
        }

        #endregion

    }
}
