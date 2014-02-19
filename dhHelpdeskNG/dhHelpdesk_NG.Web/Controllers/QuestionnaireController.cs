using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Common.Output;
using DH.Helpdesk.Dal.EntityConfigurations.Changes;

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

        #endregion

        #region Constructors and Destructors

        public QuestionnaireController(
            IQestionnaireService questionnaireService,
            IQestionnaireQuestionService questionnaireQuestionService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _questionnaireService = questionnaireService;
            _questionnaireQuestionService = questionnaireQuestionService;
        }

        #endregion

        #region Public Methods and Operators

        [HttpGet]
        public ViewResult EditQuestionnaire(int questionnaireId, int languageId)
        {
            var questionnaire = _questionnaireService.GetQuestionnaireById(questionnaireId, languageId);
            var languageOverviewsOrginal = _questionnaireService.FindActiveLanguageOverivews();
            var languageOverviews =
                    languageOverviewsOrginal.Select(o => new ItemOverview(Translation.Get(o.Name, Enums.TranslationSource.TextTranslation), o.Value.ToString())).ToList();            
            var languageList = new SelectList(languageOverviews, "Value", "Name");

            var questionnaireQuestions =
                _questionnaireQuestionService.FindQuestionnaireQuestionsOverviews(questionnaireId, languageId);

            List<QuestionnaireQuestionsOverviewModel> questionModel = null;
            if (questionnaireQuestions != null)
            {
                questionModel =
                    questionnaireQuestions.Select(
                        q => new QuestionnaireQuestionsOverviewModel(q.Id, q.QuestionNumber, q.Question,q.LanguageId)).OrderBy(qq => qq.QuestionNumber).ToList();
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
            return RedirectToAction("EditQuestionnaire", new { questionnaireId = questionnaireModel.Id, languageId = questionnaireModel.LanguageId });
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

        [HttpPost]
        public RedirectToRouteResult NewQuestionnaire(NewQuestionnaireModel questionnaireModel)
        {
            var newQuestionniare = new NewQuestionnaire(
                questionnaireModel.Name,
                questionnaireModel.Description,
                SessionFacade.CurrentCustomer.Id,
                DateTime.Now);

            _questionnaireService.AddQuestionnaire(newQuestionniare);

            return RedirectToAction("EditQuestionnaire", new { questionnaireId = newQuestionniare.Id, languageId = LanguageId.Swedish });
        }

        #endregion
    }
}