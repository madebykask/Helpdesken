using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using DH.Helpdesk.BusinessData.Enums.MailTemplates;
using DH.Helpdesk.BusinessData.Models.Feedback;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services.Feedback
{
	public class FeedbackTemplateService : IFeedbackTemplateService
	{
		private readonly IFeedbackService _feedbackService;
		private readonly ICircularService _circularService;

		private const string MainTemplate = @"<BR>{question}<BR><table><tbody><tr>{optionTemplate}</tr></tbody></table>";
		private const string OptionTemplate = @"<td ><a href='{baseurl}feedback/answer?guid={guid}&optionValue={value}&languageId={languageid}' style='padding: 0px 10px'><img src = '{baseurl}Content/img/{iconId}' style='width: 27px; height: 27px' alt={icontext}></a></td>";

		private class Templates
		{
			public const string Question = "{question}";
			public const string Option = "{optionTemplate}";
			public const string IconId = "{iconId}";
			public const string BaseUrl = "{baseurl}";
			public const string Guid = "{guid}";
			public const string OptionValue = "{value}";
			public const string LanguageId = "{languageid}";
			public const string IconText = "{icontext}";
		}

		public FeedbackTemplateService(IFeedbackService feedbackService, ICircularService circularService)
		{
			_feedbackService = feedbackService;
			_circularService = circularService;
		}

		public bool ContainsIdentifiers(string body)
		{
			if (string.IsNullOrEmpty(body))
				return false;

			return body.ToLower().Contains(FeedbackTemplate.FeedbackIdentifierPredicate.ToLower());
		}

		public string ApplyTemplate(string body, int customerId)
		{
			if (string.IsNullOrEmpty(body))
				return body;

			//var feedbacks = _feedbackService.FindFeedbackOverviews(customerId);

			//var regex = new Regex(@"\[#F(?<Identifier>(?:.)*)\]");
			//var result = regex.Matches(body);
			//foreach (Match match in result)
			//{
			//	var idenifier = match.Groups["Identifier"].Value;
			//	if (string.IsNullOrEmpty(idenifier)) continue;

			//	var feedback = feedbacks.SingleOrDefault(f => f.Identifier.Equals(idenifier, StringComparison.InvariantCultureIgnoreCase));
			//	if (feedback == null) continue;

			//	var template = CreateTemplate(feedback);

			//	Regex.Replace(body, match.Value, template);
			//}

			return body;
		}

		public IEnumerable<string> FindIdentifiers(string body)
		{
			var regex = new Regex(@"\[#F(?<Identifier>(?:.)*)\]");
			var result = regex.Matches(body);
			return from Match match in result where match.Groups["Identifier"] != null select match.Groups["Identifier"].Value;
		}

		public List<Field> GetCustomerTemplates(IEnumerable<string> identifiers, int customerId, int languageId, int caseId, string absoluterUrl)
		{
			var res = new List<Field>();

			var feedbacks = _feedbackService.GetFeedbackFullItems(customerId, languageId, identifiers);

			if (feedbacks.Any())
			{
				var fields = feedbacks.Select(f => GetField(f, languageId, caseId, absoluterUrl)).ToArray();
				res.AddRange(fields);
			}

			return res;
		}

		private Field GetField(FeedbackFullItem feedback, int languageId, int caseId,  string absoluterUrl)
		{
			var field = new Field
			{
				Id = int.MaxValue,//does not matter
				FieldType = FieldTypes.String,
				Key = $"[{FeedbackTemplate.FeedbackIdentifierPredicate}{feedback.Info.Identifier}]",
				StringValue = string.Empty
			};

			var dbCircular = _circularService.GetSingleOrDefaultByQuestionnaireId(feedback.Info.Id);
			if (dbCircular == null) return field; //if no circular, thinking that SelectedPercent = 0;

			var isSuccess = RollDices(dbCircular.CaseFilter.SelectedProcent);
			if (!isSuccess) return field;

			var casesIds = _circularService.GetAllCircularCasesIds(dbCircular.Id);
			casesIds.Add(caseId);
			var circular = new CircularForUpdate(dbCircular.Id, dbCircular.CircularName, DateTime.Now, casesIds, dbCircular.CaseFilter);
			_circularService.UpdateCircular(circular);

			var participant = _circularService.GetNotAnsweredParticipants(dbCircular.Id).SingleOrDefault();//should be only 1 part for feedback

			if(participant != null)
				field.StringValue = CreateTemplate(feedback, languageId, absoluterUrl, participant.Guid);

			return field;
		}

		private bool RollDices(int chancePercents)
		{
			var rand = new Random();
			return rand.Next(1, 101) <= chancePercents;
		}

		private string CreateTemplate(FeedbackFullItem feedback, int languageId, string absoluterUrl, Guid guid)
		{
			var template = new StringBuilder(MainTemplate);
			template.Replace(Templates.Question, feedback.Question.Question);

			var optionsTemplate = new StringBuilder();
			foreach (var option in feedback.Options.OrderBy(o => o.Position))
			{
				var optionTemplate = OptionTemplate
					.Replace(Templates.IconId, option.IconId)
					.Replace(Templates.BaseUrl, absoluterUrl)
					.Replace(Templates.OptionValue, option.Value.ToString())
					.Replace(Templates.LanguageId, languageId.ToString())
					.Replace(Templates.IconText, option.Text)
					.Replace(Templates.Guid, guid.ToString());
				optionsTemplate.Append(optionTemplate);
			}

			template.Replace(Templates.Option, optionsTemplate.ToString());

			return template.ToString();
		}
	}


	public interface IFeedbackTemplateService
	{
		bool ContainsIdentifiers(string body);
		string ApplyTemplate(string body, int customerId);
		List<Field> GetCustomerTemplates(IEnumerable<string> identifiers, int customerId, int languageId, int caseId, string absoluterUrl);
		IEnumerable<string> FindIdentifiers(string body);
	}
}
