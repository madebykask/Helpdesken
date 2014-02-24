

namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;

    public interface IQestionnaireQuestionOptionService
    {
        List<QuestionnaireQuesOption> FindQuestionnaireQuestionOptions(int questionId, int languageId);

        void AddQuestionnaireQuestionOption(QuestionnaireQuesOption newQuestionnaireQuesOption);

        void UpdateQuestionnaireQuestionOption(QuestionnaireQuesOption option);

        void DeleteQuestionnaireQuestionOptionById(int optionId, int languageId);

        //void UpdateQuestionnaire(EditQuestionnaire editedQuestionnaire);

        //EditQuestionnaire GetQuestionnaireById(int id, int languageId);
        
    }
}
