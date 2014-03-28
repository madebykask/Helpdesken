namespace DH.Helpdesk.Services.Infrastructure.MailTemplateFormatters
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using DH.Helpdesk.BusinessData.Models.MailTemplates;

    public abstract class MailTemplateFormatter<TBusinessModel>
    {
        private const string MarkPattern = @"\[#[0-9]*\]";

        protected abstract Dictionary<string, string> CreateMarkValues(
            MailTemplate template,
            TBusinessModel model,
            int customerId,
            int languageId);

        public Mail Format(MailTemplate template, TBusinessModel model, int customerId, int languageId)
        {
            var markValues = this.CreateMarkValues(template, model, customerId, languageId);

            var subjectMarks = Regex.Matches(template.Subject, MarkPattern);
            var bodyMarks = Regex.Matches(template.Body, MarkPattern);

            var subject = template.Subject;
            var body = template.Body;

            foreach (Match mark in subjectMarks)
            {
                var value = markValues[mark.Value];
                subject = subject.Replace(mark.Value, value);
            }

            foreach (Match mark in bodyMarks)
            {
                var value = markValues[mark.Value];
                body = body.Replace(mark.Value, value);
            }

            return new Mail(subject, body);
        }
    }
}