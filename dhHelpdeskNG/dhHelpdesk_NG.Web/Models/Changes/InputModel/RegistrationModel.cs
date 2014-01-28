namespace dhHelpdesk_NG.Web.Models.Changes.InputModel
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class RegistrationModel
    {
        public RegistrationModel()
        {
            this.ProcessAffectedIds = new List<int>();
            this.DepartmentAffectedIds = new List<int>();
        }

        public RegistrationModel(
            string id,
            SelectList owner,
            MultiSelectList processesAffected,
            MultiSelectList departmentsAffected,
            string description,
            string businessBenefits,
            string consequence,
            string impact,
            DateTime? desiredDateTime,
            bool verified,
            AttachedFilesContainerModel attachedFilesContainer,
            SelectList approved,
            string approvableExplanation,
            DateTime? approvedDateAndTime,
            string approvedUser)
        {
            this.Id = id;
            this.Owner = owner;
            this.ProcessesAffected = processesAffected;
            this.DepartmentsAffected = departmentsAffected;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDateTime;
            this.Verified = verified;
            this.AttachedFilesContainer = attachedFilesContainer;
            this.Approved = approved;
            this.ApprovableExplanation = approvableExplanation;
            this.ApprovedDateAndTime = approvedDateAndTime;
            this.ApprovedUser = approvedUser;
        }

        public string Id { get; set; }

        [LocalizedDisplay("Owner")]
        public SelectList Owner { get; set; }

        [IsId]
        public int? OwnerId { get; set; }

        [LocalizedDisplay("Processes affected")]
        public MultiSelectList ProcessesAffected { get; set; }

        [NotNull]
        public List<int> ProcessAffectedIds { get; set; }

        [LocalizedDisplay("Departments affected")]
        public MultiSelectList DepartmentsAffected { get; set; }

        [NotNull]
        public List<int> DepartmentAffectedIds { get; set; }

        [LocalizedDisplay("Description")]
        public string Description { get; set; }

        [LocalizedDisplay("Business benefits")]
        public string BusinessBenefits { get; set; }

        [LocalizedDisplay("Consequence")]
        public string Consequence { get; set; }

        [LocalizedDisplay("Impact")]
        public string Impact { get; set; }

        [LocalizedDisplay("Desired date")]
        public DateTime? DesiredDate { get; set; }

        [LocalizedDisplay("Verified")]
        public bool Verified { get; set; }

        [LocalizedDisplay("Attached Files")]
        public AttachedFilesContainerModel AttachedFilesContainer { get; set; }

        [LocalizedDisplay("Approved")]
        public SelectList Approved { get; set; }

        public RegistrationApproveResult ApprovedValue { get; set; }

        [LocalizedDisplay("Explanation")]
        public string ApprovableExplanation { get; set; }

        public DateTime? ApprovedDateAndTime { get; set; }

        public string ApprovedUser { get; set; }
    }
}