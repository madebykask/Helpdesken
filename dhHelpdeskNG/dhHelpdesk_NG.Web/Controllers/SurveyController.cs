namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models;

    public class SurveyController : BaseController
    {
        private readonly ISurveyService surveyService;

        private readonly ICaseService caseService;

        public SurveyController(ISurveyService surveyService, ICaseService caseService, IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this.surveyService = surveyService;
            this.caseService = caseService;
        }

        public ActionResult Vote(int id, string voteId)
        {
            var case_ = this.caseService.GetCaseById(id);
            if (case_ == null)
            {
                ViewBag.ErrorReason = "case does not exists";
                return this.View("VoteError");
            }

            ViewBag.CaseNumber = case_.CaseNumber;
            var survey = this.surveyService.GetByCaseId(id);
            if (survey != null)
            {
                ViewBag.ErrorReason = "allready voted";
                return this.View("VoteError");
            }
           
            if (!case_.IsClosed())
            {
                ViewBag.ErrorReason = "case is not closed";
                return this.View("VoteError");
            }

            survey = Survey.CreateFromString(id, voteId);
            this.surveyService.SaveSurvey(survey);
            var surveyViewModel = SurveyViewModel.CreateFromSurvey(survey);
            return this.View(surveyViewModel);
        }
    }
}
