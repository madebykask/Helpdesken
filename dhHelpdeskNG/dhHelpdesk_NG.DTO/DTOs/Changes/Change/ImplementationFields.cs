namespace dhHelpdesk_NG.DTO.DTOs.Changes.Change
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ImplementationFields
    {
        public ImplementationFields(
            int? implementationStatusId,
            DateTime? realStartDate,
            DateTime? finishingDate,
            bool buildImplemented,
            bool implementationPlanUsed,
            string changeDeviation,
            bool recoveryPlanUsed,
            bool ready) 
        {
            this.Ready = ready;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.ChangeDeviation = changeDeviation;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.BuildImplemented = buildImplemented;
            this.FinishingDate = finishingDate;
            this.RealStartDate = realStartDate;
            this.ImplementationStatusId = implementationStatusId;
        }

        [IsId]
        public int? ImplementationStatusId { get; private set; }

        public DateTime? RealStartDate { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public bool BuildImplemented { get; private set; }

        public bool ImplementationPlanUsed { get; private set; }

        public string ChangeDeviation { get; private set; }

        public bool RecoveryPlanUsed { get; private set; }

        public bool Ready { get; private set; }
    }
}
