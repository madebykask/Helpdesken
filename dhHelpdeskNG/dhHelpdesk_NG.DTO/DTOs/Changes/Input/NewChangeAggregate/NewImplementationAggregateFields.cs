namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NewImplementationAggregateFields
    {
        public NewImplementationAggregateFields(
            int? implementationStatusId,
            DateTime? realStartDate,
            DateTime? finishingDate,
            bool buildImplemented,
            bool implementationPlanUsed,
            string changeDeviation,
            bool recoveryPlanUsed,
            List<NewFile> attachedFiles,
            bool ready)
        {
            this.ImplementationStatusId = implementationStatusId;
            this.RealStartDate = realStartDate;
            this.FinishingDate = finishingDate;
            this.BuildImplemented = buildImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.ChangeDeviation = changeDeviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.AttachedFiles = attachedFiles;
            this.ImplementationReady = ready;
        }

        [IsId]
        public int? ImplementationStatusId { get; private set; }

        public DateTime? RealStartDate { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public bool BuildImplemented { get; private set; }

        public bool ImplementationPlanUsed { get; private set; }

        public string ChangeDeviation { get; private set; }

        public bool RecoveryPlanUsed { get; private set; }

        [NotNull]
        public List<NewFile> AttachedFiles { get; private set; }

        public bool ImplementationReady { get; private set; }
    }
}
