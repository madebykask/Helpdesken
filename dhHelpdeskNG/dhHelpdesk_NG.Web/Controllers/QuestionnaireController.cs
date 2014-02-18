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

        #endregion

        #region Constructors and Destructors

        public QuestionnaireController(
            IQestionnaireService questionnaireService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _questionnaireService = questionnaireService;
        }

        #endregion

        #region Public Methods and Operators

        [HttpGet]
        public ViewResult EditQuestionnaire(int questionnaireId, int languageId)
        {
            var questionnaire = _questionnaireService.GetQuestionnaireById(questionnaireId, languageId);
            var languageOverviews = _questionnaireService.FindActiveLanguageOverivews();
            var languageList = new SelectList(languageOverviews, "Value", "Name");

            var model = new EditQuestionnaireModel(
                questionnaire.Id,
                questionnaire.Name,
                questionnaire.Description,
                questionnaire.LanguageId,
                questionnaire.ChangedDate,
                languageList);

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