namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;    

    public interface IQestionnaireService
    {
        List<QuestionnaireOverview> FindQuestionnaireOverviews(int customerId);
    }
}
