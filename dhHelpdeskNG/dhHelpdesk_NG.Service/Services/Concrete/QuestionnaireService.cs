namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Questionnaire;
    using System.Globalization;

    public class QuestionnaireService : IQestionnaireService
    {
        #region Fields

        private readonly ILanguageRepository _languageRepository;

        private readonly IQuestionnaireRepository _questionnaireRepository;

        private readonly IQuestionnaireQuestionRepository _questionnaireQuestionRepository;

        #endregion

        #region Constructors and Destructors

        public QuestionnaireService(
            IQuestionnaireRepository questionnaireRepository,
            IQuestionnaireQuestionRepository questionnaireQuestionRepository,
            ILanguageRepository languageRepository)
        {
            this._questionnaireRepository = questionnaireRepository;
            this._languageRepository = languageRepository;
            this._questionnaireQuestionRepository = questionnaireQuestionRepository;
        }

        #endregion

        #region Public Methods and Operators

        public void AddQuestionnaire(NewQuestionnaire newQuestionnaire)
        {
            this._questionnaireRepository.AddSwedishQuestionnaire(newQuestionnaire);
            this._questionnaireRepository.Commit();
        }

        public List<QuestionnaireOverview> FindQuestionnaireOverviews(int customerId)
        {
            return this._questionnaireRepository.FindQuestionnaireOverviews(customerId);
        }

        public EditQuestionnaire GetQuestionnaireById(int id, int languageId)
        {
            return this._questionnaireRepository.GetQuestionnaireById(id, languageId);                        
        }

        public void UpdateQuestionnaire(EditQuestionnaire editedQuestionnaire)
        {
            switch (editedQuestionnaire.LanguageId)
            {
                case LanguageIds.Swedish:
                    this._questionnaireRepository.UpdateSwedishQuestionnaire(editedQuestionnaire);
                    break;

                default:
                    this._questionnaireRepository.UpdateOtherLanguageQuestionnaire(editedQuestionnaire);
                    break;
            }

            this._questionnaireRepository.Commit();
        }

        public void DeleteQuestionnaireById(int questionnaireId)
        {
            var questions = this._questionnaireQuestionRepository.FindQuestionnaireQuestions(questionnaireId, LanguageIds.Swedish,
                LanguageIds.Swedish);

            foreach (var question in questions)
                this._questionnaireQuestionRepository.DeleteQuestionById(question.Id);

            this._questionnaireQuestionRepository.Commit();


            this._questionnaireRepository.DeleteQuestionnaireById(questionnaireId);
            this._questionnaireRepository.Commit();
        }

        #endregion
    }
}