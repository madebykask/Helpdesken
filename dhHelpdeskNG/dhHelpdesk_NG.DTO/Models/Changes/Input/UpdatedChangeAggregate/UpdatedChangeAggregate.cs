namespace DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChangeAggregate
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedChangeAggregate
    {
        public UpdatedChangeAggregate(
            int id,
            UpdatedChangeHeader header,
            UpdatedRegistrationFields registration,
            UpdatedAnalyzeFields analyze,
            UpdatedImplementationFields implementation,
            UpdatedEvaluationFields evaluation,
            List<int> deletedLogIds,
            DateTime changedDate)
        {
            this.Id = id;
            this.Implementation = implementation;
            this.Analyze = analyze;
            this.Registration = registration;
            this.Header = header;
            this.Evaluation = evaluation;
            this.DeletedLogIds = deletedLogIds;
            this.ChangedDate = changedDate;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public UpdatedChangeHeader Header { get; private set; }

        [NotNull]
        public UpdatedRegistrationFields Registration { get; private set; }

        [NotNull]
        public UpdatedAnalyzeFields Analyze { get; private set; }

        [NotNull]
        public UpdatedImplementationFields Implementation { get; private set; }

        [NotNull]
        public UpdatedEvaluationFields Evaluation { get; private set; }

        [NotNull]
        public List<int> DeletedLogIds { get; private set; }

        public DateTime ChangedDate { get; private set; }
    }
}
