using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Dal.Repositories.Questionnaire.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;    
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;    

    public sealed class QuestionnaireQuestionOptionRepository : Repository, IQuestionnaireQuestionOptionRepository
    {
        #region Constructors and Destructors

        public QuestionnaireQuestionOptionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        #region Public Methods and Operators

        //public void AddSwedishQuestionnaireQuestion(NewQuestionnaireQuestion questionnaireQuestion)
        //{
        //    var questionnaireQuestionEntity = new QuestionnaireQuestionEntity()
        //    {
        //        Questionnaire_Id = questionnaireQuestion.QuestionnaireId,
        //        QuestionnaireQuestionNumber = questionnaireQuestion.QuestionNumber,
        //        QuestionnaireQuestion = questionnaireQuestion.Question,
        //        ShowNote = questionnaireQuestion.ShowNote,
        //        NoteText = questionnaireQuestion.NoteText,
        //        CreatedDate = questionnaireQuestion.CreatedDate,
        //        ChangedDate = questionnaireQuestion.CreatedDate
        //    };

        //    this.DbContext.QuestionnaireQuestions.Add(questionnaireQuestionEntity);
        //    this.InitializeAfterCommit(questionnaireQuestion, questionnaireQuestionEntity);
        //}

        //public void DeleteById(int questionnaireId)
        //{
        //    throw new global::System.NotImplementedException();
        //}

        //public EditQuestionnaireQuestion GetQuestionnaireQuestionById(int questionId, int languageId)
        //{
        //    EditQuestionnaireQuestion ret = null;

        //    if (languageId == LanguageId.Swedish)
        //    {
        //        var questionnaireQuestion =
        //            this.DbContext.QuestionnaireQuestions.Where(q => q.Id == questionId)
        //                .Select(
        //                    q =>
        //                    new
        //                    {
        //                        Id = q.Id,
        //                        QuestionnaireId = q.Questionnaire_Id,
        //                        LanguageId = LanguageId.Swedish,
        //                        QuestionNumber = q.QuestionnaireQuestionNumber,
        //                        Question = q.QuestionnaireQuestion,
        //                        ShowNote = q.ShowNote,
        //                        NoteText = q.NoteText,
        //                        CreateDate = q.CreatedDate
        //                    })
        //                .FirstOrDefault();
        //        if (questionnaireQuestion != null)
        //          ret = new EditQuestionnaireQuestion(questionnaireQuestion.Id, questionnaireQuestion.QuestionnaireId, questionnaireQuestion.LanguageId, questionnaireQuestion.QuestionNumber,
        //                                              questionnaireQuestion.Question, questionnaireQuestion.ShowNote, questionnaireQuestion.NoteText, questionnaireQuestion.CreateDate);
        //    }
        //    else
        //    {
        //        var questionnaireQuestion =
        //            this.DbContext.QuestionnaireQuestionLanguage
        //                    .Where(l => l.QuestionnaireQuestion_Id == questionId && l.Language_Id == languageId)
        //                    .Select(
        //                        l =>
        //                        new
        //                        {
        //                            Id = l.QuestionnaireQuestion_Id,
        //                            QuestionnaireId = -1,  // it fill again in Controller
        //                            LanguageId = l.Language_Id,
        //                            QuestionNumber = l.QuestionnaireQuestions.QuestionnaireQuestionNumber,
        //                            Question = l.QuestionnaireQuestion,
        //                            ShowNote = l.QuestionnaireQuestions.ShowNote,
        //                            NoteText = l.NoteText,
        //                            CreateDate = l.CreatedDate
        //                        })
        //                    .FirstOrDefault();
        //        if (questionnaireQuestion != null)
        //            ret = new EditQuestionnaireQuestion(questionnaireQuestion.Id, questionnaireQuestion.QuestionnaireId, questionnaireQuestion.LanguageId, questionnaireQuestion.QuestionNumber, 
        //                                              questionnaireQuestion.Question, questionnaireQuestion.ShowNote, questionnaireQuestion.NoteText, questionnaireQuestion.CreateDate);
        //    }

        //    return ret;
        //}
       

        public List<QuestionnaireQuesOption> FindQuestionnaireQuestionOptions(int questionId, int languageId, int defualtLanguageId)
        {
            // All Questionnaire Question Options (Id,OptionPos,Option,OptionValue,LanguageId,ChangeedDate)
            var allSwedishQuestionOptions =
                this.DbContext.QuestionnaireQuestionOptions.Where(q => q.QuestionnaireQuestion_Id == questionId)
                    .Select(
                        q => new { Id = q.Id, OptionPos = q.QuestionnaireQuestionOptionPos, Option = q.QuestionnaireQuestionOption, 
                                   OptionValue = q.OptionValue, LanguageId = defualtLanguageId, ChangedDate = q.ChangedDate })
                    .ToList();

            // All Questionnaire Question Options Language
            var allOtherLanguageQuestionOptions =
               this.DbContext.QuestionnaireQuestionOptionLanguage
                   .Where(q => q.Language_Id == languageId)
                   .Select(
                       q => new { Id = q.QuestionnaireQuestionOption_Id, OptionPos = q.QuestionnaireQuestionOptions.QuestionnaireQuestionOptionPos ,
                                  Option = q.QuestionnaireQuestionOption, OptionValue = q.QuestionnaireQuestionOptions.OptionValue,
                                  LanguageId = q.Language_Id , ChangedDate = q.ChangedDate })
                   .ToList();

            // Only Needed Language Question Options 
            var pureLanguageQuestionOptions =
                     allOtherLanguageQuestionOptions
                     .Where(ql => allSwedishQuestionOptions.Select(qq => qq.Id)
                                                     .Contains(ql.Id))
                    .ToList();

            
            var tempList =
                allSwedishQuestionOptions.Where(
                    sq => pureLanguageQuestionOptions.Select(lq => lq.Id)
                                               .Contains(sq.Id));
            // Swedish QuestionOptions which there was not in QuestionOption Languags 
            var exceptList = allSwedishQuestionOptions.Except(tempList);

                        
            var fullQuestionOptions = pureLanguageQuestionOptions.Union(exceptList);

            return fullQuestionOptions.Select(q => new QuestionnaireQuesOption(q.Id, questionId, q.OptionPos, q.Option, q.OptionValue, q.LanguageId, q.ChangedDate)).ToList();            
            
        }

        //public void UpdateSwedishQuestionnaireQuestion(EditQuestionnaireQuestion questionnaireQuestion)
        //{
        //    var questionnaireQuestionEntity = this.DbContext.QuestionnaireQuestions.Find(questionnaireQuestion.Id);

        //    questionnaireQuestionEntity.Questionnaire_Id = questionnaireQuestion.QuestionnaireId;
        //    questionnaireQuestionEntity.QuestionnaireQuestionNumber = questionnaireQuestion.QuestionNumber;
        //    questionnaireQuestionEntity.QuestionnaireQuestion = questionnaireQuestion.Question;
        //    questionnaireQuestionEntity.ShowNote = questionnaireQuestion.ShowNote;
        //    questionnaireQuestionEntity.NoteText = questionnaireQuestion.NoteText;            
        //    questionnaireQuestionEntity.ChangedDate = questionnaireQuestion.ChangeDate;
        //}

        //public void UpdateOtherLanguageQuestionnaireQuestion(EditQuestionnaireQuestion questionnaireQuestion)
        //{
        //    var questionnaireQuestionLanguageEntity =
        //        this.DbContext.QuestionnaireQuestionLanguage.SingleOrDefault(
        //            l => l.QuestionnaireQuestion_Id == questionnaireQuestion.Id && l.Language_Id == questionnaireQuestion.LanguageId);

        //    if (questionnaireQuestionLanguageEntity != null)
        //    {
        //        questionnaireQuestionLanguageEntity.QuestionnaireQuestion = questionnaireQuestion.Question;
        //        questionnaireQuestionLanguageEntity.NoteText = questionnaireQuestion.NoteText;
        //        questionnaireQuestionLanguageEntity.ChangedDate = questionnaireQuestion.ChangeDate;
        //    }
        //    else
        //    {
        //        var newQuestionnaireQuestionLanguageEntity = new QuestionnaireQuesLangEntity
        //        {
        //            QuestionnaireQuestion_Id = 
        //                questionnaireQuestion.Id,
        //            Language_Id = 
        //                questionnaireQuestion.LanguageId,
        //            QuestionnaireQuestion = 
        //                questionnaireQuestion.Question,
        //            NoteText = 
        //                questionnaireQuestion.NoteText,                    
        //            CreatedDate =
        //                questionnaireQuestion.ChangeDate,
        //            ChangedDate = 
        //                questionnaireQuestion.ChangeDate
        //        };

        //        this.DbContext.QuestionnaireQuestionLanguage.Add(newQuestionnaireQuestionLanguageEntity);
        //    }
        //}

        #endregion
    }
}