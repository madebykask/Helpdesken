namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;

    public interface IQestionnaireService
    {
        List<QuestionnaireOverview> FindQuestionnaireOverviews(int customerId);

        void AddQuestionnaire(NewQuestionnaire newQuestionnaire);

        void UpdateQuestionnaire(EditQuestionnaire editedQuestionnaire);

        EditQuestionnaire GetQuestionnaireById(int id, int languageId);

        List<ItemOverview> FindActiveLanguageOverivews();
    }
}
