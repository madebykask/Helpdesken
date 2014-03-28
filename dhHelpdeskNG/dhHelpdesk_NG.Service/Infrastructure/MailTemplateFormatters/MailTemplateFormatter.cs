namespace DH.Helpdesk.Services.Infrastructure.MailTemplateFormatters
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;

    public abstract class MailTemplateFormatter
    {
        protected abstract Dictionary<string, string> CreateMarkValues(
            MailTemplate template,
            Change model,
            int customerId,
            int languageId);

        public Mail Format(MailTemplate template, Change model, int customerId, int languageId)
        {
            var markValues = this.CreateMarkValues(template, model, customerId, languageId);

            var subjectMarks = Regex.Matches(template.Subject, "[#]");
            var bodyMarks = Regex.Matches(template.Body, "[#]");

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
