using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Feedback;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Dal.Repositories.Questionnaire;
using DH.Helpdesk.Domain.Questionnaire;

namespace DH.Helpdesk.Services.Services
{
	public class FeedbackService : IFeedbackService
	{
		private readonly IQuestionnaireRepository _questionnaireRepository;
		private readonly IQuestionnaireQuestionRepository _questionnaireQuestionRepository;

		public FeedbackService(IQuestionnaireRepository questionnaireRepository, IQuestionnaireQuestionRepository questionnaireQuestionRepository)
		{
			_questionnaireRepository = questionnaireRepository;
			_questionnaireQuestionRepository = questionnaireQuestionRepository;
		}

		public List<FeedbackOverview> FindFeedbackOverviews(int customerId)
		{
			var entities = _questionnaireRepository.FindFeedbackOverviews(customerId);
			return entities.Select(q => new FeedbackOverview
			{
				Id = q.Id,
				Identifier = q.Identifier,
				Name = q.QuestionnaireName,
				Description = q.QuestionnaireDescription,
                ExcludeAdministrators = q.ExcludeAdministrators,
                UseBase64Images = q.UseBase64Images,
            }).ToList();
		}

		public List<FeedbackFullItem> GetFeedbackFullItems(int customerId, int languageId, IEnumerable<string> identifiers)
		{
			var entities = _questionnaireRepository.GetFeedbackFullItems(customerId, identifiers);

			if(!entities.Any())
				return new List<FeedbackFullItem>();

			return entities.Select(f => ToFeedbackFullItem(f, languageId)).ToList();
		}

		public int AddFeedback(NewQuestionnaire newFeedback)
		{
			newFeedback.Type = QuestionnaireType.Feedback;
			_questionnaireRepository.AddSwedishQuestionnaire(newFeedback);
			_questionnaireRepository.Commit();

			return newFeedback.Id;
		}

		public EditQuestionnaire GetFeedback(int feedbackId, int languageId)
		{
			return _questionnaireRepository.GetQuestionnaireById(feedbackId, languageId);
		}

		public void UpdateFeedback(EditQuestionnaire editedQuestionnaire)
		{
			switch (editedQuestionnaire.LanguageId)
			{
				case LanguageIds.Swedish:
					_questionnaireRepository.UpdateSwedishQuestionnaire(editedQuestionnaire);
					break;

				default:
					_questionnaireRepository.UpdateOtherLanguageQuestionnaire(editedQuestionnaire);
					break;
			}

			_questionnaireRepository.Commit();
		}

		public void DeleteFeedbackById(int feedbackId)
		{
			var questions = _questionnaireQuestionRepository.FindQuestionnaireQuestions(feedbackId, LanguageIds.Swedish,
				LanguageIds.Swedish);

			foreach (var question in questions)
				_questionnaireQuestionRepository.DeleteQuestionById(question.Id);

			_questionnaireQuestionRepository.Commit();

			_questionnaireRepository.DeleteQuestionnaireById(feedbackId);
			_questionnaireRepository.Commit();
		}

		private FeedbackFullItem ToFeedbackFullItem(QuestionnaireEntity entity, int languageId)
		{
			var item = new FeedbackFullItem();
			item.Info = new FeedbackOverview();
			item.Info.Identifier = entity.Identifier;
			item.Info.ExcludeAdministrators = entity.ExcludeAdministrators;
			item.Info.UseBase64Images = entity.UseBase64Images;
            item.Info.Id = entity.Id;

			var feedbackLangEntity = entity.QuestionnaireLanguageEntities != null && entity.QuestionnaireLanguageEntities.Any() ?
				entity.QuestionnaireLanguageEntities.SingleOrDefault(le => le.Language_Id == languageId) :
				null;
			item.Info.Name = feedbackLangEntity == null ? entity.QuestionnaireName : feedbackLangEntity.QuestionnaireName;
			item.Info.Description = feedbackLangEntity == null ? entity.QuestionnaireDescription : feedbackLangEntity.QuestionnaireDescription;

			item.Question = new FeedbackQuestionOverview();
			var questionEntity = entity.QuestionnaireQuestionEntities.SingleOrDefault();
			if (questionEntity == null) throw new Exception("Only one question in feedback is allowed.");
			item.Question.Id = questionEntity.Id;
			item.Question.IsShowNote = questionEntity.ShowNote.ToBool();

			var questionLangEntity = questionEntity.QuestionnaireQuesLangEntities != null && questionEntity.QuestionnaireQuesLangEntities.Any() ?
				questionEntity.QuestionnaireQuesLangEntities.SingleOrDefault(qqe => qqe.Language_Id == languageId) :
				null;
			item.Question.NoteText = questionLangEntity == null ?
				questionEntity.NoteText :
				questionLangEntity.NoteText;
			item.Question.Question = questionLangEntity == null ?
				questionEntity.QuestionnaireQuestion :
				questionLangEntity.QuestionnaireQuestion;

			item.Options = new List<FeedbackQuestionOption>();
			var optionsEntities = questionEntity.QuestionnaireQuestionOptionEntities.ToList();
			foreach (var optionEntity in optionsEntities)
			{
				var option = new FeedbackQuestionOption();
				option.Id = optionEntity.Id;
				option.IconId = optionEntity.IconId;
				option.Position = optionEntity.QuestionnaireQuestionOptionPos;
				option.Value = optionEntity.OptionValue;
			    option.IconSrc = optionEntity.IconSrc != null
			        ? FeedBack.ImgId + Convert.ToBase64String(optionEntity.IconSrc)
			        : string.Empty;

				var optionLangEntity = optionEntity.QuestionnaireQuesOpLangEntities != null && optionEntity.QuestionnaireQuesOpLangEntities.Any() ?
					optionEntity.QuestionnaireQuesOpLangEntities.SingleOrDefault(qqo => qqo.Language_Id == languageId) :
					null;
				option.Text = optionLangEntity == null ?
					optionEntity.QuestionnaireQuestionOption :
					optionLangEntity.QuestionnaireQuestionOption;
				item.Options.Add(option);
			}


			return item;
		}
	}

	public interface IFeedbackService
	{
		List<FeedbackOverview> FindFeedbackOverviews(int customerId);
		List<FeedbackFullItem> GetFeedbackFullItems(int customerId, int languageId, IEnumerable<string> identifiers);
		int AddFeedback(NewQuestionnaire newFeedback);
		EditQuestionnaire GetFeedback(int feedbackId, int languageId);
		void UpdateFeedback(EditQuestionnaire editedQuestionnaire);
		void DeleteFeedbackById(int feedbackId);
	}
}
