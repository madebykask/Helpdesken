namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Write
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CircularForInsert : Circular
    {
        public CircularForInsert(
            string circularName,
            int questionnaireId,
            int status,
            DateTime createdDate,
            List<int> relatedCaseIds)
            : base(circularName)
        {
            this.QuestionnaireId = questionnaireId;
            this.Status = status;
            this.CreatedDate = createdDate;
            this.RelatedCaseIds = relatedCaseIds;
        }

        public int QuestionnaireId { get; private set; }

        public int Status { get; private set; }

        public DateTime CreatedDate { get; private set; }

        [NotNull]
        public List<int> RelatedCaseIds { get; private set; }
    }
}
