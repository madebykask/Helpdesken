﻿namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
  
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;    
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Repositories.Questionnaire;


    public class QuestionnaireQuestionOptionService : IQestionnaireQuestionOptionService
    {
        #region Fields        

        private readonly  IQuestionnaireQuestionOptionRepository _questionnaireQuestionOptionRepository;

        #endregion

        #region Constructors and Destructors

        public QuestionnaireQuestionOptionService(IQuestionnaireQuestionOptionRepository questionnaireQuestionOptionRepository)
        {
            this._questionnaireQuestionOptionRepository = questionnaireQuestionOptionRepository;            
        }

        #endregion

        #region Public Methods and Operators

        public void AddQuestionnaireQuestionOption(QuestionnaireQuesOption newQuestionnaireQuesOption)
        {            
            this._questionnaireQuestionOptionRepository.AddQuestionOption(
                    newQuestionnaireQuesOption);            
            this._questionnaireQuestionOptionRepository.Commit();
        }        

        public List<QuestionnaireQuesOption> FindQuestionnaireQuestionOptions(int questionId, int languageId)
        {            
            return this._questionnaireQuestionOptionRepository.FindQuestionnaireQuestionOptions(questionId, languageId, LanguageId.Swedish);                         
        }

        public void DeleteQuestionnaireQuestionOptionById(int optionId, int languageId)
        {
            if (languageId == LanguageId.Swedish)
                this._questionnaireQuestionOptionRepository.DeleteQuestionOptionById(optionId);
            else
                this._questionnaireQuestionOptionRepository.DeleteQuestionOptionById(optionId,languageId);

            this._questionnaireQuestionOptionRepository.Commit();
        }
        
        public void UpdateQuestionnaireQuestionOption(QuestionnaireQuesOption option)
        {
            switch (option.LanguageId)
            {
                case LanguageId.Swedish:
                    this._questionnaireQuestionOptionRepository.UpdateQuestionOption(option);
                    break;

                default:
                    this._questionnaireQuestionOptionRepository.UpdateQuestionOption(option, option.LanguageId);
                    break;
            }

            this._questionnaireQuestionOptionRepository.Commit();
        }

        #endregion
    }
}