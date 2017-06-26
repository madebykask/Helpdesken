namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;

    public interface IQestionnaireQuestionOptionService
    {
        List<QuestionnaireQuesOption> FindQuestionnaireQuestionOptions(int questionId, int languageId);

        void AddQuestionnaireQuestionOption(QuestionnaireQuesOption newQuestionnaireQuesOption);

        void UpdateQuestionnaireQuestionOption(QuestionnaireQuesOption option);

        void DeleteQuestionnaireQuestionOptionById(int optionId, int languageId);

        void UpdateQuestionnaireQuestionOptionIcon(int optionId, byte[] iconSrc);
    }
}
