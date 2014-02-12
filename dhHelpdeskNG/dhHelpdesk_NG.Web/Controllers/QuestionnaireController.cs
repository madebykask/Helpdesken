namespace DH.Helpdesk.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models.Questionnaire;

    public class QuestionnaireController : BaseController
    {
        #region Fields

        private readonly IQestionnaireService questionnaireService;

        #endregion

        #region Constructors and Destructors

        public QuestionnaireController(
            IQestionnaireService questionnaireService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this.questionnaireService = questionnaireService;
        }

        #endregion

        #region Public Methods and Operators

        [HttpGet]
        public ViewResult Index()
        {
            var questionnaires = this.questionnaireService.FindQuestionnaireOverviews(SessionFacade.CurrentCustomer.Id);

            var model = new List<QuestionnaireOverviewModel>();

            foreach (var questionnaireOverview in questionnaires)
            {
                var tmp = new QuestionnaireOverviewModel(
                    id: questionnaireOverview.Id, 
                    name: questionnaireOverview.Name, 
                    description: questionnaireOverview.Description);

                model.Add(tmp);
            }
            
            return this.View(model);
        }

        #endregion

        #region Methods
       
        #endregion
    }
}