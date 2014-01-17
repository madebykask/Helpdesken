namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class ImplementationModel
    {
        [LocalizedDisplay("Implementation status")]
        public SelectList ImplementationStatus { get; set; }

        [IsId]
        public int ImplementationStatusId { get; set; }

        [LocalizedDisplay("Real start date")]
        public DateTime RealStartDate { get; set; }

        [LocalizedDisplay("Finishing date")]
        public DateTime FinishingDate { get; set; }

        [LocalizedDisplay("Build implemented")]
        public bool BuildImplemented { get; set; }

        [LocalizedDisplay("Implementation plan used")]
        public bool ImplementationPlanUsed { get; set; }

        [LocalizedDisplay("Change deviation")]
        public string ChangeDeviation { get; set; }

        [LocalizedDisplay("Recovery plan used")]
        public bool RecoveryPlanUsed { get; set; }

        [LocalizedDisplay("Attached files")]
        public List<string> AttachedFiles { get; set; }

        [LocalizedDisplay("Implementation ready")]
        public bool ImplementationReady { get; set; }
    }
}