﻿namespace DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewImplementationFields
    {
        public NewImplementationFields(
            int? statusId,
            DateTime? realStartDate,
            DateTime? finishingDate,
            bool buildImplemented,
            bool implementationPlanUsed,
            string deviation,
            bool recoveryPlanUsed,
            bool implementationReady)
        {
            this.StatusId = statusId;
            this.RealStartDate = realStartDate;
            this.FinishingDate = finishingDate;
            this.BuildImplemented = buildImplemented;
            this.ImplementationPlanUsed = implementationPlanUsed;
            this.Deviation = deviation;
            this.RecoveryPlanUsed = recoveryPlanUsed;
            this.ImplementationReady = implementationReady;
        }

        [IsId]
        public int? StatusId { get; private set; }

        public DateTime? RealStartDate { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public bool BuildImplemented { get; private set; }

        public bool ImplementationPlanUsed { get; private set; }

        public string Deviation { get; private set; }

        public bool RecoveryPlanUsed { get; private set; }

        public bool ImplementationReady { get; private set; }
    }
}