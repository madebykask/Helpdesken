namespace DH.Helpdesk.Dal.Repositories.Questionnaire
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IQuestionnaireQuestionOptionRepository : INewRepository
    {
        #region Public Methods and Operators

        List<QuestionnaireQuesOption> FindQuestionnaireQuestionOptions(int questionId, int languageId, int defualtLanguageId);

        //void AddSwedishQuestionnaireQuestion(NewQuestionnaireQuestion questionnaireQuestion);        

        //void DeleteById(int questionnaireId);

        //EditQuestionnaireQuestion GetQuestionnaireQuestionById(int questionId, int languageId);

        //void UpdateOtherLanguageQuestionnaireQuestion(EditQuestionnaireQuestion questionnaireQuestion);

        //void UpdateSwedishQuestionnaireQuestion(EditQuestionnaireQuestion questionnaireQuestion);

        #endregion
    }
}