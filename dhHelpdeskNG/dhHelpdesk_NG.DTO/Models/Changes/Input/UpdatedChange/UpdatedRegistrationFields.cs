namespace DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedRegistrationFields
    {
        #region Constructors and Destructors

        public UpdatedRegistrationFields(
            List<Contact> contacts,
            int? ownerId,
            string description,
            string businessBenefits,
            string consequence,
            string impact,
            DateTime? desiredDate,
            bool verified,
            StepStatus approval,
            DateTime? approvedDateAndTime,
            int? approvedByUserId,
            string rejectExplanation)
        {
            this.Contacts = contacts;
            this.OwnerId = ownerId;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDate;
            this.Verified = verified;
            this.Approval = approval;
            this.ApprovedDateAndTime = approvedDateAndTime;
            this.ApprovedByUserId = approvedByUserId;
            this.RejectExplanation = rejectExplanation;
        }

        #endregion

        #region Public Properties

        public List<Contact> Contacts { get; private set; }

        public StepStatus Approval { get; private set; }

        public int? ApprovedByUserId { get; private set; }

        public DateTime? ApprovedDateAndTime { get; private set; }

        public string BusinessBenefits { get; private set; }

        public string Consequence { get; private set; }

        public string Description { get; private set; }

        public DateTime? DesiredDate { get; private set; }

        public string Impact { get; private set; }

        [IsId]
        public int? OwnerId { get; private set; }

        public string RejectExplanation { get; private set; }

        public bool Verified { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static UpdatedRegistrationFields CreateEmpty()
        {
            return new UpdatedRegistrationFields(
                new List<Contact>(), 
                null,
                null,
                null,
                null,
                null,
                null,
                false,
                StepStatus.None,
                null,
                null,
                null);
        }

        #endregion
    }
}