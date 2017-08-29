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

        void AddQuestionOption(QuestionnaireQuesOption newOption);

        void UpdateQuestionOption(QuestionnaireQuesOption option);

        void UpdateQuestionOption(QuestionnaireQuesOption option, int languageId);

        void DeleteQuestionOptionById(int optionId);        

        void DeleteQuestionOptionById(int optionId, int languageId);                

        #endregion

        void UpdateQuestionnaireQuestionOptionIcon(int optionId, byte[] iconSrc, string fileName);
        QuestionnaireQuesOption GetQuestionnaireQuestionOption(int optionId);
    }
}