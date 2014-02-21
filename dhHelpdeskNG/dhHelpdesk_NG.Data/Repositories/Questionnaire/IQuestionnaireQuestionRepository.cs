namespace DH.Helpdesk.Dal.Repositories.Questionnaire
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IQuestionnaireQuestionRepository : INewRepository
    {
        #region Public Methods and Operators

        void AddSwedishQuestionnaireQuestion(NewQuestionnaireQuestion questionnaireQuestion);

        List<QuestionnaireQuestionsOverview> FindQuestionnaireQuestions(int questionnaireId, int languageId, int defualtLanguageId);

        void DeleteById(int questionnaireId);

        EditQuestionnaireQuestion GetQuestionnaireQuestionById(int questionId, int languageId);

        void UpdateOtherLanguageQuestionnaireQuestion(EditQuestionnaireQuestion questionnaireQuestion);

        void UpdateSwedishQuestionnaireQuestion(EditQuestionnaireQuestion questionnaireQuestion);

        #endregion
    }
}