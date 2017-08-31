namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Write
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ParticipantForInsert : Participant
    {
        public ParticipantForInsert(Guid guid, bool isAnonymm, DateTime createdDate, List<Answer> answers, bool isFeedback = false)
            : base(guid)
        {
            this.IsAnonym = isAnonymm;
            this.CreatedDate = createdDate;
            this.Answers = answers;
            this.IsFeedback = isFeedback;
        }

        public bool IsAnonym { get; private set; }

        public DateTime CreatedDate { get; private set; }

        [NotNull]
        public List<Answer> Answers { get; private set; }

        public bool IsFeedback { get; private set; }
    }
}
