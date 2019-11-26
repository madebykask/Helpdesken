using DH.Helpdesk.BusinessData.Enums.MailTemplates;
using DH.Helpdesk.BusinessData.Models.Questionnaire;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Helpers;
using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;

namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Statistics.Output;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Services.Response.Questionnaire;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models.Questionnaire.Input;
    using DH.Helpdesk.Web.Models.Questionnaire.Output;

    public class QuestionnaireController : UserInteractionController
    {
        #region Fields

        private readonly IQestionnaireService _questionnaireService;
		private readonly IFeedbackService _feedbackService;

		private readonly IQestionnaireQuestionService _questionnaireQuestionService;

        private readonly IQestionnaireQuestionOptionService _questionnaireQuestionOptionService;

        private readonly ICircularService _circularService;

        private readonly IDepartmentService _departmentService;

        private readonly ICaseTypeService _caseTypeService;

        private readonly IProductAreaService _productAreaService;

        private readonly IWorkingGroupService _workingGroupService;

        private readonly ILanguageService _languageService;

        private readonly IMailTemplateService _mailTemplateService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly IEmailGroupService _emailGroupService;
        private readonly IEmailService _emailService;
        private readonly ISendToDialogModelFactory _sendToDialogModelFactory;

        #endregion

        #region Constructors and Destructors

        public QuestionnaireController(
            IQestionnaireService questionnaireService,
			IFeedbackService feedbackService,
			IQestionnaireQuestionService questionnaireQuestionService,
            IQestionnaireQuestionOptionService questionnaireQuestionOptionService,
            ICircularService circularService,
            IDepartmentService departmentService,
            ICaseTypeService caseTypeService,
            IProductAreaService productAreaService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService,
            ILanguageService languageService,
            IMailTemplateService mailTemplateService,
            IUserService userService,
            IEmailGroupService emailGroupService,
            IEmailService emailService,
            ISendToDialogModelFactory sendToDialogModelFactory,
            ISettingService settingService)
            : base(masterDataService)
        {
            _questionnaireService = questionnaireService;
			_feedbackService = feedbackService;
			_questionnaireQuestionService = questionnaireQuestionService;
            _questionnaireQuestionOptionService = questionnaireQuestionOptionService;
            _circularService = circularService;
            _departmentService = departmentService;
            _caseTypeService = caseTypeService;
            _productAreaService = productAreaService;
            _workingGroupService = workingGroupService;
            _languageService = languageService;
            _mailTemplateService = mailTemplateService;
            _userService = userService;
            _settingService = settingService;
            _emailGroupService = emailGroupService;
            _emailService = emailService;
            _sendToDialogModelFactory = sendToDialogModelFactory;
        }

        #endregion

        [HttpGet]
        public ViewResult EditQuestionnaire(int questionnaireId, int languageId)
        {
            var questionnaire = _questionnaireService.GetQuestionnaireById(questionnaireId, languageId);
            var languageOverviewsOrginal = _languageService.GetOverviews(true);
            var languageOverviews =
                languageOverviewsOrginal.Select(
                    o =>
                    new ItemOverview(Translation.GetCoreTextTranslation(o.Name),
                        o.Value.ToString())).ToList();
            var languageList = new SelectList(languageOverviews, "Value", "Name");

            var questionnaireQuestions =
                _questionnaireQuestionService.FindQuestionnaireQuestionsOverviews(questionnaireId, languageId);
            List<QuestionnaireQuestionsOverviewModel> questionModel = null;
            if (questionnaireQuestions != null)
            {
                questionModel =
                    questionnaireQuestions.Select(
                        q => new QuestionnaireQuestionsOverviewModel(q.Id, q.QuestionNumber, q.Question, q.LanguageId))
                        .OrderBy(qq => qq.QuestionNumber)
                        .ToList();
            }


            EditQuestionnaireModel model = null;
            if (questionnaire != null)
            {
                model = new EditQuestionnaireModel(
                    questionnaire.Id,
                    questionnaire.Name,
                    questionnaire.Description,
                    questionnaire.LanguageId,
                    questionnaire.ChangedDate,
                    languageList,
                    questionModel);
            }
            else
            {
                model = new EditQuestionnaireModel(
                    questionnaireId,
                    "",
                    "",
                    languageId,
                    DateTime.Now,
                    languageList,
                    null);
            }

            var dbCirculars = _circularService.GetCircularOverviews(questionnaireId, (int)CircularStates.Sent);
            model.IsSent = dbCirculars.Any();

            return View(model);
        }

        [HttpPost]
        public RedirectToRouteResult EditQuestionnaire(EditQuestionnaireModel questionnaireModel)
        {
            var editQuestionniare = new EditQuestionnaire(
                questionnaireModel.Id,
                questionnaireModel.Name,
                questionnaireModel.Description,
                questionnaireModel.LanguageId,
                DateTime.Now);

            _questionnaireService.UpdateQuestionnaire(editQuestionniare);
            return RedirectToAction(
                "EditQuestionnaire",
                new { questionnaireId = questionnaireModel.Id, languageId = questionnaireModel.LanguageId });
        }

        [HttpGet]
        public ViewResult Index()
        {
			var model = new QuestionnaireListsModel();

            var questionnaires = _questionnaireService.FindQuestionnaireOverviews(SessionFacade.CurrentCustomer.Id);
			var feedbacks = _feedbackService.FindFeedbackOverviews(SessionFacade.CurrentCustomer.Id);
			if (questionnaires.Any())
	        {
		        model.Questionnaires = questionnaires.Select(q => new QuestionnaireOverviewModel(q.Id, q.Name, q.Description)).OrderBy(x => x.Name).ToList();
			}
            if (feedbacks.Any())
            {
                model.Feedbacks = feedbacks.Select(q => new FeedbackOverviewModel(q.Id, q.Name, q.Description)).OrderBy(x => x.Name).ToList();
            }

            return View(model);
        }

        [HttpGet]
        public ViewResult NewQuestionnaire()
        {
            var model = new NewQuestionnaireModel();
            return View(model);
        }

        [HttpGet]
        public ViewResult NewQuestionnaireQuestion(int questionnaireId, int languageId)
        {
            var model = new NewQuestionnaireQuestionModel();
            model.QuestionnaireId = questionnaireId;
            model.LanguageId = languageId;
            return View(model);
        }

        [HttpPost]
        public RedirectToRouteResult NewQuestionnaire(NewQuestionnaireModel questionnaireModel)
        {
            var newQuestionniare = new NewQuestionnaire(
                questionnaireModel.Name,
                questionnaireModel.Description,
                SessionFacade.CurrentCustomer.Id,
                DateTime.Now);

            _questionnaireService.AddQuestionnaire(newQuestionniare);

            return RedirectToAction(
                "EditQuestionnaire",
                new { questionnaireId = newQuestionniare.Id, languageId = LanguageIds.Swedish });
        }

        [HttpPost]
        public RedirectToRouteResult NewQuestionnaireQuestion(NewQuestionnaireQuestionModel questionnaireQuestionModel)
        {
            var newQuestionniareQuestion = new NewQuestionnaireQuestion(
                questionnaireQuestionModel.QuestionnaireId,
                questionnaireQuestionModel.QuestionNumber,
                questionnaireQuestionModel.Question,
                questionnaireQuestionModel.ShowNote,
                questionnaireQuestionModel.NoteText,
                DateTime.Now);

            _questionnaireQuestionService.AddQuestionnaireQuestion(newQuestionniareQuestion);

            return RedirectToAction(
                "EditQuestionnaireQuestion",
                new
                    {
                        questionnaireId = questionnaireQuestionModel.QuestionnaireId,
                        questionnaireQuestionId = newQuestionniareQuestion.Id,
                        languageId = LanguageIds.Swedish
                    });
        }

        [HttpGet]
        public ViewResult EditQuestionnaireQuestion(int questionnaireId, int questionnaireQuestionId, int languageId)
        {
            var questionnaireQuestion =
                _questionnaireQuestionService.GetQuestionnaireQuestionById(questionnaireQuestionId, languageId);
            var languageOverviewsOrginal = _questionnaireQuestionService.FindActiveLanguageOverivews();
            var languageOverviews =
                languageOverviewsOrginal.Select(
                    o =>
                    new ItemOverview(
                        Translation.GetCoreTextTranslation(o.Name),
                        o.Value.ToString())).ToList();
            var languageList = new SelectList(languageOverviews, "Value", "Name");


            string currentQuestionNumber;
            int currentShowNote;

            if (languageId != LanguageIds.Swedish)
            {
                var questionnaireQuestionSwedish =
                    _questionnaireQuestionService.GetQuestionnaireQuestionById(
                        questionnaireQuestionId,
                        LanguageIds.Swedish);
                currentQuestionNumber = questionnaireQuestionSwedish.QuestionNumber;
                currentShowNote = questionnaireQuestionSwedish.ShowNote;
            }
            else
            {
                currentQuestionNumber = questionnaireQuestion.QuestionNumber;
                currentShowNote = questionnaireQuestion.ShowNote;
            }

            var questionnaireQuestionOptions =
                _questionnaireQuestionOptionService.FindQuestionnaireQuestionOptions(
                    questionnaireQuestionId,
                    languageId);

            List<QuestionnaireQuesOptionModel> questionOptionsModel = null;
            if (questionnaireQuestionOptions != null)
            {
                questionOptionsModel =
                    questionnaireQuestionOptions.Select(
                        q =>
                        new QuestionnaireQuesOptionModel(
                            q.Id,
                            q.QuestionId,
                            q.OptionPos,
                            q.Option,
                            q.OptionValue,
                            q.LanguageId,
                            q.ChangedDate)).OrderBy(qq => qq.OptionPos).ToList();
            }

            var dbCirculars = _circularService.GetCircularOverviews(questionnaireId, (int)CircularStates.Sent);
            var isSent = dbCirculars.Any();

            EditQuestionnaireQuestionModel model = null;
            if (questionnaireQuestion != null)
            {
                model = new EditQuestionnaireQuestionModel(
                    questionnaireQuestion.Id,
                    questionnaireId,
                    questionnaireQuestion.LanguageId,
                    currentQuestionNumber,
                    questionnaireQuestion.Question,
                    currentShowNote,
                    questionnaireQuestion.NoteText,
                    questionnaireQuestion.ChangeDate,
                    languageList,
                    questionOptionsModel,
                    isSent);
            }
            else
            {
                model = new EditQuestionnaireQuestionModel(
                    questionnaireQuestionId,
                    questionnaireId,
                    languageId,
                    currentQuestionNumber,
                    "",
                    currentShowNote,
                    "",
                    DateTime.Now,
                    languageList,
                    questionOptionsModel,
                    isSent);
            }
            return View(model);
        }

        [HttpPost]
        public RedirectToRouteResult EditQuestionnaireQuestion(
            EditQuestionnaireQuestionModel questionnaireQuestionModel,
            List<QuestionnaireQuesOptionModel> Options)
        {
            var editQuestionniareQuestion = new EditQuestionnaireQuestion(
                questionnaireQuestionModel.Id,
                questionnaireQuestionModel.QuestionnaireId,
                questionnaireQuestionModel.LanguageId,
                questionnaireQuestionModel.QuestionNumber,
                questionnaireQuestionModel.Question,
                questionnaireQuestionModel.ShowNote,
                questionnaireQuestionModel.NoteText,
                DateTime.Now);

            _questionnaireQuestionService.UpdateQuestionnaireQuestion(editQuestionniareQuestion);

            if (Options != null)
            {
                DateTime now = DateTime.Now;
                foreach (var option in Options)
                {
                    var questionOption = new QuestionnaireQuesOption(
                        option.Id,
                        option.QuestionId,
                        option.OptionPos,
                        option.Option,
                        option.OptionValue,
                        questionnaireQuestionModel.LanguageId,
                        now);
                    _questionnaireQuestionOptionService.UpdateQuestionnaireQuestionOption(questionOption);
                }
            }

            return RedirectToAction(
                "EditQuestionnaireQuestion",
                new
                    {
                        questionnaireId = questionnaireQuestionModel.QuestionnaireId,
                        questionnaireQuestionId = questionnaireQuestionModel.Id,
                        languageId = questionnaireQuestionModel.LanguageId
                    });
        }

        [HttpGet]
        public RedirectToRouteResult AddQuestionOption(
            int questionnaireId,
            int questionnaireQuestionId,
            int languageId,
            int optionPos,
            string optionText,
            int optionValue)
        {
            var newOption = new QuestionnaireQuesOption(
                1,
                questionnaireQuestionId,
                optionPos,
                optionText,
                optionValue,
                languageId,
                DateTime.Now);

            _questionnaireQuestionOptionService.AddQuestionnaireQuestionOption(newOption);

            return RedirectToAction(
                "EditQuestionnaireQuestion",
                new
                    {
                        questionnaireId = questionnaireId,
                        questionnaireQuestionId = questionnaireQuestionId,
                        languageId = languageId
                    });
        }

        [HttpPost]
        public RedirectToRouteResult DeleteQuestionOption(
            int questionnaireId,
            int questionnaireQuestionId,
            int languageId,
            int optionId)
        {
            _questionnaireQuestionOptionService.DeleteQuestionnaireQuestionOptionById(optionId, languageId);

            return RedirectToAction(
                "EditQuestionnaireQuestion",
                new
                    {
                        questionnaireId = questionnaireId,
                        questionnaireQuestionId = questionnaireQuestionId,
                        languageId = languageId
                    });
        }

        [HttpPost]
        public RedirectToRouteResult DeleteQuestion(int questionnaireId, int languageId, int questionId)
        {
            _questionnaireQuestionService.DeleteQuestionnaireQuestionById(questionId);

            return RedirectToAction(
                "EditQuestionnaire",
                new { questionnaireId = questionnaireId, languageId = languageId });
        }

        [HttpPost]
        public RedirectToRouteResult DeleteQuestionnaire(int questionnaireId)
        {
            _questionnaireService.DeleteQuestionnaireById(questionnaireId);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult PreviewQuestionnaire(int questionnaireId, int languageId)
        {

            var languageOverviewsOrginal = _questionnaireQuestionService.FindActiveLanguageOverivews();
            var languageOverviews =
                languageOverviewsOrginal.Select(
                    o =>
                    new ItemOverview(
                        Translation.Get(o.Name, Enums.TranslationSource.TextTranslation),
                        o.Value.ToString())).ToList();
            var languageList = new SelectList(languageOverviews, "Value", "Name");

            List<SubQuestions> questions = new List<SubQuestions>();

            var allQuestions = _questionnaireQuestionService.FindQuestionnaireQuestionsOverviews(
                questionnaireId,
                languageId);

            foreach (var question in allQuestions)
            {
                List<SubOptions> options = new List<SubOptions>();
                var allOptions = _questionnaireQuestionOptionService.FindQuestionnaireQuestionOptions(
                    question.Id,
                    question.LanguageId);

                foreach (var option in allOptions)
                {
                    SubOptions opt = new SubOptions(option.OptionPos, option.Option, option.OptionValue);
                    options.Add(opt);
                }


                SubQuestions ques = new SubQuestions(
                    question.QuestionNumber,
                    question.Question,
                    question.ShowNote.ToBool(),
                    question.NoteText,
                    options.OrderBy(o => o.OptionPos).ToList());
                questions.Add(ques);
            }

            var questionnaire = _questionnaireService.GetQuestionnaireById(questionnaireId, languageId);

            PreviewQuestionnaireModel model = new PreviewQuestionnaireModel(
                questionnaireId,
                languageId,
                questionnaire.Name,
                questionnaire.Description,
                questions.OrderBy(q => q.QuestionNumber).ToList(),
                languageList);

            return View(model);
        }

        [HttpGet]
        public ViewResult CircularOverview(int questionnaireId, int? statusId)
        {
            var ensuredStatusId = statusId ?? CircularStateId.All;
            List<CircularOverviewModel> circularOverviews = this.CreateCircularOverviewModels(
                questionnaireId,
                ensuredStatusId);

            var viewModel = new CircularOverviewViewModel(
                questionnaireId,
                circularOverviews,
                ensuredStatusId,
                new ReportFilter(new List<int>()));

            ViewData["StatusId"] = ensuredStatusId;

            return this.View(viewModel);
        }

        [HttpPost]
        public PartialViewResult SearchCirculars(int questionnaireId, int show)
        {
            List<CircularOverviewModel> circularOverviews = this.CreateCircularOverviewModels(questionnaireId, show);
            ViewBag.QuestionnaireId = questionnaireId; // todo
            ViewData["StatusId"] = show;

            return this.PartialView("CircularOverviewGrid", circularOverviews);
        }

        [HttpGet]
        public ViewResult NewCircular(int questionnaireId, int? backStatusId)
        {
            ViewBag.BackStatusId = backStatusId;
            var model = GetCircularModel(
                0,
                questionnaireId,
                "",
                null,
                CircularStates.None, 
                new List<ConnectedToCircularOverview>());

            return View("EditCircular", model);
        }

        [HttpGet]
        public ViewResult EditCircular(int circularId, int? backStatusId)
        {
            ViewBag.BackStatusId = backStatusId;
            CircularForEdit circular = this._circularService.GetById(circularId);

            List<ConnectedCase> connectedCases = this._circularService.GetConnectedCases(circularId);
            List<ConnectedToCircularOverview> connecteCasesOverviews =
                connectedCases.Select(
                    x =>
                    new ConnectedToCircularOverview(
                        circularId,
                        x.CaseId,
                        x.CaseNumber,
                        x.Caption,
                        x.Email,
                        x.Guid,
                        x.IsSent)).ToList();

            var model = GetCircularModel(
                circular.Id,
                circular.QuestionnaireId,
                circular.CircularName,
                circular.ChangedDate,
                circular.Status,
                connecteCasesOverviews);
            model.MailTemplateId = circular.MailTemplateId;
            model.CaseFilter = new CircularCaseFilter
            {
                IsUniqueEmail = circular.CaseFilter.IsUniqueEmail,
                FinishingDateFrom = circular.CaseFilter.FinishingDateFrom,
                FinishingDateTo = circular.CaseFilter.FinishingDateTo,
                SelectedProcent = circular.CaseFilter.SelectedProcent,
                SelectedDepartments = circular.CaseFilter.SelectedDepartments,
                SelectedCaseTypes = circular.CaseFilter.SelectedCaseTypes,
                SelectedProductAreas = circular.CaseFilter.SelectedProductAreas,
                SelectedWorkingGroups = circular.CaseFilter.SelectedWorkingGroups
            };

            return this.View(model);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteCircular(int questionnaireId, int circularId, int? backStatusId)
        {
            this._circularService.DeleteById(circularId);

            return this.RedirectToAction("CircularOverview", new { questionnaireId, statusId = backStatusId });
        }

        [HttpPost]
        public ActionResult EditCircular(CircularModel newCircular, int[] connectedCases, int? backStatusId)
        {
            var cases = connectedCases == null || !connectedCases.Any()
                            ? new List<int>()
                            : connectedCases.ToList();

            var caseFilter = new BusinessData.Models.Questionnaire.CircularCaseFilter
            {
                IsUniqueEmail = newCircular.CaseFilter.IsUniqueEmail,
                FinishingDateFrom = newCircular.CaseFilter.FinishingDateFrom,
                FinishingDateTo = newCircular.CaseFilter.FinishingDateTo,
                SelectedDepartments = newCircular.CaseFilter.SelectedDepartments,
                SelectedCaseTypes = newCircular.CaseFilter.SelectedCaseTypes,
                SelectedProductAreas = newCircular.CaseFilter.SelectedProductAreas,
                SelectedWorkingGroups = newCircular.CaseFilter.SelectedWorkingGroups,
                SelectedProcent = newCircular.CaseFilter.SelectedProcent
            };

            var extraEmails = newCircular.ExtraEmails?.Split(BRConstItem.Email_Char_Separator).Where(x => !string.IsNullOrEmpty(x)).ToList();
            var circularId = newCircular.Id;
            if (newCircular.Id == 0)
            {
                var circular = new CircularForInsert(newCircular.CircularName, newCircular.QuestionnaireId,
                    CircularStateId.ReadyToSend, DateTime.Now, cases, caseFilter, extraEmails);
                circular.MailTemplateId = newCircular.MailTemplateId;
                circularId = _circularService.AddCircular(circular);
            }
            else
            {
                var circular = new CircularForUpdate(newCircular.Id, newCircular.CircularName, DateTime.Now, caseFilter, extraEmails);
                circular.MailTemplateId = newCircular.MailTemplateId;
                this._circularService.UpdateCircular(circular, cases);
                circularId = circular.Id;
            }

            return this.RedirectToAction("EditCircular", new { circularId = circularId, backStatusId });
        }

        [HttpPost]
        public PartialViewResult CaseRowGrid(
            int questionnaireId,
            CircularCaseFilter caseFilter)
        {
            List<AvailableCase> cases = this._circularService.GetAvailableCases(
                SessionFacade.CurrentCustomer.Id,
                questionnaireId,
                caseFilter.SelectedDepartments,
                caseFilter.SelectedCaseTypes,
                caseFilter.SelectedProductAreas,
                caseFilter.SelectedWorkingGroups,
                caseFilter.SelectedProcent,
                caseFilter.FinishingDateFrom,
                caseFilter.FinishingDateTo.HasValue ? caseFilter.FinishingDateTo.Value.GetEndOfDay() : caseFilter.FinishingDateTo,
                caseFilter.IsUniqueEmail);

            List<ConnectedToCircularOverview> models =
                cases.Select(c => new ConnectedToCircularOverview(0, c.CaseId, c.CaseNumber, c.Caption, c.Email, Guid.Empty, c.IsSent))
                    .ToList();

            ViewData["QuestionnaireId"] = questionnaireId;
            return this.PartialView("_CircularPartOverviewWithDelete", models);
        }

        [HttpGet]
        public RedirectToRouteResult Send(int circularId, int? backStatusId)
        {
            string actionUrl = this.CreateQuestionnarieUrl();
            this._circularService.SendQuestionnaire(actionUrl, circularId, this.OperationContext);

            return this.RedirectToAction("EditCircular", new { circularId, backStatusId });
        }

        [HttpGet]
        public RedirectToRouteResult Remind(int circularId, int? backStatusId)
        {
            string actionUrl = this.CreateQuestionnarieUrl();
            this._circularService.Remind(actionUrl, circularId, this.OperationContext);

            return this.RedirectToAction("EditCircular", new { circularId, backStatusId });
        }

        [HttpGet]
        public ViewResult Statistics(int questionnaireId, int circularId)
        {
            QuestionnaireOverview questionnaire = this._circularService.GetQuestionnaire(
                questionnaireId,
                OperationContext);
            List<OptionResult> results = this._circularService.GetResult(circularId);

            var viewModel = new StatisticsViewModel(questionnaireId, questionnaire, results);

            return this.View("Statistics", viewModel);
        }

        [HttpPost]
        public PartialViewResult Statistics(int questionnaireId, ReportFilter reportFilter)
        {
            QuestionnaireOverview questionnaire = this._circularService.GetQuestionnaire(
                questionnaireId,
                OperationContext);
            List<OptionResult> results = this._circularService.GetResults(
                reportFilter.ConnectedCirculars,
                reportFilter.CircularCreatedDate.DateFrom,
                reportFilter.CircularCreatedDate.DateTo);

            var viewModel = new StatisticsViewModel(questionnaireId, questionnaire, results);

            return this.PartialView("StatisticsGrid", viewModel);
        }

        #region PRIVATE

        private CircularModel GetCircularModel(int circularId, int questionnaireId, string name, DateTime? changedDate, CircularStates status, List<ConnectedToCircularOverview> connectedCases)
        {
            var departmentsOrginal = _departmentService.GetDepartments(SessionFacade.CurrentCustomer.Id);
            var availableDp =
                departmentsOrginal.Select(x => new SelectListItem { Text = x.DepartmentName, Value = x.Id.ToString() })
                    .ToList();

            var selectedDp = new List<int>();

            var caseTypesOrginal = _caseTypeService.GetCaseTypes(SessionFacade.CurrentCustomer.Id);
            var availableCt =
                caseTypesOrginal.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();

            var selectedCt = new List<int>();


            var productAreaOrginal = this._productAreaService.GetTopProductAreasWithChilds(SessionFacade.CurrentCustomer.Id);
            var availablePa =
                productAreaOrginal.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();

            var selectedPa = new List<int>();


            var workingGroupsOrginal = _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);
            var availableWg =
                workingGroupsOrginal.Select(
                    x => new SelectListItem { Text = x.WorkingGroupName, Value = x.Id.ToString() }).ToList();

            var selectedWg = new List<int>();

            var extraEmails = _circularService.GetCircularExtraEmails(circularId);
            var extraEmailsStr = extraEmails != null && extraEmails.Any()
                ? string.Join(BRConstItem.Email_Separator, extraEmails) + BRConstItem.Email_Separator
                : string.Empty;

            var templates = new List<SelectListItem>{ new SelectListItem { Text = string.Empty } };
            templates.AddRange(_mailTemplateService.GetMailTemplates(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId)
                    .Where(x => x.IsStandard == 0).Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }).ToList());

            var responsibleUsersList = _userService.GetAvailablePerformersOrUserId(SessionFacade.CurrentCustomer.Id);
            var customerSettings = _settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            var extraEmailsModel = _sendToDialogModelFactory.CreateNewSendToDialogModel(SessionFacade.CurrentCustomer.Id, responsibleUsersList.ToList(), customerSettings,
                _emailGroupService, _workingGroupService, _emailService);

            var model = new CircularModel(
                circularId,
                questionnaireId,
                availableDp,
                selectedDp,
                availableCt,
                selectedCt,
                availablePa,
                selectedPa,
                availableWg,
                selectedWg,
                false,
                name,
                changedDate,
                status,
                connectedCases,
                extraEmailsStr,
                templates,
                extraEmailsModel);

            var lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Text = "5", Value = "5" });
            lst.Add(new SelectListItem { Text = "10", Value = "10" });
            lst.Add(new SelectListItem { Text = "20", Value = "20" });
            lst.Add(new SelectListItem { Text = "25", Value = "25" });
            lst.Add(new SelectListItem { Text = "50", Value = "50" });
            lst.Add(new SelectListItem { Text = "100", Value = "100" });
            model.Procent = lst;

            model.ModelMode = 0;

            model.CaseFilter.FinishingDateFrom = null;
            model.CaseFilter.FinishingDateTo = null;

            return model;
        }

        private List<CircularOverviewModel> CreateCircularOverviewModels(int questionnaireId, int state)
        {
            List<CircularOverview> circulars = this._circularService.GetCircularOverviews(questionnaireId, state);

            List<CircularOverviewModel> circularOverviews =
                circulars.Select(
                    c =>
                    new CircularOverviewModel(
                        c.Id,
                        c.CircularName,
                        c.Date,
                        c.State,
                        c.TotalParticipants,
                        c.SentParticipants,
                        c.AnsweredParticipants)).ToList();
            return circularOverviews;
        }

        private string CreateQuestionnarieUrl()
        {
            const string ParamString = "?guid=";

            Uri url = this.HttpContext.Request.Url;
            string fullUrl = string.Empty;
            if (url != null)
            {
                fullUrl = this.Url.Action("Questionnaire", "QuestionnaireAnswer", null, url.Scheme, null);
                fullUrl = string.Format("{0}{1}", fullUrl, ParamString);
            }

            return fullUrl;
        }

        #endregion
    }
}