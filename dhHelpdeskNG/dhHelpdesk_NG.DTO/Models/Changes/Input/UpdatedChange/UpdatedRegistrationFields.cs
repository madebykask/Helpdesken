namespace DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedRegistrationFields
    {
        public UpdatedRegistrationFields(
            int? ownerId,
            string description,
            string businessBenefits,
            string consequence,
            string impact,
            DateTime? desiredDate,
            bool verified,
            RegistrationApproveResult approved,
            string approvedUser,
            DateTime? approvedDateAndTime,
            string changeRecommendation)
        {
            this.ChangeRecommendation = changeRecommendation;
            this.ApprovedUser = approvedUser;
            this.ApprovedDateAndTime = approvedDateAndTime;
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

        public DateTime? ApprovedDateAndTime { get; private set; }

        public string ApprovedUser { get; private set; }

        public string ChangeRecommendation { get; private set; }
    }
}
