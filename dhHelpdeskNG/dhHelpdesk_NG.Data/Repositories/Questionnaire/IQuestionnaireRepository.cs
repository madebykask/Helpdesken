namespace DH.Helpdesk.Dal.Repositories.Questionnaire
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Questionnaire;

    public interface IQuestionnaireRepository : IRepository<QuestionnaireEntity>
    {
        #region Public Methods and Operators

        List<QuestionnaireOverview> FindOverviews(int customerId);

        void DeleteById(int questionnaireId);
        
        #endregion
    }
}