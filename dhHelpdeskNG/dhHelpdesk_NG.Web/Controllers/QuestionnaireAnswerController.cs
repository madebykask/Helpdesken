using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Models.Questionnaire.Input;
using DH.Helpdesk.Web.Models.Questionnaire.Output;

namespace DH.Helpdesk.Web.Controllers
{
    [Authorize]
    public class QuestionnaireAnswerController : Controller
    {
        private readonly ICircularService _circularService;
        private readonly IInfoService _infoService;

        public QuestionnaireAnswerController(ICircularService circularService, IInfoService infoService)
        {
            _circularService = circularService;
            _infoService = infoService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ViewResult Questionnaire(QuestionnaireAnswerParams parameters)
        {
            var detailed = this._circularService.GetQuestionnaire(parameters.Guid, parameters.LanguageId);

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

            var questionnarieViewModel = new QuestionnaireViewModel(questionnarieModel, false, parameters.Guid, parameters.CustomerId, parameters.LanguageId);

            return this.View("Quiestionnaire", questionnarieViewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public RedirectToRouteResult Questionnaire(AnswersViewModel model)
        {
            List<Answer> ids =
                model.Questions.Where(x => x.SelectedOptionId != null)
                    .Select(x => new Answer(x.NoteText, (int)x.SelectedOptionId))
                    .ToList();

            var participant = new ParticipantForInsert(model.Guid, model.IsAnonym, DateTime.Now, ids);

            this._circularService.SaveAnswers(participant);

            return this.RedirectToAction("QuestionnaireCompleted", new RouteValueDictionary(new { customerId = model.CustomerId, languageId = model.LanguageId }));
        }

        [AllowAnonymous]
        [HttpGet]
        public ViewResult QuestionnaireCompleted(int customerId, int languageId)
        {
            var html = _infoService.GetInfoText(4, customerId, languageId).Name;
            return View("QuestionnaireCompleted", model: html);
        }
    }
}