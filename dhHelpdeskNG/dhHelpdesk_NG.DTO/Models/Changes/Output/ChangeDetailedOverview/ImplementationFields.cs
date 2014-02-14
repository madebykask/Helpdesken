namespace DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview
{
    using System;

    public sealed class ImplementationFields
    {
        public ImplementationFields(
            string status,
            DateTime? realStartDate,
            bool buildImplemented,
            bool implementationPlanUsed,
            string deviation,
            bool recoveryPlanUsed,
            DateTime? finishingDate,
            bool implementationReady)
        {
            this.Status = status;
            this.RealStartDate = realStartDate;
            this.BuildImplemented = buildImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.FinishingDate = finishingDate;
            this.ImplementationReady = implementationReady;
        }

        public string Status { get; private set; }

        public DateTime? RealStartDate { get; private set; }

        public bool BuildImplemented { get; private set; }

        public bool ImplementationPlanUsed { get; private set; }

        public string Deviation { get; private set; }

        public bool RecoveryPlanUsed { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public bool ImplementationReady { get; private set; }
    }
}
