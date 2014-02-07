namespace DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewImplementationFields
    {
        public NewImplementationFields(
            int? implementationStatusId,
            DateTime? realStartDate,
            DateTime? finishingDate,
            bool buildImplemented,
            bool implementationPlanUsed,
            string changeDeviation,
            bool recoveryPlanUsed,
            bool implementaionReady) 
        {
            this.ImplementaionReady = implementaionReady;
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

        public bool ImplementaionReady { get; private set; }
    }
}
