﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DH.Helpdesk.BusinessData.Enums.MailTemplates;
using DH.Helpdesk.BusinessData.Models.Feedback;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.Cases;

namespace DH.Helpdesk.Services.Services.Feedback
{
    public class FeedbackTemplateService : IFeedbackTemplateService
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ICircularService _circularService;

        private const string MainTemplate = @"<BR>{question}<BR><table><tbody><tr>{optionTemplate}</tr></tbody></table>";
//        private const string OptionTemplate = @"<td ><a href='{baseurl}feedback/answer?guid={guid}&optionValue={value}&languageId={languageid}' style='padding: 0px 10px'><img src = '{baseurl}Content/img/{iconId}' style='width: 27px; height: 27px' alt={icontext}></a></td>";
        private const string OptionTemplate = @"<td ><a href='{baseurl}FeedbackAnswer/Answer?guid={guid}&optionId={id}&languageId={languageid}&customerId={customerId}' style='padding: 0px 10px'><img src = '{baseurl}Content/img/{iconId}' style='width: 27px; height: 27px' alt={icontext}></a></td>";

        private class Templates
        {
            public const string Question = "{question}";
            public const string Option = "{optionTemplate}";
            public const string IconId = "{iconId}";
            public const string BaseUrl = "{baseurl}";
            public const string Guid = "{guid}";
            public const string OptionId = "{id}";
            public const string LanguageId = "{languageid}";
            public const string IconText = "{icontext}";
            public const string CustomerId = "{customerId}";
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

        public IEnumerable<string> FindIdentifiers(string body)
        {
            var regex = new Regex(@"\[#F(?<Identifier>(?:.)*)\]");
            var result = regex.Matches(body);
            return from Match match in result where match.Groups["Identifier"] != null select match.Groups["Identifier"].Value;
        }

        public List<FeedbackField> GetCustomerTemplates(IEnumerable<string> identifiers, int customerId, int languageId, int caseId, string absoluterUrl)
        {
            var res = new List<FeedbackField>();

            var feedbacks = _feedbackService.GetFeedbackFullItems(customerId, languageId, identifiers);

            if (feedbacks.Any())
            {
                var fields = feedbacks.Select(f => GetField(f, languageId, caseId, absoluterUrl, customerId)).ToArray();
                res.AddRange(fields);
            }

            return res;
        }

        public void UpdateFeedbackStatus(FeedbackField field)
        {
            if (field.CircularId > 0)
                _circularService.SetStatus(field.CircularId, CircularStates.Sent);

            if (field.CircularPartGuid != Guid.Empty)
                _circularService.UpdateParticipantSendDate(field.CircularPartGuid, DateTime.Now);
        }

        private FeedbackField GetField(FeedbackFullItem feedback, int languageId, int caseId, string absoluterUrl, int customerId)
        {
            var field = new FeedbackField
            {
                FieldType = FieldTypes.String,
                Key = $"[{FeedbackTemplate.FeedbackIdentifierPredicate}{feedback.Info.Identifier}]",
                FeedbackId = feedback.Info.Id,
                StringValue = string.Empty
            };

            var dbCircular = _circularService.GetSingleOrDefaultByQuestionnaireId(feedback.Info.Id);
            if (dbCircular == null) throw new NullReferenceException("Missing Circular for Feedback. Feedback should contain Circular");
            field.CircularId = dbCircular.Id;

            var isSuccess = RollDices(dbCircular.CaseFilter.SelectedProcent);
            if (!isSuccess) return field;

            var casesIds = _circularService.GetAllCircularCasesIds(dbCircular.Id);
            casesIds.Add(caseId);
            var circular = new CircularForUpdate(dbCircular.Id, dbCircular.CircularName, DateTime.Now, casesIds, dbCircular.CaseFilter);
            _circularService.UpdateCircular(circular);//also in this method Participant is created.

            var participant = _circularService.GetNotAnsweredParticipants(dbCircular.Id).SingleOrDefault(p => p.CaseId == caseId);//should be only 1 part. for feedback

            if (participant != null)
            {
                field.StringValue = CreateTemplate(feedback, languageId, absoluterUrl, participant.Guid, customerId);
                field.CircularPartGuid = participant.Guid;
            }

            return field;
        }

        private bool RollDices(int chancePercents)
        {
            var rand = new Random();
            return rand.Next(1, 101) <= chancePercents;
        }

        private string CreateTemplate(FeedbackFullItem feedback, int languageId, string absoluterUrl, Guid guid, int customerId)
        {
            var template = new StringBuilder(MainTemplate);
            template.Replace(Templates.Question, feedback.Question.Question);

            var optionsTemplate = new StringBuilder();
            foreach (var option in feedback.Options.OrderBy(o => o.Position))
            {
                var optionTemplate = OptionTemplate
                    .Replace(Templates.IconId, option.IconId)
                    .Replace(Templates.BaseUrl, absoluterUrl)
                    .Replace(Templates.OptionId, option.Id.ToString())
                    .Replace(Templates.LanguageId, languageId.ToString())
                    .Replace(Templates.IconText, option.Text)
                    .Replace(Templates.Guid, guid.ToString())
                    .Replace(Templates.CustomerId, customerId.ToString());
                optionsTemplate.Append(optionTemplate);
            }

            template.Replace(Templates.Option, optionsTemplate.ToString());

            return template.ToString();
        }
    }

    public interface IFeedbackTemplateService
    {
        bool ContainsIdentifiers(string body);
        List<FeedbackField> GetCustomerTemplates(IEnumerable<string> identifiers, int customerId, int languageId, int caseId, string absoluterUrl);
        IEnumerable<string> FindIdentifiers(string body);
        void UpdateFeedbackStatus(FeedbackField field);
    }
}
