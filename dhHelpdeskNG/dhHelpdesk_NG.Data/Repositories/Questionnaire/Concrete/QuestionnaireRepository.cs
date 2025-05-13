using System.Data.Entity;
using System.Runtime.Remoting.Messaging;
using DH.Helpdesk.Dal.Enums;

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
				Type = questionnaire.Type,
                QuestionnaireName = questionnaire.Name,
                QuestionnaireDescription = questionnaire.Description ?? string.Empty,
                Customer_Id = questionnaire.CustomerId,
                CreatedDate = questionnaire.CreatedDate,
				Identifier = questionnaire.Identifier,
				ExcludeAdministrators = questionnaire.ExcludeAdministrators,
                UseBase64Images = questionnaire.UseBase64Images,
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

            if (languageId != LanguageIds.Swedish)
            {
               var questionnaireLng =
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
	            if (questionnaireLng != null)
	            {
		            ret = new EditQuestionnaire(questionnaireLng.Questionnaire_Id, questionnaireLng.Name,
						questionnaireLng.Description, questionnaireLng.languageId, questionnaireLng.CreateDate);
	            }
            }

            var questionnaire =
                this.DbContext.Questionnaires.Where(q => q.Id == id)
                    .Select(
                        q =>
                        new
                        {
                            q.Id,
                            Name = q.QuestionnaireName,
                            Description = q.QuestionnaireDescription,
							Identifier = q.Identifier,
                            languageId = LanguageIds.Swedish,
                            CreateDate = q.CreatedDate,
                            q.ExcludeAdministrators,
                            q.UseBase64Images
                        }).FirstOrDefault();
			// If Language = Swedish or there is no Questionnaire for this language_Id
	        if (questionnaire != null)
	        {
		        if (ret == null)
		        {
			        ret = new EditQuestionnaire(questionnaire.Id, questionnaire.Name, questionnaire.Description,
				        questionnaire.languageId, questionnaire.CreateDate);
		        }
		        ret.Identifier = questionnaire.Identifier;
		        ret.ExcludeAdministrators = questionnaire.ExcludeAdministrators;
                ret.UseBase64Images = questionnaire.UseBase64Images;
            }

	        return ret;
        }

        public List<QuestionnaireOverview> FindQuestionnaireOverviews(int customerId)
        {
            var questionnaires =
                this.DbContext.Questionnaires
					.Where(q => q.Customer_Id == customerId && q.Type == QuestionnaireType.Questionnaire)
                    .Select(q => new { Id = q.Id, Name = q.QuestionnaireName, Description = q.QuestionnaireDescription })
                    .ToList();

            return questionnaires.Select(q => new QuestionnaireOverview(q.Id, q.Name, q.Description)).ToList();
        }

		public List<QuestionnaireEntity> FindFeedbackOverviews(int customerId)
		{
			return DbContext.Questionnaires
				.Where(q => q.Customer_Id == customerId && q.Type == QuestionnaireType.Feedback)
				.ToList();
		}

	    public List<QuestionnaireEntity> GetFeedbackFullItems(int customerId, IEnumerable<string> identifiers = null)
	    {
		    var query = DbContext.Questionnaires.AsNoTracking()
			    .Where(q => q.Customer_Id == customerId && q.Type == QuestionnaireType.Feedback)
			    .Include(q => q.QuestionnaireQuestionEntities)
			    .Include(q => q.QuestionnaireQuestionEntities.Select(qe => qe.QuestionnaireQuesLangEntities))
			    .Include(q => q.QuestionnaireQuestionEntities.Select(qe => qe.QuestionnaireQuestionOptionEntities))
			    .Include(
				    q =>
					    q.QuestionnaireQuestionEntities.Select(
						    qe => qe.QuestionnaireQuestionOptionEntities.Select(qeo => qeo.QuestionnaireQuesOpLangEntities)))
			    .Include(q => q.QuestionnaireLanguageEntities);

		    if (identifiers != null)
			    query = query.Where(q => identifiers.Contains(q.Identifier));

			return query.ToList();
		}

		public void UpdateSwedishQuestionnaire(EditQuestionnaire questionnaire)
        {
            var questionnaireEntity = this.DbContext.Questionnaires.Find(questionnaire.Id);

            questionnaireEntity.QuestionnaireName = questionnaire.Name;
			questionnaireEntity.Identifier = questionnaire.Identifier;
			questionnaireEntity.ExcludeAdministrators = questionnaire.ExcludeAdministrators;
			questionnaireEntity.QuestionnaireDescription = questionnaire.Description ?? string.Empty;
            questionnaireEntity.ChangedDate = questionnaire.ChangedDate;
            questionnaireEntity.UseBase64Images = questionnaire.UseBase64Images;
        }

        public void UpdateOtherLanguageQuestionnaire(EditQuestionnaire questionnaire)
        {
			var questionnaireEntity =
				DbContext.Questionnaires.SingleOrDefault(
					l => l.Id == questionnaire.Id);

			if (questionnaireEntity != null)
			{
				questionnaireEntity.Identifier = questionnaire.Identifier;
				questionnaireEntity.ExcludeAdministrators = questionnaire.ExcludeAdministrators;
                questionnaireEntity.UseBase64Images = questionnaire.UseBase64Images;
            }

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