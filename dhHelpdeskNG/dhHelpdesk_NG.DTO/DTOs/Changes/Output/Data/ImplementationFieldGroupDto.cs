namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data
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
            State = state;
            RealStartDate = realStartDate;
            BuildAndTextImplemented = buildAndTextImplemented;
            ImplementationPlanUsed = implementationPlanUsed;
            Deviation = deviation;
            RecoveryPlanUsed = recoveryPlanUsed;
            FinishingDate = finishingDate;
            ImplementationReady = implementationReady;
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
