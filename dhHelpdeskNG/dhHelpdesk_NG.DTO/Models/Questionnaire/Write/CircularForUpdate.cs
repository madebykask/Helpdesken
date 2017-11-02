using System.Collections.Generic;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Write
{
    using System;

    public sealed class CircularForUpdate : Circular
    {
        public CircularForUpdate(int id, string circularName, DateTime changedDate, List<int> relatedCaseIds, CircularCaseFilter caseFilter, List<string> extraEmails = null)
            : base(circularName)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
            this.RelatedCaseIds = relatedCaseIds;
            CaseFilter = caseFilter;
            ExtraEmails = extraEmails ?? new List<string>();
        }

        public DateTime ChangedDate { get; private set; }

        [NotNull]
        public List<int> RelatedCaseIds { get; private set; }

        public CircularCaseFilter CaseFilter { get; set; }

        public List<string> ExtraEmails { get; set; }

        public int? MailTemplateId { get; set; }
    }
}