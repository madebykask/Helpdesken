namespace DH.Helpdesk.BusinessData.Models.Changes
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class History : BusinessModel
    {
        public static History CreateNew(
            int changeId,
            UpdatedOrdererFields orderer,
            UpdatedGeneralFields general,
            UpdatedRegistrationFields registration,
            UpdatedAnalyzeFields analyze,
            UpdatedImplementationFields implementation,
            UpdatedEvaluationFields evaluation,
            int createdByUserId,
            DateTime createdDateAndTime)
        {
            return new History
                   {
                       ChangeId = changeId,
                       Orderer = orderer,
                       General = general,
                       Registration = registration,
                       Analyze = analyze,
                       Implementation = implementation,
                       Evaluation = evaluation,
                       CreatedByUserId = createdByUserId,
                       CreatedDateAndTime = createdDateAndTime
                   };
        }

        private History()
        {
        }

        [IsId]
        public int ChangeId { get; private set; }

        [NotNull]
        public UpdatedOrdererFields Orderer { get; private set; }

        [NotNull]
        public UpdatedGeneralFields General { get; private set; }

        [NotNull]
        public UpdatedRegistrationFields Registration { get; private set; }

        [NotNull]
        public UpdatedAnalyzeFields Analyze { get; private set; }

        [NotNull]
        public UpdatedImplementationFields Implementation { get; private set; }

        [NotNull]
        public UpdatedEvaluationFields Evaluation { get; private set; }

        public DateTime CreatedDateAndTime { get; private set; }

        [IsId]
        public int CreatedByUserId { get; private set; }
    }
}