﻿using DH.Helpdesk.BusinessData.Models.Questionnaire;

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

        private readonly IQestionnaireQuestionService _questionnaireQuestionService;

        private readonly IQestionnaireQuestionOptionService _questionnaireQuestionOptionService;

        private readonly ICircularService _circularService;

        private readonly IDepartmentService _departmentService;

        private readonly ICaseTypeService _caseTypeService;

        private readonly IProductAreaService _productAreaService;

        private readonly IWorkingGroupService _workingGroupService;

        private readonly IInfoService _infoService;

        #endregion

        #region Constructors and Destructors

        public QuestionnaireController(
            IQestionnaireService questionnaireService,
            IQestionnaireQuestionService questionnaireQuestionService,
            IQestionnaireQuestionOptionService questionnaireQuestionOptionService,
            ICircularService circularService,
            IDepartmentService departmentService,
            ICaseTypeService caseTypeService,
            IProductAreaService productAreaService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService,
            IInfoService infoService)
            : base(masterDataService)
        {
            _questionnaireService = questionnaireService;
            _questionnaireQuestionService = questionnaireQuestionService;
            _questionnaireQuestionOptionService = questionnaireQuestionOptionService;
            _circularService = circularService;
            _departmentService = departmentService;
            _caseTypeService = caseTypeService;
            _productAreaService = productAreaService;
            _workingGroupService = workingGroupService;
            _infoService = infoService;
        }

        #endregion

        [HttpGet]
        public ViewResult EditQuestionnaire(int questionnaireId, int languageId)
        {
            var questionnaire = _questionnaireService.GetQuestionnaireById(questionnaireId, languageId);
            var languageOverviewsOrginal = _questionnaireService.FindActiveLanguageOverivews();
            var languageOverviews =
                languageOverviewsOrginal.Select(
                    o =>
                    new ItemOverview(
                        Translation.Get(o.Name, Enums.TranslationSource.TextTranslation),
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
            var questionnaires = _questionnaireService.FindQuestionnaireOverviews(SessionFacade.CurrentCustomer.Id);
            var model = questionnaires.Select(q => new QuestionnaireOverviewModel(q.Id, q.Name, q.Description)).OrderBy(x => x.Name).ToList();
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
                        Translation.Get(o.Name, Enums.TranslationSource.TextTranslation),
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
                    questionOptionsModel);
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
                    questionOptionsModel);
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
        public ViewResult CircularOverview(int questionnaireId)
        {
            List<CircularOverviewModel> circularOverviews = this.CreateCircularOverviewModels(
                questionnaireId,
                CircularStateId.All);

            var viewModel = new CircularOverviewViewModel(
                questionnaireId,
                circularOverviews,
                CircularStateId.ReadyToSend,
                new ReportFilter(new List<int>()));

            return this.View(viewModel);
        }

        [HttpPost]
        public PartialViewResult SearchCirculars(int questionnaireId, int show)
        {
            List<CircularOverviewModel> circularOverviews = this.CreateCircularOverviewModels(questionnaireId, show);
            ViewBag.QuestionnaireId = questionnaireId; // todo

            return this.PartialView("CircularOverviewGrid", circularOverviews);
        }

        [HttpGet]
        public ViewResult NewCircular(int questionnaireId)
        {
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
        public ViewResult EditCircular(int circularId)
        {
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
        public RedirectToRouteResult DeleteCircular(int questionnaireId, int stateId, int circularId)
        {
            this._circularService.DeleteById(circularId);

            return this.RedirectToAction("CircularOverview", new { questionnaireId, state = stateId });
        }

        [HttpPost]
        public ActionResult EditCircular(CircularModel newCircular, int[] connectedCases)
        {
            var cases = connectedCases == null || connectedCases.Count() == 0
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

            if (newCircular.Id == 0)
            {
                var circular = new CircularForInsert(newCircular.CircularName, newCircular.QuestionnaireId,
                    CircularStateId.ReadyToSend, DateTime.Now, cases, caseFilter);

                this._circularService.AddCircular(circular);
            }
            else
            {
                var circular = new CircularForUpdate(newCircular.Id, newCircular.CircularName, DateTime.Now, cases, caseFilter);
                this._circularService.UpdateCircular(circular);
            }

            return this.RedirectToAction(
                "CircularOverview",
                new { questionnaireId = newCircular.QuestionnaireId, state = CircularStateId.All });
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
                caseFilter.FinishingDateTo,
                caseFilter.IsUniqueEmail);

            List<ConnectedToCircularOverview> models =
                cases.Select(c => new ConnectedToCircularOverview(0, c.CaseId, c.CaseNumber, c.Caption, c.Email, Guid.Empty, c.IsSent))
                    .ToList();

            ViewData["QuestionnaireId"] = questionnaireId;
            return this.PartialView("_CircularPartOverviewWithDelete", models);
        }

        [HttpGet]
        public RedirectToRouteResult Send(int circularId)
        {
            string actionUrl = this.CreateQuestionnarieUrl();
            this._circularService.SendQuestionnaire(actionUrl, circularId, this.OperationContext);

            return this.RedirectToAction("EditCircular", new { circularId });
        }

        [HttpGet]
        public RedirectToRouteResult Remind(int circularId)
        {
            string actionUrl = this.CreateQuestionnarieUrl();
            this._circularService.Remind(actionUrl, circularId, this.OperationContext);

            return this.RedirectToAction("EditCircular", new { circularId });
        }

        [HttpGet]
        public ViewResult Questionnaire(Guid guid)
        {
            var detailed = this._circularService.GetQuestionnaire(guid, this.OperationContext);

            List<QuestionnaireQuestionModel> questionnarieQuestionsModel = (from question in detailed.Questionnaire.Questions
                                                                            let options =
                                                                                question.Options.Select(
                                                                                    option =>
                                                                                    new QuestionnaireQuestionOptionModel
                                                                                        (
                                                                                        option.Id,
                                                                                        option.Option,
                                                                                        option.Position)).ToList()
                                                                            select
                                                                                new QuestionnaireQuestionModel(
                                                                                question.Id,
                                                                                question.Question,
                                                                                question.Number,
                                                                                question.IsShowNote,
                                                                                question.NoteText,
                                                                                options)).ToList();

            var questionnarieModel = new QuestionnaireModel(
                detailed.Questionnaire.Id,
                detailed.Questionnaire.Name,
                detailed.Questionnaire.Description,
                detailed.CaseId,
                detailed.Caption,
                questionnarieQuestionsModel);

            var questionnarieViewModel = new QuestionnaireViewModel(questionnarieModel, false, guid);

            return this.View("Quiestionnaire", questionnarieViewModel);
        }

        [HttpPost]
        public RedirectToRouteResult Questionnaire(AnswersViewModel model)
        {
            List<Answer> ids =
                model.Questions.Where(x => x.SelectedOptionId != null)
                    .Select(x => new Answer(x.NoteText, (int)x.SelectedOptionId))
                    .ToList();

            var participant = new ParticipantForInsert(model.Guid, model.IsAnonym, OperationContext.DateAndTime, ids);

            this._circularService.SaveAnswers(participant);

            return this.RedirectToAction("QuestionnaireCompleted", "Questionnaire");
        }

        [HttpGet]
        public ViewResult QuestionnaireCompleted()
        {
            var html = _infoService.GetInfoText(4, OperationContext.CustomerId, OperationContext.LanguageId).Name;
            return View("QuestionnaireCompleted", model: html);
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


            var productAreaOrginal = this._productAreaService.GetTopProductAreasForUser(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentUser);
            var availablePa =
                productAreaOrginal.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();

            var selectedPa = new List<int>();


            var workingGroupsOrginal = _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);
            var availableWg =
                workingGroupsOrginal.Select(
                    x => new SelectListItem { Text = x.WorkingGroupName, Value = x.Id.ToString() }).ToList();

            var selectedWg = new List<int>();

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
                connectedCases);

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
                fullUrl = this.Url.Action("Questionnaire", "Questionnaire", null, url.Scheme, null);
                fullUrl = string.Format("{0}{1}", fullUrl, ParamString);
            }

            return fullUrl;
        }

        #endregion
    }
}