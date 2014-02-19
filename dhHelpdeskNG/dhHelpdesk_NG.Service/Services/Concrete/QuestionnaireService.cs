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

    public class QuestionnaireService : IQestionnaireService
    {
        #region Fields

        private readonly ILanguageRepository languageRepository;

        private readonly IQuestionnaireRepository questionnaireRepository;

        #endregion

        #region Constructors and Destructors

        public QuestionnaireService(
            IQuestionnaireRepository questionnaireRepository,
            ILanguageRepository languageRepository)
        {
            this.questionnaireRepository = questionnaireRepository;
            this.languageRepository = languageRepository;
        }

        #endregion

        #region Public Methods and Operators

        public void AddQuestionnaire(NewQuestionnaire newQuestionnaire)
        {
            this.questionnaireRepository.AddSwedishQuestionnaire(newQuestionnaire);
            this.questionnaireRepository.Commit();
        }

        public List<ItemOverview> FindActiveLanguageOverivews()
        {
            var overviews = this.languageRepository.GetAll().Select(l => new { Name = l.Name, Value = l.Id.ToString() }).ToList();
            return
               overviews.Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public List<QuestionnaireOverview> FindQuestionnaireOverviews(int customerId)
        {
            return this.questionnaireRepository.FindQuestionnaireOverviews(customerId);
        }

        public EditQuestionnaire GetQuestionnaireById(int id, int languageId)
        {
            return this.questionnaireRepository.GetQuestionnaireById(id, languageId);                        
        }

        public void UpdateQuestionnaire(EditQuestionnaire editedQuestionnaire)
        {
            switch (editedQuestionnaire.LanguageId)
            {
                case LanguageId.Swedish:
                    this.questionnaireRepository.UpdateSwedishQuestionnaire(editedQuestionnaire);
                    break;

                default:
                    this.questionnaireRepository.UpdateOtherLanguageQuestionnaire(editedQuestionnaire);
                    break;
            }

            this.questionnaireRepository.Commit();
        }

        #endregion
    }
}