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

        public void AddSwedishQuestionnaireQuestion(NewQuestionnaireQuestion questionnaireQuestion)
        {
            var questionnaireQuestionEntity = new QuestionnaireQuestionEntity()
            {
                Questionnaire_Id = questionnaireQuestion.QuestionnaireId,
                QuestionnaireQuestionNumber = questionnaireQuestion.QuestionNumber,
                QuestionnaireQuestion = questionnaireQuestion.Question,
                ShowNote = questionnaireQuestion.ShowNote,
                NoteText = questionnaireQuestion.NoteText ?? string.Empty,
                CreatedDate = questionnaireQuestion.CreatedDate,
                ChangedDate = questionnaireQuestion.CreatedDate
            };

            this.DbContext.QuestionnaireQuestions.Add(questionnaireQuestionEntity);
            this.InitializeAfterCommit(questionnaireQuestion, questionnaireQuestionEntity);
        }

        public void DeleteQuestionById(int questionId)
        {
            var questionOptionLanguages = this.DbContext.QuestionnaireQuestionOptionLanguage
                   .Where(ol => ol.QuestionnaireQuestionOptions.QuestionnaireQuestion_Id == questionId);

            foreach (var optionLanguage in questionOptionLanguages)
                this.DbContext.QuestionnaireQuestionOptionLanguage.Remove(optionLanguage);


            var questionOptions = this.DbContext.QuestionnaireQuestionOptions
                   .Where(o => o.QuestionnaireQuestion_Id == questionId);

            foreach (var option in questionOptions)
                this.DbContext.QuestionnaireQuestionOptions.Remove(option);


            var questionLanguages = this.DbContext.QuestionnaireQuestionLanguage
                   .Where(q => q.QuestionnaireQuestion_Id == questionId);

            foreach (var questionLanguage in questionLanguages)
                this.DbContext.QuestionnaireQuestionLanguage.Remove(questionLanguage);


            var question = this.DbContext.QuestionnaireQuestions.Find(questionId);
            if (question != null)
                this.DbContext.QuestionnaireQuestions.Remove(question); 

        }       

        public EditQuestionnaireQuestion GetQuestionnaireQuestionById(int questionId, int languageId)
        {
            EditQuestionnaireQuestion ret = null;

            if (languageId != LanguageIds.Swedish)
            {
                var questionnaireQuestion =
                    this.DbContext.QuestionnaireQuestionLanguage
                            .Where(l => l.QuestionnaireQuestion_Id == questionId && l.Language_Id == languageId)
                            .Select(
                                l =>
                                new
                                {
                                    Id = l.QuestionnaireQuestion_Id,
                                    QuestionnaireId = -1,  // it fill again in Controller
                                    LanguageId = l.Language_Id,
                                    QuestionNumber = l.QuestionnaireQuestions.QuestionnaireQuestionNumber,
                                    Question = l.QuestionnaireQuestion,
                                    ShowNote = l.QuestionnaireQuestions.ShowNote,
                                    NoteText = l.NoteText,
                                    CreateDate = l.CreatedDate
                                })
                            .FirstOrDefault();

                if (questionnaireQuestion != null)
                    ret = new EditQuestionnaireQuestion(questionnaireQuestion.Id, questionnaireQuestion.QuestionnaireId, questionnaireQuestion.LanguageId, questionnaireQuestion.QuestionNumber,
                                                      questionnaireQuestion.Question, questionnaireQuestion.ShowNote, questionnaireQuestion.NoteText, questionnaireQuestion.CreateDate);
            }

            if (ret == null)
            {
                var questionnaireQuestion =
                    this.DbContext.QuestionnaireQuestions.Where(q => q.Id == questionId)
                        .Select(
                            q =>
                            new
                            {
                                Id = q.Id,
                                QuestionnaireId = q.Questionnaire_Id,
                                LanguageId = LanguageIds.Swedish,
                                QuestionNumber = q.QuestionnaireQuestionNumber,
                                Question = q.QuestionnaireQuestion,
                                ShowNote = q.ShowNote,
                                NoteText = q.NoteText,
                                CreateDate = q.CreatedDate
                            })
                        .FirstOrDefault();
                if (questionnaireQuestion != null)
                    ret = new EditQuestionnaireQuestion(questionnaireQuestion.Id, questionnaireQuestion.QuestionnaireId, questionnaireQuestion.LanguageId, questionnaireQuestion.QuestionNumber,
                                                        questionnaireQuestion.Question, questionnaireQuestion.ShowNote, questionnaireQuestion.NoteText, questionnaireQuestion.CreateDate);
            }

            return ret;
        }
       

        public List<QuestionnaireQuestionsOverview> FindQuestionnaireQuestions(int questionnaireId, int languageId, int defualtLanguageId )
        {
            // All Questionnaire Questions (Id,Number,Question)
            var allSwedishQuestions =
                this.DbContext.QuestionnaireQuestions.Where(q => q.Questionnaire_Id == questionnaireId)
                    .Select(
                        q => new { Id = q.Id, Number = q.QuestionnaireQuestionNumber, 
                                   Question = q.QuestionnaireQuestion, LanguageId = defualtLanguageId,
                                   ShowNote = q.ShowNote, NoteText = q.NoteText 
                                 }
                           )
                    .ToList();

            // All Questionnaire-Language Questions
            var allOtherLanguageQuestions =
               this.DbContext.QuestionnaireQuestionLanguage
                   .Where(q => q.Language_Id == languageId && q.QuestionnaireQuestions.Questionnaire_Id == questionnaireId)
                   .Select(
                       q => new { Id = q.QuestionnaireQuestion_Id, Number = q.QuestionnaireQuestions.QuestionnaireQuestionNumber, 
                                  Question = q.QuestionnaireQuestion, LanguageId = languageId,
                                  ShowNote = q.QuestionnaireQuestions.ShowNote, NoteText = q.NoteText
                                })
                   .ToList();

            // Only Needed Language Questions
            var pureLanguageQuestions =
                     allOtherLanguageQuestions
                     .Where(ql => allSwedishQuestions.Select(qq => qq.Id)
                                                     .Contains(ql.Id))
                    .ToList();

            
            var tempList =
                allSwedishQuestions.Where(
                    sq => pureLanguageQuestions.Select(lq => lq.Id)
                                               .Contains(sq.Id));
            // Swedish Questions which there was not in Question Languags 
            var exceptList = allSwedishQuestions.Except(tempList);

                        
            var fullQuestionList = pureLanguageQuestions.Union(exceptList);
            
            return fullQuestionList.Select(q => new QuestionnaireQuestionsOverview(q.Id, q.Number, q.Question, q.ShowNote, q.NoteText, q.LanguageId)).ToList();            
            
        }

        public void UpdateSwedishQuestionnaireQuestion(EditQuestionnaireQuestion questionnaireQuestion)
        {
            var questionnaireQuestionEntity = this.DbContext.QuestionnaireQuestions.Find(questionnaireQuestion.Id);

            questionnaireQuestionEntity.Questionnaire_Id = questionnaireQuestion.QuestionnaireId;
            questionnaireQuestionEntity.QuestionnaireQuestionNumber = questionnaireQuestion.QuestionNumber;
            questionnaireQuestionEntity.QuestionnaireQuestion = questionnaireQuestion.Question;
            questionnaireQuestionEntity.ShowNote = questionnaireQuestion.ShowNote;
            questionnaireQuestionEntity.NoteText = questionnaireQuestion.NoteText ?? string.Empty;            
            questionnaireQuestionEntity.ChangedDate = questionnaireQuestion.ChangeDate;
        }

        public void UpdateOtherLanguageQuestionnaireQuestion(EditQuestionnaireQuestion questionnaireQuestion)
        {
            var questionnaireQuestionLanguageEntity =
                this.DbContext.QuestionnaireQuestionLanguage.SingleOrDefault(
                    l => l.QuestionnaireQuestion_Id == questionnaireQuestion.Id && l.Language_Id == questionnaireQuestion.LanguageId);

            if (questionnaireQuestionLanguageEntity != null)
            {
                questionnaireQuestionLanguageEntity.QuestionnaireQuestion = questionnaireQuestion.Question;
                questionnaireQuestionLanguageEntity.NoteText = questionnaireQuestion.NoteText;
                questionnaireQuestionLanguageEntity.ChangedDate = questionnaireQuestion.ChangeDate;
            }
            else
            {
                var newQuestionnaireQuestionLanguageEntity = new QuestionnaireQuesLangEntity
                {
                    QuestionnaireQuestion_Id = 
                        questionnaireQuestion.Id,
                    Language_Id = 
                        questionnaireQuestion.LanguageId,
                    QuestionnaireQuestion = 
                        questionnaireQuestion.Question,
                    NoteText = 
                        questionnaireQuestion.NoteText,                    
                    CreatedDate =
                        questionnaireQuestion.ChangeDate,
                    ChangedDate = 
                        questionnaireQuestion.ChangeDate
                };

                this.DbContext.QuestionnaireQuestionLanguage.Add(newQuestionnaireQuestionLanguageEntity);
            }
        }

        #endregion
    }
}