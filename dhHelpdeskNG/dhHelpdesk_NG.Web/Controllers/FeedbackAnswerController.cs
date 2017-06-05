using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                NoteTextLabel = question.NoteText,
                OptionId = parameters.OptionId
            };
            var ids = new List<Answer> { new Answer("", parameters.OptionId) };

            var participant = new ParticipantForInsert(model.Guid, model.IsAnonym, DateTime.Now, ids, true);
            var questionId = _circularService.SaveAnswers(participant);

            return RedirectToAction("ThankYou", new RouteValueDictionary(new { customerId = parameters.CustomerId, languageId = parameters.LanguageId, noteLabel = model.NoteTextLabel, isShowNote = model.IsShowNote, questionId }));
        }

        [AllowAnonymous]
        public ActionResult ThankYou(int customerId, int languageId, bool isShowNote = false, string noteLabel = null, int questionId = 0)
        {
            var html = _infoService.GetInfoText(4, customerId, languageId).Name;
            var model = new FeedbackThankYouModel
            {
                Html = html,
                NoteLabel = noteLabel,
                IsShowNote = isShowNote,
                LanguageId = languageId,
                CustomerId = customerId,
                QuestionId = questionId
            };

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult SaveComment(int customerId, int languageId, int questionId, string noteText)
        {
            if (questionId > 0 && !string.IsNullOrEmpty(noteText))
            {
                _circularService.SaveFeedbackNote(questionId, noteText);
            }
            return Json(new { success = true });
        }
    }
}