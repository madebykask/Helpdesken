
namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;

    public interface IQestionnaireQuestionService
    {
        List<QuestionnaireQuestionsOverview> FindQuestionnaireQuestionsOverviews(int questionnaireId, int laguageId);

        void AddQuestionnaireQuestion(NewQuestionnaireQuestion newQuestionnaireQuestion);

        void UpdateQuestionnaireQuestion(EditQuestionnaireQuestion editedQuestionnaireQuestion);

        void DeleteQuestionnaireQuestionById(int questionId);

        EditQuestionnaireQuestion GetQuestionnaireQuestionById(int id, int languageId);

        List<ItemOverview>FindActiveLanguageOverivews();
    }
}
