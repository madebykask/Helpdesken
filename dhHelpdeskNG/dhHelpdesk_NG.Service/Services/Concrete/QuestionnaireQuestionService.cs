using System;

namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Questionnaire;
    using System.Globalization;

    public class QuestionnaireQuestionService : IQestionnaireQuestionService
    {
        #region Fields

        private readonly ILanguageRepository languageRepository;

        private readonly IQuestionnaireQuestionRepository questionnaireQuestionRepository;

        #endregion

        #region Constructors and Destructors

        public QuestionnaireQuestionService(
            IQuestionnaireQuestionRepository questionnaireQuestionRepository,
            ILanguageRepository languageRepository)
        {
            this.questionnaireQuestionRepository = questionnaireQuestionRepository;
            this.languageRepository = languageRepository;
        }

        #endregion

        #region Public Methods and Operators

        public void AddQuestionnaireQuestion(NewQuestionnaireQuestion newQuestionnaireQuestion)
        {
            this.questionnaireQuestionRepository.AddSwedishQuestionnaireQuestion(newQuestionnaireQuestion);
            this.questionnaireQuestionRepository.Commit();
        }

        public List<ItemOverview> FindActiveLanguageOverivews()
        {
            var overviews = this.languageRepository.GetAll().Select(l => new { Name = l.Name, Value = l.Id.ToString() }).ToList();
            return
               overviews.Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public List<QuestionnaireQuestionsOverview> FindQuestionnaireQuestionsOverviews(int questionnaireId, int languageId)
        {
            List<QuestionnaireQuestionsOverview> ret = null;
            ret = this.questionnaireQuestionRepository.FindQuestionnaireQuestions(questionnaireId,languageId,LanguageId.Swedish);            
            return ret;
        }

        public EditQuestionnaireQuestion GetQuestionnaireQuestionById(int questionId, int languageId)
        {            
            return this.questionnaireQuestionRepository.GetQuestionnaireQuestionById(questionId, languageId);
        }

        public void UpdateQuestionnaireQuestion(EditQuestionnaireQuestion editedQuestionnaireQuestion)
        {
            switch (editedQuestionnaireQuestion.LanguageId)
            {
                case LanguageId.Swedish:
                    this.questionnaireQuestionRepository.UpdateSwedishQuestionnaireQuestion(editedQuestionnaireQuestion);
                    break;

                default:
                    this.questionnaireQuestionRepository.UpdateOtherLanguageQuestionnaireQuestion(editedQuestionnaireQuestion);
                    break;
            }

            this.questionnaireQuestionRepository.Commit();
        }

        #endregion
    }
}