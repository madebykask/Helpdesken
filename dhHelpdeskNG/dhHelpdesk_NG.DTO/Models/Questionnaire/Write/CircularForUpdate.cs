using System.Collections.Generic;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Write
{
    using System;

    public sealed class CircularForUpdate : Circular
    {
        public CircularForUpdate(int id, string circularName, DateTime changedDate, List<int> relatedCaseIds)
            : base(circularName)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
            this.RelatedCaseIds = relatedCaseIds;
        }

        public DateTime ChangedDate { get; private set; }

        [NotNull]
        public List<int> RelatedCaseIds { get; private set; }
    }
}