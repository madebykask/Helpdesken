namespace DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Services.Exceptions;

    using LinqLib.Sequence;

    public abstract class MailTemplateFormatter<TBusinessModel>
    {
        #region Constants

        private const string MarkPattern = @"\[#[0-9]*\]";

        #endregion

        #region Public Methods and Operators

        public Mail Format(MailTemplate template, TBusinessModel businessModel, int customerId, int languageId)
        {
            var markValues = this.GetMarkValues(template, businessModel, customerId, languageId);

            var subjectMarks = Regex.Matches(template.Subject, MarkPattern);
            var bodyMarks = Regex.Matches(template.Body, MarkPattern);

//            CheckMarks(markValues, subjectMarks, bodyMarks);

            var subject = template.Subject;
            var body = template.Body;

            foreach (Match mark in subjectMarks)
            {
                if (!markValues.ContainsKey(mark.Value))
                {
                    continue;
                }

                var value = markValues[mark.Value];
                subject = subject.Replace(mark.Value, value);
            }

            foreach (Match mark in bodyMarks)
            {
                if (!markValues.ContainsKey(mark.Value))
                {
                    continue;
                }

                var value = markValues[mark.Value];
                body = body.Replace(mark.Value, value);
            }

            return new Mail(subject, body);
        }

        #endregion

        #region Methods

        protected abstract Dictionary<string, string> GetMarkValues(
            MailTemplate template,
            TBusinessModel businessModel,
            int customerId,
            int languageId);

        private static void CheckMarks(
            Dictionary<string, string> markValues,
            MatchCollection subjectMarks,
            MatchCollection bodyMarks)
        {
            var notFoundSubjectMarks =
                subjectMarks.Cast<Match>().Where(m => !markValues.ContainsKey(m.Value)).Select(m => m.Value).ToList();

            var notFoundBodyMarks =
                bodyMarks.Cast<Match>().Where(m => !markValues.ContainsKey(m.Value)).Select(m => m.Value).ToList();

            if (notFoundSubjectMarks.Any() || notFoundBodyMarks.Any())
            {
                var notFoundMarks = new List<string>();

                notFoundMarks.AddRange(notFoundSubjectMarks);
                notFoundMarks.AddRange(notFoundBodyMarks);

                notFoundMarks = notFoundMarks.Distinct(m => m).ToList();
                throw new MarksNotFoundException("Specified marks was not found in mail template.", notFoundMarks);
            }
        }

        #endregion
    }
}