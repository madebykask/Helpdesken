namespace DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;

    public sealed class RegistrationFields
    {
        public RegistrationFields(
            string owner,
            string description,
            string businessBenefits,
            string consequence,
            string impact,
            DateTime? desiredDate,
            bool verified,
            RegistrationApprovalResult approval,
            string rejectExplanation)
        {
            this.Owner = owner;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDate;
            this.Verified = verified;
            this.Approval = approval;
            this.RejectExplanation = rejectExplanation;
        }

        public string Owner { get; private set; }

        public string Description { get; private set; }

        public string BusinessBenefits { get; private set; }

        public string Consequence { get; private set; }

        public string Impact { get; private set; }

        public DateTime? DesiredDate { get; private set; }

        public bool Verified { get; private set; }

        public RegistrationApprovalResult Approval { get; private set; }

        public string RejectExplanation { get; private set; }
    }
}
