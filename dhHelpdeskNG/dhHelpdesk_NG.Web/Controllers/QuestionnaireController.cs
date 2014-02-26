using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Routing;
using System.Web.UI.WebControls;
using DH.Helpdesk.BusinessData.Models.Common.Output;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Dal.EntityConfigurations.Changes;
using DH.Helpdesk.Dal.EntityConfigurations.Questionnaire;

namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models.Questionnaire.Input;
    using DH.Helpdesk.Web.Models.Questionnaire.Output;

    public class QuestionnaireController : BaseController
    {
        #region Fields

        private readonly IQestionnaireService _questionnaireService;

        private readonly IQestionnaireQuestionService _questionnaireQuestionService;

        private readonly IQestionnaireQuestionOptionService _questionnaireQuestionOptionService;

        private readonly ICircularService _circularService;

        #endregion

        #region Constructors and Destructors

        public QuestionnaireController(
            IQestionnaireService questionnaireService,
            IQestionnaireQuestionService questionnaireQuestionService,
            IQestionnaireQuestionOptionService questionnaireQuestionOptionService,
            ICircularService circularService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _questionnaireService = questionnaireService;
            _questionnaireQuestionService = questionnaireQuestionService;
            _questionnaireQuestionOptionService = questionnaireQuestionOptionService;
            _circularService = circularService;
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
                        new ItemOverview(Translation.Get(o.Name, Enums.TranslationSource.TextTranslation),
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
                    questionModel
                    );
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
                    null
                    );
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
            return RedirectToAction("EditQuestionnaire",
                new {questionnaireId = questionnaireModel.Id, languageId = questionnaireModel.LanguageId});
        }

        [HttpGet]
        public ViewResult Index()
        {
            var questionnaires = _questionnaireService.FindQuestionnaireOverviews(SessionFacade.CurrentCustomer.Id);
            var model = questionnaires.Select(q => new QuestionnaireOverviewModel(q.Id, q.Name, q.Description)).ToList();
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

            return RedirectToAction("EditQuestionnaire",
                new {questionnaireId = newQuestionniare.Id, languageId = LanguageId.Swedish});
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

            return RedirectToAction("EditQuestionnaire",
                new {questionnaireId = newQuestionniareQuestion.QuestionnaireId, languageId = LanguageId.Swedish});
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
                        new ItemOverview(Translation.Get(o.Name, Enums.TranslationSource.TextTranslation),
                            o.Value.ToString())).ToList();
            var languageList = new SelectList(languageOverviews, "Value", "Name");


            string currentQuestionNumber;
            int currentShowNote;

            if (languageId != LanguageId.Swedish)
            {
                var questionnaireQuestionSwedish =
                    _questionnaireQuestionService.GetQuestionnaireQuestionById(questionnaireQuestionId,
                        LanguageId.Swedish);
                currentQuestionNumber = questionnaireQuestionSwedish.QuestionNumber;
                currentShowNote = questionnaireQuestionSwedish.ShowNote;
            }
            else
            {
                currentQuestionNumber = questionnaireQuestion.QuestionNumber;
                currentShowNote = questionnaireQuestion.ShowNote;
            }

            var questionnaireQuestionOptions =
                _questionnaireQuestionOptionService.FindQuestionnaireQuestionOptions(questionnaireQuestionId, languageId);

            List<QuestionnaireQuesOptionModel> questionOptionsModel = null;
            if (questionnaireQuestionOptions != null)
            {
                questionOptionsModel =
                    questionnaireQuestionOptions.Select(
                        q =>
                            new QuestionnaireQuesOptionModel(q.Id, q.QuestionId, q.OptionPos, q.Option, q.OptionValue,
                                q.LanguageId, q.ChangedDate)).OrderBy(qq => qq.OptionPos).ToList();
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
                    questionOptionsModel
                    );
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
                    questionOptionsModel
                    );
            }
            return View(model);
        }

        [HttpPost]
        public RedirectToRouteResult EditQuestionnaireQuestion(EditQuestionnaireQuestionModel questionnaireQuestionModel,List<QuestionnaireQuesOptionModel>  Options)
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
                    var questionOption = new QuestionnaireQuesOption
                        (
                        option.Id,
                        option.QuestionId,
                        option.OptionPos,
                        option.Option,
                        option.OptionValue,
                        questionnaireQuestionModel.LanguageId,
                        now
                        );
                    _questionnaireQuestionOptionService.UpdateQuestionnaireQuestionOption(questionOption);
                }
            }

            return RedirectToAction("EditQuestionnaireQuestion", new
            {
                questionnaireId = questionnaireQuestionModel.QuestionnaireId,
                questionnaireQuestionId = questionnaireQuestionModel.Id,
                languageId = questionnaireQuestionModel.LanguageId
            }
                );
        }

        [HttpGet]
        public RedirectToRouteResult AddQuestionOption(int questionnaireId, int questionnaireQuestionId, int languageId,
                                                       int optionPos, string optionText, int optionValue)
        {
            var newOption = new QuestionnaireQuesOption
                (
                1,
                questionnaireQuestionId,
                optionPos,
                optionText,
                optionValue,
                languageId,
                DateTime.Now
                );

            _questionnaireQuestionOptionService.AddQuestionnaireQuestionOption(newOption);

            return RedirectToAction("EditQuestionnaireQuestion",
                new
                {
                    questionnaireId = questionnaireId,
                    questionnaireQuestionId = questionnaireQuestionId,
                    languageId = languageId
                });
        }

        [HttpPost]
        public RedirectToRouteResult DeleteQuestionOption(int questionnaireId, int questionnaireQuestionId, int languageId, int optionId)
        {            
            _questionnaireQuestionOptionService.DeleteQuestionnaireQuestionOptionById(optionId,languageId);

            return RedirectToAction("EditQuestionnaireQuestion",
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

            return RedirectToAction("EditQuestionnaire",
                new
                {
                    questionnaireId = questionnaireId,                    
                    languageId = languageId
                });
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
                        new ItemOverview(Translation.Get(o.Name, Enums.TranslationSource.TextTranslation),
                            o.Value.ToString())).ToList();
            var languageList = new SelectList(languageOverviews, "Value", "Name");

            List<SubQuestions> questions = new List<SubQuestions>();

            var allQuestions = _questionnaireQuestionService.FindQuestionnaireQuestionsOverviews(questionnaireId, languageId);

            foreach (var question in allQuestions)
            {
                List<SubOptions> options = new List<SubOptions>();
                var allOptions = _questionnaireQuestionOptionService.FindQuestionnaireQuestionOptions(question.Id, question.LanguageId);

                foreach (var option in allOptions)
                {
                    SubOptions opt = new SubOptions
                    (
                      option.OptionPos,
                      option.Option,
                      option.OptionValue
                    );
                    options.Add(opt);
                }


                SubQuestions ques = new SubQuestions
                    (
                      question.QuestionNumber,
                      question.Question,
                      question.ShowNote.ToBool(),
                      question.NoteText,
                      options.OrderBy(o=> o.OptionPos).ToList()
                    );
                questions.Add(ques);
            }

            var questionnaire = _questionnaireService.GetQuestionnaireById(questionnaireId, languageId);

            PreviewQuestionnaireModel model = new PreviewQuestionnaireModel
                (
                  questionnaireId,
                  languageId,
                  questionnaire.Name,
                  questionnaire.Description,
                  questions.OrderBy(q=> q.QuestionNumber).ToList(),
                  languageList
                );
            
            return View(model);
        }


        [HttpGet]
        public ViewResult CircularOverview(int questionnaireId, int state)
        {
            IEnumerable<CircularOverview> circulars = null;
            if (state == CircularStateId.All)
               circulars = _circularService.FindCircularOverviews(questionnaireId);
            else            
               circulars = _circularService.FindCircularOverviews(questionnaireId).Where(c=> c.State == state);
            

           List<CircularOverviewModel> model = null;            
           model = circulars.Select(c => new CircularOverviewModel(
                                                c.Id, c.CircularName, c.Date, 
                                                c.State == CircularStateId.ReadyToSend? "Ready To Send" : "Sent"))
                                 .ToList();

            ViewBag.qId = questionnaireId;
            ViewBag.curState = state;

            return View(model);
        }
    }
}