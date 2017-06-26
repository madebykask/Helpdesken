using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Domain.Questionnaire;

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

        public void AddQuestionOption(QuestionnaireQuesOption newOption)
        {
            var questionnaireQuestionOptionEntity = new QuestionnaireQuestionOptionEntity()
            {
                QuestionnaireQuestion_Id = newOption.QuestionId,
                QuestionnaireQuestionOptionPos = newOption.OptionPos,
                QuestionnaireQuestionOption = newOption.Option,
                OptionValue = newOption.OptionValue,
                IconId = newOption.IconId,
                CreatedDate = newOption.ChangedDate,
                ChangedDate = newOption.ChangedDate,
                IconSrc = newOption.IconSrc
            };

            this.DbContext.QuestionnaireQuestionOptions.Add(questionnaireQuestionOptionEntity);
            this.InitializeAfterCommit(newOption, questionnaireQuestionOptionEntity);
        }

        public void UpdateQuestionOption(QuestionnaireQuesOption option)
        {
            var questionnaireQuestionOptionEntity = this.DbContext.QuestionnaireQuestionOptions.Find(option.Id);

            questionnaireQuestionOptionEntity.QuestionnaireQuestion_Id = option.QuestionId;
            questionnaireQuestionOptionEntity.QuestionnaireQuestionOptionPos = option.OptionPos;
            questionnaireQuestionOptionEntity.QuestionnaireQuestionOption = option.Option;
            questionnaireQuestionOptionEntity.OptionValue = option.OptionValue;
            questionnaireQuestionOptionEntity.IconId = option.IconId;
            questionnaireQuestionOptionEntity.ChangedDate = option.ChangedDate;
            questionnaireQuestionOptionEntity.IconSrc = option.IconSrc;
        }

        public void UpdateQuestionOption(QuestionnaireQuesOption option, int languageId)
        {
            var questionnaireQuestionOptionLangEntity = this.DbContext.QuestionnaireQuestionOptionLanguage.Find(option.Id, languageId);
            if (questionnaireQuestionOptionLangEntity == null)
            {
                AddLanguageQuestionnaireQuestionOption(option);
            }
            else
            {
                questionnaireQuestionOptionLangEntity.QuestionnaireQuestionOption = option.Option;
                questionnaireQuestionOptionLangEntity.ChangedDate = option.ChangedDate;
            }
        }

        private void AddLanguageQuestionnaireQuestionOption(QuestionnaireQuesOption newOption)
        {
            var questionnaireQuesOpLangEntity = new QuestionnaireQuesOpLangEntity()
            {
                QuestionnaireQuestionOption_Id = newOption.Id,
                Language_Id = newOption.LanguageId,
                QuestionnaireQuestionOption = newOption.Option,
                CreatedDate = newOption.ChangedDate,
                ChangedDate = newOption.ChangedDate
            };

            this.DbContext.QuestionnaireQuestionOptionLanguage.Add(questionnaireQuesOpLangEntity);
        }


        public void DeleteQuestionOptionById(int optionId)
        {
            var questionOptionLanguages = this.DbContext.QuestionnaireQuestionOptionLanguage.Where(ol => ol.QuestionnaireQuestionOption_Id == optionId);

            foreach (var optionLanguage in questionOptionLanguages)
                this.DbContext.QuestionnaireQuestionOptionLanguage.Remove(optionLanguage);

            var questionOption = this.DbContext.QuestionnaireQuestionOptions.Find(optionId);
            this.DbContext.QuestionnaireQuestionOptions.Remove(questionOption);
        }

        public void DeleteQuestionOptionById(int optionId, int languageId)
        {
            var questionOption =
                this.DbContext.QuestionnaireQuestionOptionLanguage
                     .Find(optionId, languageId);
            this.DbContext.QuestionnaireQuestionOptionLanguage.Remove(questionOption);
        }

        public void UpdateQuestionnaireQuestionOptionIcon(int optionId, byte[] iconSrc)
        {
            var option = DbContext.QuestionnaireQuestionOptions.SingleOrDefault(x => x.Id == optionId);
            if (option != null)
            {
                option.IconSrc = iconSrc;
                DbContext.SaveChanges();
            }
        }

        public List<QuestionnaireQuesOption> FindQuestionnaireQuestionOptions(int questionId, int languageId, int defualtLanguageId)
        {
            // All Questionnaire Question Options (Id,OptionPos,Option,OptionValue,LanguageId,ChangeedDate)
            var allSwedishQuestionOptions =
                this.DbContext.QuestionnaireQuestionOptions.Where(q => q.QuestionnaireQuestion_Id == questionId)
                    .Select(
                        q => new
                        {
                            Id = q.Id,
                            OptionPos = q.QuestionnaireQuestionOptionPos,
                            Option = q.QuestionnaireQuestionOption,
                            OptionValue = q.OptionValue,
                            LanguageId = defualtLanguageId,
                            ChangedDate = q.ChangedDate,
                            IconId = q.IconId,
                            IconSrc = q.IconSrc
                        })
                    .ToList();

            // All Questionnaire Question Options Language
            var allOtherLanguageQuestionOptions =
               this.DbContext.QuestionnaireQuestionOptionLanguage
                   .Where(q => q.Language_Id == languageId)
                   .Select(
                       q => new
                       {
                           Id = q.QuestionnaireQuestionOption_Id,
                           OptionPos = q.QuestionnaireQuestionOptions.QuestionnaireQuestionOptionPos,
                           Option = q.QuestionnaireQuestionOption,
                           OptionValue = q.QuestionnaireQuestionOptions.OptionValue,
                           LanguageId = q.Language_Id,
                           ChangedDate = q.ChangedDate,
                           IconId = "",
                           IconSrc = (byte[])null
                       })
                   .ToList();

            // Only Needed Language Question Options 
            var pureLanguageQuestionOptions =
                     allOtherLanguageQuestionOptions
                     .Where(ql => allSwedishQuestionOptions.Select(qq => qq.Id).Contains(ql.Id))
                    .ToList();


            var tempList = allSwedishQuestionOptions.Where(sq => pureLanguageQuestionOptions.Select(lq => lq.Id).Contains(sq.Id));
            // Swedish QuestionOptions which there was not in QuestionOption Languags 
            var exceptList = allSwedishQuestionOptions.Except(tempList);


            var fullQuestionOptions = pureLanguageQuestionOptions.Union(exceptList);

            return fullQuestionOptions.Select(q => new QuestionnaireQuesOption
            {
                Id = q.Id,
                IconId = allSwedishQuestionOptions.Single(s => s.Id == q.Id).IconId,
                Option = q.Option,
                QuestionId = questionId,
                LanguageId = q.LanguageId,
                ChangedDate = q.ChangedDate,
                OptionPos = q.OptionPos,
                OptionValue = q.OptionValue,
                IconSrc = allSwedishQuestionOptions.Single(s => s.Id == q.Id).IconSrc
            }).ToList();
        }


        #endregion
    }
}