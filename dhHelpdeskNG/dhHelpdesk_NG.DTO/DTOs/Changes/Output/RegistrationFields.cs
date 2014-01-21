namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class RegistrationFields
    {
        public RegistrationFields(
            List<Contact> contacts,
            int ownerId,
            List<int> processesAffectedIds,
            List<int> departmentAffectedIds,
            string description,
            string businessBenefits,
            string consequece,
            string impact,
            DateTime desiredDate,
            bool verified,
            RegistrationApproveResult approved,
            string approvalExplanation)
        {
            this.Contacts = contacts;
            this.OwnerId = ownerId;
            this.ProcessesAffectedIds = processesAffectedIds;
            this.DepartmentAffectedIds = departmentAffectedIds;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequece = consequece;
            this.Impact = impact;
            this.DesiredDate = desiredDate;
            this.Verified = verified;
            this.Approved = approved;
            this.ApprovalExplanation = approvalExplanation;
        }

        [NotNull]
        public List<Contact> Contacts { get; private set; }

        [IsId]
        public int OwnerId { get; private set; }

        [NotNull]
        public List<int> ProcessesAffectedIds { get; private set; }

        [NotNull]
        public List<int> DepartmentAffectedIds { get; private set; }

        public string Description { get; private set; }

        public string BusinessBenefits { get; private set; }

        public string Consequece { get; private set; }

        public string Impact { get; private set; }

        public DateTime DesiredDate { get; private set; }

        public bool Verified { get; private set; }

        public RegistrationApproveResult Approved { get; private set; }

        public string ApprovalExplanation { get; private set; }
    }
}
