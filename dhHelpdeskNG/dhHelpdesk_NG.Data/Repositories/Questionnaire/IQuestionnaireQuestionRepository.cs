namespace DH.Helpdesk.Dal.Repositories.Questionnaire
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IQuestionnaireQuestionRepository : INewRepository
    {
        #region Public Methods and Operators

        void AddSwedishQuestionnaire(NewQuestionnaire questionnaire);

        void DeleteById(int questionnaireId);

        //List<QuestionnaireQuestionsOverview> FindQuestionnaireQuestionsOverviews(int customerId);

        List<QuestionnaireQuestionsOverview> FindQuestionnaireQuestionsLanguage(int questionnaireId,int languageId, int defualtLanguageId);

        EditQuestionnaire GetQuestionnaireById(int id, int languageId);

        void UpdateOtherLanguageQuestionnaire(EditQuestionnaire questionnaire);

        void UpdateSwedishQuestionnaire(EditQuestionnaire questionnaire);

        #endregion
    }
}