namespace dhHelpdesk_NG.DTO.DTOs.Changes.Change
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class RegistrationFields
    {
        public RegistrationFields(
            int? ownerId,
            string description,
            string businessBenefits,
            string consequence,
            string impact,
            DateTime? desiredDate,
            bool verified,
            RegistrationApproveResult approved,
            string changeRecommendation)
        {
            this.ChangeRecommendation = changeRecommendation;
            this.Approved = approved;
            this.Verified = verified;
            this.DesiredDate = desiredDate;
            this.Impact = impact;
            this.Consequence = consequence;
            this.BusinessBenefits = businessBenefits;
            this.Description = description;
            this.OwnerId = ownerId;
        }

        [IsId]
        public int? OwnerId { get; private set; }

        public string Description { get; private set; }

        public string BusinessBenefits { get; private set; }

        public string Consequence { get; private set; }

        public string Impact { get; private set; }

        public DateTime? DesiredDate { get; private set; }

        public bool Verified { get; private set; }

        public RegistrationApproveResult Approved { get; private set; }

        public string ChangeRecommendation { get; private set; }
    }
}
