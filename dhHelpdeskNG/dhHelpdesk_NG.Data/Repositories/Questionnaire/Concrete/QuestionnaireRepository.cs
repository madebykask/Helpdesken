namespace DH.Helpdesk.Dal.Repositories.Questionnaire.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Questionnaire;

    public sealed class QuestionnaireRepository : Repository, IQuestionnaireRepository
    {
        #region Constructors and Destructors

        public QuestionnaireRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        #region Public Methods and Operators

        public void AddSwedishQuestionnaire(NewQuestionnaire questionnaire)
        {
            var questionnaireEntity = new QuestionnaireEntity
            {
                QuestionnaireName = questionnaire.Name,
                QuestionnaireDescription = questionnaire.Description ?? string.Empty,
                Customer_Id = questionnaire.CustomerId,
                CreatedDate = questionnaire.CreatedDate
            };
            this.DbContext.Questionnaires.Add(questionnaireEntity);
            this.InitializeAfterCommit(questionnaire, questionnaireEntity);
        }

        public void DeleteQuestionnaireById(int questionnaireId)
        {
            var questionnaireLanguages = this.DbContext.QuestionnaireLanguages.Where(ql => ql.Questionnaire_Id == questionnaireId);

            foreach (var questionnaireLanguage in questionnaireLanguages)            
                this.DbContext.QuestionnaireLanguages.Remove(questionnaireLanguage);
            
            var questionnaire = this.DbContext.Questionnaires.Find(questionnaireId);
            if (questionnaire != null)
                this.DbContext.Questionnaires.Remove(questionnaire);    
        }

        public EditQuestionnaire GetQuestionnaireById(int id, int languageId)
        {
            EditQuestionnaire ret = null;             

            if (languageId != LanguageId.Swedish)
            {
               var questionnaires =
                    this.DbContext.QuestionnaireLanguages.Where(l => l.Questionnaire_Id == id && l.Language_Id == languageId).Select(
                            l =>
                            new
                            {
                                l.Questionnaire_Id,
                                Name = l.QuestionnaireName,
                                Description = l.QuestionnaireDescription,
                                languageId = l.Language_Id,
                                CreateDate = l.CreatedDate
                            }).FirstOrDefault();
                if (questionnaires != null)
                    ret = new EditQuestionnaire(questionnaires.Questionnaire_Id, questionnaires.Name, questionnaires.Description, questionnaires.languageId, questionnaires.CreateDate);
            }

            // If Language = Swedish or there is no Questionnaire for this language_Id
            if (ret == null) 
            {
                var questionnaires =
                    this.DbContext.Questionnaires.Where(q => q.Id == id)
                        .Select(
                            q =>
                            new
                            {
                                q.Id,
                                Name = q.QuestionnaireName,
                                Description = q.QuestionnaireDescription,
                                languageId = LanguageId.Swedish,
                                CreateDate = q.CreatedDate
                            }).FirstOrDefault();
                if (questionnaires != null)
                  ret = new EditQuestionnaire(questionnaires.Id, questionnaires.Name, questionnaires.Description, questionnaires.languageId, questionnaires.CreateDate);
            }

            return ret;
        }

        public List<QuestionnaireOverview> FindQuestionnaireOverviews(int customerId)
        {
            var questionnaires =
                this.DbContext.Questionnaires.Where(q => q.Customer_Id == customerId)
                    .Select(
                        q => new { Id = q.Id, Name = q.QuestionnaireName, Description = q.QuestionnaireDescription })
                    .ToList();

            return questionnaires.Select(q => new QuestionnaireOverview(q.Id, q.Name, q.Description)).ToList();
        }

        public void UpdateSwedishQuestionnaire(EditQuestionnaire questionnaire)
        {
            var questionnaireEntity = this.DbContext.Questionnaires.Find(questionnaire.Id);

            questionnaireEntity.QuestionnaireName = questionnaire.Name;
            questionnaireEntity.QuestionnaireDescription = questionnaire.Description ?? string.Empty;
            questionnaireEntity.ChangedDate = questionnaire.ChangedDate;
        }

        public void UpdateOtherLanguageQuestionnaire(EditQuestionnaire questionnaire)
        {
            var questionnaireLanguageEntity =
                this.DbContext.QuestionnaireLanguages.SingleOrDefault(
                    l => l.Questionnaire_Id == questionnaire.Id && l.Language_Id == questionnaire.LanguageId);

            if (questionnaireLanguageEntity != null)
            {
                questionnaireLanguageEntity.QuestionnaireName = questionnaire.Name;
                questionnaireLanguageEntity.QuestionnaireDescription = questionnaire.Description;
                questionnaireLanguageEntity.ChangedDate = questionnaire.ChangedDate;
            }
            else
            {
                var newquestionnaireLanguageEntity = new QuestionnaireLanguageEntity
                {
                    Questionnaire_Id =
                        questionnaire.Id,
                    QuestionnaireName =
                        questionnaire.Name,
                    QuestionnaireDescription =
                        questionnaire.Description,
                    Language_Id =
                        questionnaire.LanguageId,
                    CreatedDate =
                        questionnaire.ChangedDate
                };

                this.DbContext.QuestionnaireLanguages.Add(newquestionnaireLanguageEntity);
            }
        }

        #endregion
    }
}