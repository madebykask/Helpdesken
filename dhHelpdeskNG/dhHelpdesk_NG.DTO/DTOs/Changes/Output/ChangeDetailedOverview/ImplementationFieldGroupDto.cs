namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeDetailedOverview
{
    using System;

    public sealed class ImplementationFieldGroupDto
    {
        public ImplementationFieldGroupDto(
            string state,
            DateTime? realStartDate,
            bool buildAndTextImplemented,
            bool implementationPlanUsed,
            string deviation,
            bool recoveryPlanUsed,
            DateTime? finishingDate,
            bool implementationReady)
        {
            this.State = state;
            this.RealStartDate = realStartDate;
            this.BuildAndTextImplemented = buildAndTextImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.FinishingDate = finishingDate;
            this.ImplementationReady = implementationReady;
        }

        public string State { get; private set; }

        public DateTime? RealStartDate { get; private set; }

        public bool BuildAndTextImplemented { get; private set; }

        public bool ImplementationPlanUsed { get; private set; }

        public string Deviation { get; private set; }

        public bool RecoveryPlanUsed { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public bool ImplementationReady { get; private set; }
    }
}
