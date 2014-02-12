namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Dal.Repositories.Questionnaire;

    public class QuestionnaireService : IQestionnaireService
    {
        #region Fields

        private readonly IQuestionnaireRepository questionnaireRepository;

        #endregion

        #region Constructors and Destructors

        public QuestionnaireService(IQuestionnaireRepository questionnaireRepository)
        {
            this.questionnaireRepository = questionnaireRepository;
        }

        #endregion

        #region Public Methods and Operators

        public List<QuestionnaireOverview> FindQuestionnaireOverviews(int customerId)
        {
            return this.questionnaireRepository.FindOverviews(customerId);
        }

        #endregion
    }
}