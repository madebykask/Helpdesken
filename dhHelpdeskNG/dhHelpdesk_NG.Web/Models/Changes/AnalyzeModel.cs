namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DataAnnotationsExtensions;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class AnalyzeModel
    {
        public AnalyzeModel()
        {
            this.AttachedFiles = new List<string>();
        }

        [LocalizedDisplay("Category")]
        public SelectList Category { get; set; }

        [IsId]
        public int CategoryId { get; set; }

        [LocalizedDisplay("Priority")]
        public SelectList Priority { get; set; }

        [IsId]
        public int PriorityId { get; set; }

        [LocalizedDisplay("Responsible")]
        public SelectList Responsible { get; set; }

        [IsId]
        public int ResponsibleId { get; set; }

        [LocalizedDisplay("Solution")]
        public string Solution { get; set; }

        [LocalizedDisplay("Cost")]
        public int Cost { get; set; }

        [LocalizedDisplay("Yearly cost")]
        public int YearlyCost { get; set; }

        [LocalizedDisplay("Currency")]
        public SelectList Currency { get; set; }

        [IsId]
        public int CurrencyId { get; set; }

        [Min(0)]
        [LocalizedDisplay("Time estimates hours")]
        public int TimeEstimatesHours { get; set; }

        [LocalizedDisplay("Risk")]
        public string Risk { get; set; }

        [LocalizedDisplay("Start date")]
        public DateTime StartDate { get; set; }

        [LocalizedDisplay("End date")]
        public DateTime EndDate { get; set; }

        [LocalizedDisplay("Has implementation plan")]
        public bool HasImplementationPlan { get; set; }

        [LocalizedDisplay("Has recovery plan")]
        public bool HasRecoveryPlan { get; set; }

        [NotNull]
        [LocalizedDisplay("Attached files")]
        public List<string> AttachedFiles { get; set; }

        [LocalizedDisplay("Approved")]
        public SelectList Approved { get; set; }

        public Enums.AnalyzeApproveResult ApprovedValue { get; set; }

        [LocalizedDisplay("Reject recommendation")]
        public string RejectRecommendation { get; set; }
    }
}