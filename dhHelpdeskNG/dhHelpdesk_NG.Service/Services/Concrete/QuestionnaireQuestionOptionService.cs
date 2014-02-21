using System;

namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;    
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Questionnaire;
    using System.Globalization;

    public class QuestionnaireQuestionOptionService : IQestionnaireQuestionOptionService
    {
        #region Fields        

        private readonly  IQuestionnaireQuestionOptionRepository _questionnaireQuestionOptionRepository;

        #endregion

        #region Constructors and Destructors

        public QuestionnaireQuestionOptionService(
            IQuestionnaireQuestionOptionRepository questionnaireQuestionOptionRepository
            )
        {
            this._questionnaireQuestionOptionRepository = questionnaireQuestionOptionRepository;            
        }

        #endregion

        #region Public Methods and Operators

        //public void AddQuestionnaireQuestion(NewQuestionnaireQuestion newQuestionnaireQuestion)
        //{
        //    this.questionnaireQuestionRepository.AddSwedishQuestionnaireQuestion(newQuestionnaireQuestion);
        //    this.questionnaireQuestionRepository.Commit();
        //}        

        public List<QuestionnaireQuesOption> FindQuestionnaireQuestionOptions(int questionId, int languageId)
        {            
            return this._questionnaireQuestionOptionRepository.FindQuestionnaireQuestionOptions(questionId, languageId, LanguageId.Swedish);                         
        }

        //public EditQuestionnaireQuestion GetQuestionnaireQuestionById(int questionId, int languageId)
        //{            
        //    return this.questionnaireQuestionRepository.GetQuestionnaireQuestionById(questionId, languageId);
        //}

        //public void UpdateQuestionnaireQuestion(EditQuestionnaireQuestion editedQuestionnaireQuestion)
        //{
        //    switch (editedQuestionnaireQuestion.LanguageId)
        //    {
        //        case LanguageId.Swedish:
        //            this.questionnaireQuestionRepository.UpdateSwedishQuestionnaireQuestion(editedQuestionnaireQuestion);
        //            break;

        //        default:
        //            this.questionnaireQuestionRepository.UpdateOtherLanguageQuestionnaireQuestion(editedQuestionnaireQuestion);
        //            break;
        //    }

        //    this.questionnaireQuestionRepository.Commit();
        //}

        #endregion
    }
}