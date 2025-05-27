using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using DH.Helpdesk.BusinessData.Enums.MailTemplates;
using DH.Helpdesk.BusinessData.Models.Feedback;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.Cases;

namespace DH.Helpdesk.Services.Services.Feedback
{
    public class FeedbackTemplateService : IFeedbackTemplateService
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ICircularService _circularService;

        private const string MainTemplate = @"<BR>{question}<BR><table><tbody><tr>{optionTemplate}</tr></tbody></table>";
        private const string OptionTemplate = @"<td ><a href='{baseurl}FeedbackAnswer/Answer?guid={guid}&optionId={id}&languageId={languageid}&customerId={customerId}' style='padding: 0px 10px'><img src='{baseurl}Content/img/{iconId}' style='width: 27px; height: 27px' alt={icontext} title={icontext}></a></td>";
        private const string OptionTemplateLoaded = @"<td ><a href='{baseurl}FeedbackAnswer/Answer?guid={guid}&optionId={id}&languageId={languageid}&customerId={customerId}' style='padding: 0px 10px'><img src='{baseurl}FeedbackAnswer/GetFeedbackImg/{id}' style='width: 27px; height: 27px' alt={icontext} title={icontext}></a></td>";
        private const string OptionTemplateBase64 = @"<td ><a href='{baseurl}FeedbackAnswer/Answer?guid={guid}&optionId={id}&languageId={languageid}&customerId={customerId}' style='padding: 0px 10px'><img src='{base64Img}' style='width: 27px; height: 27px' alt={icontext} title={icontext}></a></td>";

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
            public const string Base64Img = "{base64Img}";
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
            var regex = new Regex(@"\[#F(?<Identifier>(?:.)*?)\]");
            var result = regex.Matches(body);
            return from Match match in result where match.Groups["Identifier"] != null select match.Groups["Identifier"].Value;
        }

        public List<FeedbackField> GetCustomerTemplates(IEnumerable<string> identifiers, int customerId, int languageId, int caseId, int caseTypeId, string absoluterUrl)
        {
            var res = new List<FeedbackField>();

            var feedbacks = _feedbackService.GetFeedbackFullItems(customerId, languageId, identifiers);

            if (feedbacks.Any())
            {
                var fields = feedbacks.Select(f => GetField(f, languageId, caseId, absoluterUrl, customerId, caseTypeId)).ToArray();
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

        private FeedbackField GetField(FeedbackFullItem feedback, int languageId, int caseId, string absoluterUrl, int customerId, int caseTypeId)
        {
            var field = new FeedbackField
            {
                FieldType = FieldTypes.String,
                Key = $"[{FeedbackTemplate.FeedbackIdentifierPredicate}{feedback.Info.Identifier}]",
                FeedbackId = feedback.Info.Id,
                StringValue = string.Empty,
                ExcludeAdministrators = feedback.Info.ExcludeAdministrators,
                UseBase64Images = feedback.Info.UseBase64Images
            };

            var dbCircular = _circularService.GetSingleOrDefaultByQuestionnaireId(feedback.Info.Id);
            if (dbCircular == null) throw new NullReferenceException("Missing Circular for Feedback. Feedback should contain Circular");
            field.CircularId = dbCircular.Id;

            var isSuccess = RollDices(dbCircular.CaseFilter.SelectedProcent);
            var hasCaseType = CheckCaseType(caseTypeId, dbCircular);
            if (!isSuccess || !hasCaseType) return field;

            var circular = new CircularForUpdate(dbCircular.Id, dbCircular.CircularName, DateTime.Now, dbCircular.CaseFilter);
            _circularService.UpdateCircular(circular); //also in this method Participant is created.
            if(!_circularService.HasCase(dbCircular.Id, caseId)) // should be only 1 part. for feedback
			    _circularService.AddCaseToCircular(dbCircular.Id, caseId);

			var participant = _circularService.GetNotAnsweredParticipant(dbCircular.Id, caseId);// should be only 1 part. for feedback

            if (participant != null)
            {
                field.StringValue = CreateTemplate(feedback, languageId, absoluterUrl, participant.Guid, customerId);
                field.CircularPartGuid = participant.Guid;
            }

            return field;
        }

        private static bool CheckCaseType(int caseTypeId, CircularForEdit dbCircular)
        {
            return (dbCircular.CaseFilter.SelectedCaseTypes == null || !dbCircular.CaseFilter.SelectedCaseTypes.Any()) ||
                   (dbCircular.CaseFilter.SelectedCaseTypes != null &&
                    dbCircular.CaseFilter.SelectedCaseTypes.Any() &&
                    dbCircular.CaseFilter.SelectedCaseTypes.Contains(caseTypeId));
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
            if (!feedback.Info.UseBase64Images)
            {
                //"Som vanligt"
                foreach (var option in feedback.Options.OrderBy(o => o.Position))
                {
                    if (!string.IsNullOrEmpty(option.IconSrc))
                    {
                        var optionTemplate = OptionTemplateLoaded
                            .Replace(Templates.BaseUrl, absoluterUrl)
                            .Replace(Templates.OptionId, option.Id.ToString())
                            .Replace(Templates.LanguageId, languageId.ToString())
                            .Replace(Templates.IconText, option.Text)
                            .Replace(Templates.Guid, guid.ToString())
                            .Replace(Templates.CustomerId, customerId.ToString());
                        optionsTemplate.Append(optionTemplate);
                    }
                    else
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
                }
            }
            else
            {

                foreach (var option in feedback.Options.OrderBy(o => o.Position))
                {
                    //Om det finns IconSource = uppladdad bild är den redan konverterad till base&4 när den kommer till denna metoden
                    if(!String.IsNullOrEmpty(option.IconSrc))
                    {
                        var optionTemplate = OptionTemplateBase64
                            .Replace(Templates.Base64Img, option.IconSrc)
                            .Replace(Templates.BaseUrl, absoluterUrl)
                            .Replace(Templates.OptionId, option.Id.ToString())
                            .Replace(Templates.LanguageId, languageId.ToString())
                            .Replace(Templates.IconText, option.Text)
                            .Replace(Templates.Guid, guid.ToString())
                            .Replace(Templates.CustomerId, customerId.ToString());
                        optionsTemplate.Append(optionTemplate);
                    }
                    else //konvertera existerande systembild till base64
                    {
                        var source = $"{absoluterUrl}Content/img/{option.IconId}";
                        var bytes = new System.Net.WebClient().DownloadData(source);
                        var mimeType = GetMimeFromBytes(bytes);
                        var base64 = Convert.ToBase64String(bytes);
                        var imgSrc = $"data:{mimeType};base64,{base64}";

                        var optionTemplate = OptionTemplateBase64
                            .Replace(Templates.Base64Img, imgSrc)
                            .Replace(Templates.BaseUrl, absoluterUrl)
                            .Replace(Templates.OptionId, option.Id.ToString())
                            .Replace(Templates.LanguageId, languageId.ToString())
                            .Replace(Templates.IconText, option.Text)
                            .Replace(Templates.Guid, guid.ToString())
                            .Replace(Templates.CustomerId, customerId.ToString());
                        optionsTemplate.Append(optionTemplate);
                    }
                        
                }
            }
            template.Replace(Templates.Option, optionsTemplate.ToString());

            return template.ToString();
        }
        private static string GetMimeFromBytes(byte[] data)
        {
            if (data.Length >= 4 &&
                data[0] == 0x89 && data[1] == 0x50 &&
                data[2] == 0x4E && data[3] == 0x47)
                return "image/png";

            if (data.Length >= 3 &&
                data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF)
                return "image/jpeg";

            if (data.Length >= 4 &&
                data[0] == 0x47 && data[1] == 0x49 &&
                data[2] == 0x46 && data[3] == 0x38)
                return "image/gif";

            return "application/octet-stream";
        }
    }

    public interface IFeedbackTemplateService
    {
        bool ContainsIdentifiers(string body);
        List<FeedbackField> GetCustomerTemplates(IEnumerable<string> identifiers, int customerId, int languageId, int caseId, int caseTypeId, string absoluterUrl);
        IEnumerable<string> FindIdentifiers(string body);
        void UpdateFeedbackStatus(FeedbackField field);
    }
}
