namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChangeAggregate
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class UpdatedRegistrationFields
    {
        public UpdatedRegistrationFields(
            List<Contact> contacts,
            int? ownerId,
            List<int> processesAffectedIds,
            List<int> departmentAffectedIds,
            string description,
            string businessBenefits,
            string consequece,
            string impact,
            DateTime? desiredDate,
            bool verified,
            List<DeletedFile> deletedFiles,
            List<NewFile> newFiles,
            RegistrationApproveResult approved,
            DateTime? approvedDateAndTime,
            string approvedUser,
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
            this.DeletedFiles = deletedFiles;
            this.NewFiles = newFiles;
            this.Approved = approved;
            this.ApprovedDateAndTime = approvedDateAndTime;
            this.ApprovedUser = approvedUser;
            this.ApprovalExplanation = approvalExplanation;
        }

        [NotNull]
        public List<Contact> Contacts { get; private set; }

        [IsId]
        public int? OwnerId { get; private set; }

        [NotNull]
        public List<int> ProcessesAffectedIds { get; private set; }

        [NotNull]
        public List<int> DepartmentAffectedIds { get; private set; }

        public string Description { get; private set; }

        public string BusinessBenefits { get; private set; }

        public string Consequece { get; private set; }

        public string Impact { get; private set; }

        public DateTime? DesiredDate { get; private set; }

        public bool Verified { get; private set; }

        [NotNull]
        public List<DeletedFile> DeletedFiles { get; private set; }

        [NotNull]
        public List<NewFile> NewFiles { get; private set; } 

        public RegistrationApproveResult Approved { get; private set; }

        public DateTime? ApprovedDateAndTime { get; private set; }

        public string ApprovedUser { get; private set; }

        public string ApprovalExplanation { get; private set; }
    }
}
