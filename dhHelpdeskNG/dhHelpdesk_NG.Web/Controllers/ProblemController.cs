﻿namespace dhHelpdesk_NG.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.FinishingCause;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Infrastructure.Filters.Problems;
    using dhHelpdesk_NG.Web.Models.Problem;

    public class ProblemsController : BaseController
    {
        private readonly IProblemService problemService;
        private readonly IProblemLogService problemLogService;
        private readonly IFinishingCauseService finishingCauseService;

        private readonly ICaseService caseService;

        private readonly IUserService userService;

        public ProblemsController(
                IProblemService problemService,
                IMasterDataService masterDataService,
                IUserService userService,
                IProblemLogService problemLogService,
                ICaseService caseService,
                IFinishingCauseService finishingCauseService)
            : base(masterDataService)
        {
            this.problemService = problemService;
            this.userService = userService;
            this.problemLogService = problemLogService;
            this.caseService = caseService;
            this.finishingCauseService = finishingCauseService;
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

        public static ProblemOutputModel MapProblemOverviewToOutputModel(ProblemOverview problemOverview)
        {
            return new ProblemOutputModel
                       {
                           Id = problemOverview.Id,
                           Name = problemOverview.Name,
                           Description = problemOverview.Description,
                           ProblemNumber = problemOverview.ProblemNumber,
                           ResponsibleUserName = problemOverview.ResponsibleUserName
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

        public static LogOutputModel MapLogs(ProblemLogOverview arg)
        {
            return new LogOutputModel
                       {
                           Id = arg.Id,
                           Date = DateTime.SpecifyKind(arg.ChangedDate, DateTimeKind.Utc).ToShortDateString(),
                           LogNote = arg.LogText,
                           RegistratedBy = arg.ChangedByUserName
                       };
        }

        public static LogEditModel MapLogs(NewProblemLogDto arg)
        {
            return new LogEditModel
            {
                Id = arg.Id,
                FinishingDate = arg.FinishingDate.HasValue ? DateTime.SpecifyKind(arg.FinishingDate.Value, DateTimeKind.Utc).ToShortDateString() : string.Empty,
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
            return new CaseOutputModel
                       {
                           Id = arg.Id,
                           CaseNumber = arg.CaseNumber.ToString(),
                           Caption = arg.Caption,
                           RegistrationDate = DateTime.SpecifyKind(arg.RegTime, DateTimeKind.Utc).ToShortDateString(),
                           WatchDate = arg.WatchDate.HasValue ? DateTime.SpecifyKind(arg.WatchDate.Value, DateTimeKind.Utc).ToShortDateString() : string.Empty,
                       };
        }

        public ActionResult Index()
        {
            var filter = SessionFacade.GetPageFilters<ProblemFilter>(Enums.PageName.Problems) ?? new ProblemFilter();

            var problems = this.problemService.GetCustomerProblems(SessionFacade.CurrentCustomer.Id, filter.EntityStatus);

            var problemOutputModels = problems.Select(MapProblemOverviewToOutputModel).ToList();

            var viewModel = new ProblemOutputViewModel { Problems = problemOutputModels, Show = (Enums.Show)filter.EntityStatus };

            return this.View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Search(EntityStatus show)
        {
            var problems = this.problemService.GetCustomerProblems(SessionFacade.CurrentCustomer.Id, show);
            var problemOutputModels = problems.Select(MapProblemOverviewToOutputModel).ToList();

            SessionFacade.SavePageFilters(Enums.PageName.Problems, new ProblemFilter { EntityStatus = show });

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
                log.FinishingDate == null ? null : (DateTime?)DateTime.Parse(log.FinishingDate),
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

            return this.RedirectToAction("Problem", new { id = problemDto.Id });
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
            var causes = this.finishingCauseService.GetFinishingCausesWithChilds(SessionFacade.CurrentCustomer.Id).Select(this.CauseToDropDownItem).ToList();
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
                log.FinishingDate == null ? null : (DateTime?)DateTime.Parse(log.FinishingDate),
                log.FinishConnectedCases ? 1 : 0) { ProblemId = log.ProblemId };

            this.problemLogService.AddLog(logDto);
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
                log.FinishingDate == null ? null : (DateTime?)DateTime.Parse(log.FinishingDate),
                log.FinishConnectedCases ? 1 : 0) { ProblemId = log.ProblemId, Id = log.Id };

            this.problemLogService.UpdateLog(logDto);
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

        private ProblemEditViewModel CreateProblemEditViewModel(int id)
        {
            var problem = this.problemService.GetProblem(id);
            var logs = this.problemLogService.GetProblemLogs(id);

            // todo!!!
            var cases = this.caseService.GetCases().Where(x => x.Problem_Id == id);

            var users = this.userService.GetUsers(SessionFacade.CurrentCustomer.Id);

            var problemOutputModel = MapProblemOverviewToEditOutputModel(problem);
            var outputLogs = logs.Select(MapLogs).ToList();
            var outputCases = cases.Select(MapCase).ToList();
            var userOutputModels = users.Select(x => new SelectListItem { Text = string.Format("{0} {1}", x.FirstName, x.SurName), Value = x.Id.ToString(CultureInfo.InvariantCulture) }).ToList();

            return new ProblemEditViewModel { Problem = problemOutputModel, Users = userOutputModels, Logs = outputLogs, Cases = outputCases };
        }
    }
}
