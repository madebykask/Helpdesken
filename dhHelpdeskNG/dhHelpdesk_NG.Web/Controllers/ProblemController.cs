using System.Collections.Generic;
using System.Data;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Helpers;

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
    using DH.Helpdesk.Common.Enums;

    public class ProblemsController : BaseController
    {
        private readonly IProblemService _problemService;
        private readonly IProblemLogService _problemLogService;
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly ISettingService _settingService;
        private readonly ICaseService _caseService;
        private readonly ICustomerService _customerService;

        private readonly IUserService _userService;

        private readonly ILogService _logService;

        public ProblemsController(
                IProblemService problemService,
                IMasterDataService masterDataService,
                IUserService userService,
                IProblemLogService problemLogService,
                ICaseService caseService,
                ISettingService settingService,
                IFinishingCauseService finishingCauseService,
                ICustomerService customerService,
                ILogService logService)
            : base(masterDataService)
        {
            _problemService = problemService;
            _userService = userService;
            _problemLogService = problemLogService;
            _caseService = caseService;
            _finishingCauseService = finishingCauseService;
            _settingService = settingService;
            _customerService = customerService;
            _logService = logService;
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
                CaseIcon = GetCaseIcon(arg)
            };
        }

        public ActionResult Index()
        {
            var filter = SessionFacade.FindPageFilters<ProblemFilter>(PageName.Problems) ?? new ProblemFilter { EntityStatus = EntityStatus.Active };
            var problems = this._problemService.GetCustomerProblems(SessionFacade.CurrentCustomer.Id, filter.EntityStatus);
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
            var problems = this._problemService.GetCustomerProblems(SessionFacade.CurrentCustomer.Id, show);
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
            var users = _userService.GetAvailablePerformersOrUserId(SessionFacade.CurrentCustomer.Id);

            var userOutputModels = 
                users.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(CultureInfo.InvariantCulture),
                    Text = $"{x.FirstName} {x.SurName}"
                }).ToList();

            var viewModel = new ProblemEditViewModel { Problem = new ProblemEditModel(), Users = userOutputModels };

            return View(viewModel);
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
            //Loop through all cases here?
            this._problemService.AddProblem(problemDto, logDto);

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

            this._problemService.AddProblem(problemDto);

            return this.RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            this._problemService.DeleteProblem(id);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Save(ProblemEditModel problem)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var problemDto = 
                new NewProblemDto(problem.Name, problem.Description, problem.ResponsibleUserId, problem.InventoryNumber, problem.ShowOnStartPage, SessionFacade.CurrentCustomer.Id, null)
                {
                    Id = problem.Id
                };

            this._problemService.UpdateProblem(problemDto);

            return this.RedirectToAction("Index");
        }

        public ActionResult Activate(int id)
        {
            this._problemService.ActivateProblem(id);
            return this.RedirectToAction("Problem", new { id });
        }

        public PartialViewResult LogForNewProblem()
        {
            var causes = _finishingCauseService.GetFinishingCausesWithChilds(SessionFacade.CurrentCustomer.Id);
            return this.PartialView("_InputLog", new LogEditModel { FinishingCauses = causes });
        }

        public PartialViewResult Log(int id)
        {
            var causes = _finishingCauseService.GetFinishingCausesWithChilds(SessionFacade.CurrentCustomer.Id);
            var finishingCauses = _finishingCauseService.GetFinishingCauseInfos(SessionFacade.CurrentCustomer.Id);
            var log = MapLogs(this._problemLogService.GetProblemLog(id));
            log.FinishingCauses = causes;
            log.FinishingCause = CommonHelper.GetFinishingCauseFullPath(finishingCauses.ToArray(), log.FinishingCauseId);

            return this.PartialView("EditLog", log);
        }

        public PartialViewResult NewLog(int problemId)
        {
            var causes = _finishingCauseService.GetFinishingCausesWithChilds(SessionFacade.CurrentCustomer.Id);
            return this.PartialView("NewLog", new LogEditModel { FinishingCauses = causes });
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

            this._problemLogService.AddLog(logDto);

            //Save LogNote - SaveCaseLog
            SendProblemLogEmailAndSaveHistory(log);

            //Save entry in TblLog
            SaveLogFile(log);


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

            this._problemLogService.UpdateLog(logDto);

            SendProblemLogEmailAndSaveHistory(log);

            //Save entry in TblLog
            SaveLogFile(log);

            return this.RedirectToAction("ProblemActiveLog", new { id = log.ProblemId });
        }

        public ActionResult DeleteLog(int problemId, int logId)
        {
            this._problemLogService.DeleteLog(logId);
            return this.RedirectToAction("ProblemActiveLog", new { id = problemId });
        }

        public ActionResult ResetLog(string s)
        {
            return null;
        }

        #region Private

        private ProblemEditViewModel CreateProblemEditViewModel(int id)
        {
            var problem = this._problemService.GetProblem(id);
            var logs = this._problemLogService.GetProblemLogs(id);

            // todo!!!
            var cases = this._caseService.GetProblemCases(SessionFacade.CurrentCustomer.Id, id);
            var setting = this._settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            var isFirstName = (setting.IsUserFirstLastNameRepresentation == 1);

            var users = _userService.GetAvailablePerformersOrUserId(SessionFacade.CurrentCustomer.Id); 
            var problemOutputModel = MapProblemOverviewToEditOutputModel(problem);

            var outputLogs = logs.Select(x=> MapLogs(x, isFirstName)).OrderBy(x => x.Date).ToList();

            var outputCases = cases.Select(MapCase).ToList();
            
            
            var userOutputModels = users.Select(x => new SelectListItem 
                { 
                    Text = (isFirstName? string.Format("{0} {1}", x.FirstName, x.SurName) : string.Format("{0} {1}", x.SurName, x.FirstName)), 
                    Value = x.Id.ToString(CultureInfo.InvariantCulture)
            }).OrderBy(x => x.Text).ToList();

            return new ProblemEditViewModel { Problem = problemOutputModel, Users = userOutputModels, Logs = outputLogs, Cases = outputCases };
        }

        private void SaveLogFile(LogEditModel log)
        {

            if (log.FinishConnectedCases)
            {
                var cases = _caseService.GetProblemCases(log.ProblemId);

                foreach (var c in cases)
                {

                    var caseHistoryId = _problemService.GetCaseHistoryId(c.Id, log.ProblemId);

                    var caseLog = new CaseLog
                    {
                        CaseHistoryId = caseHistoryId,
                        CaseId = c.Id,
                        UserId = SessionFacade.CurrentUser.Id,
                        FinishingDate = log.FinishConnectedCases ? (log.FinishingDate.HasValue ? log.FinishingDate : DateTime.Now) : null,
                        FinishingType = log.FinishConnectedCases ? log.FinishingCauseId : null,
                    };

                    IDictionary<string, string> errors;
                    int logId = _logService.SaveLog(caseLog, 0, out errors);
                }

            }

        }

        private void SendProblemLogEmailAndSaveHistory(LogEditModel log)
        {
            if (log.ExternNotering || log.FinishConnectedCases)
            {
                var cases = _caseService.GetProblemCases(log.ProblemId);
                foreach (var c in cases)
                {
                    var userTimeZone = TimeZoneInfo.Local;
                    var caseHistoryId = _problemService.GetCaseHistoryId(c.Id, log.ProblemId);

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
                        SendMailAboutCaseToNotifier = true,
                        CaseHistoryId = caseHistoryId
                    };

                    if (log.ExternNotering)
                        caseLog.TextExternal = log.LogText;

                    if (log.InternNotering)
                        caseLog.TextInternal = log.LogText;

                    IDictionary<string, string> errors;
                    // save casehistory
                    var ei = new CaseExtraInfo()
                    {
                        CreatedByApp = CreatedByApplications.Helpdesk5,
                        LeadTimeForNow = c.LeadTime,
                        ActionLeadTime = 0,
                        ActionExternalTime = 0
                    };
                    var newid = _caseService.SaveCase(c, caseLog, SessionFacade.CurrentUser.Id, User.Identity.Name, ei, out errors);
                    _caseService.SendProblemLogEmail(c, caseMailSetting, newid, userTimeZone, caseLog, log.FinishConnectedCases);
                }
            }
        }

        private static GlobalEnums.CaseIcon GetCaseIcon(Case cs)
        {
            var ret = GlobalEnums.CaseIcon.Normal;

            if (cs.FinishingDate.HasValue)
                if (!cs.ApprovedDate.HasValue && string.Compare("1", cs.CaseType.RequireApproving.ToString(), true, CultureInfo.InvariantCulture) == 0)
                    ret = GlobalEnums.CaseIcon.FinishedNotApproved;
                else
                    ret = GlobalEnums.CaseIcon.Finished;

            return ret;
        }

        #endregion

    }
}
