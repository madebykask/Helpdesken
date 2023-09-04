using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Models.Feedback;
using DH.Helpdesk.Web.Models.Questionnaire.Input;

namespace DH.Helpdesk.Web.Controllers
{
    [Authorize]
    public class FeedbackAnswerController : Controller
    {
        private readonly ICircularService _circularService;
        private readonly IInfoService _infoService;
        private readonly IQestionnaireQuestionOptionService _questionnaireQuestionOptionService;

        public FeedbackAnswerController(ICircularService circularService, IInfoService infoService, IQestionnaireQuestionOptionService qestionnaireQuestionOptionService)
        {
            _circularService = circularService;
            _infoService = infoService;
            _questionnaireQuestionOptionService = qestionnaireQuestionOptionService;
        }

        [AllowAnonymous]
        public ActionResult Answer(QuestionnaireAnswerParams parameters)
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
            var infoText = _infoService.GetInfoText(4, customerId, languageId);
            var html = string.Empty;
            if (infoText != null)
            {
                html = infoText.Name;
            }
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
                return Json(new { success = true});
            }
            return Json(new { success = false });
        }

        [AllowAnonymous]
        public void GetFeedbackImg(int id)
        {
            var requestContext = Request.RequestContext;
            var routeValues = requestContext.RouteData.Values;
            if (routeValues.ContainsKey("id"))
            {
                var option = _questionnaireQuestionOptionService.GetQuestionnaireQuestionOption(id);
                if (option.IconSrc != null && option.IconSrc.Length > 0)
                {
                    requestContext.HttpContext.Response.Clear();
                    requestContext.HttpContext.Response.AddHeader("Content-Disposition", "inline;attachment; filename=\"" + option.IconId + "\"");
                    requestContext.HttpContext.Response.AddHeader("Content-Length", option.IconSrc.Length.ToString());
                    requestContext.HttpContext.Response.ContentType = "application/octet-stream";
                    requestContext.HttpContext.Response.BinaryWrite(option.IconSrc);

                    if (requestContext.HttpContext.Response.IsClientConnected)
                    {
                        requestContext.HttpContext.Response.Flush();
                    }
                }
            }
        }
    }
}