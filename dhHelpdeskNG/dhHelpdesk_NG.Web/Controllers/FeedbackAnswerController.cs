using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Models.Feedback;

namespace DH.Helpdesk.Web.Controllers
{
    [Authorize]
    public class FeedbackAnswerController : Controller
    {
        private readonly ICircularService _circularService;
        private readonly IInfoService _infoService;

        public FeedbackAnswerController(ICircularService circularService, IInfoService infoService)
        {
            _circularService = circularService;
            _infoService = infoService;
        }

        [AllowAnonymous]
        public ActionResult Answer(FeedbackAnswerParams parameters)
        {
            var detailed = _circularService.GetQuestionnaire(parameters.Guid, parameters.LanguageId);
            var question = detailed.Questionnaire.Questions.First();

            var model = new FeedbackAnswerModel
            {
                Guid = parameters.Guid,
                CaseId = detailed.CaseId,
                IsAnonym = false,
                IsShowNote = question.IsShowNote,
                NoteTextLabel = question.NoteText
            };

            var options = question.Options.First(o => o.Value == parameters.OptionValue);
            model.OptionId = options.Id;
            var ids = new List<Answer> {new Answer("", options.Id)};
            var participant = new ParticipantForInsert(model.Guid, model.IsAnonym, DateTime.Now, ids);
            _circularService.SaveAnswers(participant);

            return RedirectToAction("ThankYou", new RouteValueDictionary(new { customerId = parameters.CustomerId, languageId = parameters.LanguageId }));
        }

        [AllowAnonymous]
        public ActionResult ThankYou(int customerId, int languageId)
        {
            var html = _infoService.GetInfoText(4, customerId, languageId).Name;
            return View(model: html);
        }
    }
}