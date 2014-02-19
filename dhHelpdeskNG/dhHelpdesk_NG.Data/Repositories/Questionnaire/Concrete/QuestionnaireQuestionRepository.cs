using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Dal.Repositories.Questionnaire.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;    
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Questionnaire;

    public sealed class QuestionnaireQuestionRepository : Repository, IQuestionnaireQuestionRepository
    {
        #region Constructors and Destructors

        public QuestionnaireQuestionRepository(IDatabaseFactory databaseFactory)
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
                QuestionnaireDescription = questionnaire.Description,
                Customer_Id = questionnaire.CustomerId,
                CreatedDate = questionnaire.CreatedDate
            };
            this.DbContext.Questionnaires.Add(questionnaireEntity);
            this.InitializeAfterCommit(questionnaire, questionnaireEntity);
        }

        public void DeleteById(int questionnaireId)
        {
            throw new global::System.NotImplementedException();
        }

        public EditQuestionnaire GetQuestionnaireById(int id, int languageId)
        {
            EditQuestionnaire ret = null;

            if (languageId == LanguageId.Swedish)
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
            else
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


            return ret;
        }

        //public List<QuestionnaireQuestionsOverview> FindQuestionnaireQuestionsOverviews(int questionnaireId)
        //{
        //    var questionnaireQuestions =
        //        this.DbContext.QuestionnaireQuestions.Where(q => q.Questionnaire_Id == questionnaireId)
        //            .Select(
        //                q => new { Id = q.Id, QuestionNumber = q.QuestionnaireQuestionNumber, Question = q.QuestionnaireQuestion })
        //            .ToList();

        //    return questionnaireQuestions.Select(q => new QuestionnaireQuestionsOverview(q.Id, q.QuestionNumber, q.Question)).ToList();
        //}

        public List<QuestionnaireQuestionsOverview> FindQuestionnaireQuestionsLanguage(int questionnaireId, int languageId, int defualtLanguageId )
        {
            // A: All Questionnaire Questions (Id,Number,Question)
            var allSwedishQuestions =
                this.DbContext.QuestionnaireQuestions.Where(q => q.Questionnaire_Id == questionnaireId)
                    .Select(
                        q => new { Id = q.Id, Number = q.QuestionnaireQuestionNumber, Question = q.QuestionnaireQuestion, LanguageId = defualtLanguageId })
                    .ToList();

            // All Questionnaire-Language Questions
            var allOtherLanguageQuestions =
               this.DbContext.QuestionnaireQuestionLanguage
                   .Where(q => q.Language_Id == languageId)
                   .Select(
                       q => new { Id = q.QuestionnaireQuestion_Id, Number = q.QuestionnaireQuestions.QuestionnaireQuestionNumber, Question = q.QuestionnaireQuestion, LanguageId = languageId })
                   .ToList();

            // B: Only Needed Language Questions
            var pureLanguageQuestions =
                     allOtherLanguageQuestions
                     .Where(ql => allSwedishQuestions.Select(qq => qq.Id)
                                                     .Contains(ql.Id))
                    .ToList();

            // temp: A contain B
            var tempList =
                allSwedishQuestions.Where(
                    sq => pureLanguageQuestions.Select(lq => lq.Id)
                                               .Contains(sq.Id));
            // C: A - temp  
            var exceptList = allSwedishQuestions.Except(tempList);

            // E: B + C
            var fullQuestionList = pureLanguageQuestions.Union(exceptList);
            
            return fullQuestionList.Select(q => new QuestionnaireQuestionsOverview(q.Id, q.Number, q.Question, q.LanguageId)).ToList();            
            
        }

        public void UpdateSwedishQuestionnaire(EditQuestionnaire questionnaire)
        {
            var questionnaireEntity = this.DbContext.Questionnaires.Find(questionnaire.Id);

            questionnaireEntity.QuestionnaireName = questionnaire.Name;
            questionnaireEntity.QuestionnaireDescription = questionnaire.Description;
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