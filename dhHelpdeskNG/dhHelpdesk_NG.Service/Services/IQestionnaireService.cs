using DH.Helpdesk.Domain.Questionnaire;

namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;

    public interface IQestionnaireService
    {
        List<QuestionnaireOverview> FindQuestionnaireOverviews(int customerId);

		void AddQuestionnaire(NewQuestionnaire newQuestionnaire);

        void UpdateQuestionnaire(EditQuestionnaire editedQuestionnaire);

        void DeleteQuestionnaireById(int questionnaireId);        

        EditQuestionnaire GetQuestionnaireById(int id, int languageId);
    }
}
