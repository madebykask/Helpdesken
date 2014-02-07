namespace DH.Helpdesk.Web.Models.Changes.Edit
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

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
            ConfigurableFieldModel<MultiSelectList> processesAffected,
            ConfigurableFieldModel<MultiSelectList> departmentsAffected,
            ConfigurableFieldModel<string> description,
            ConfigurableFieldModel<string> businessBenefits,
            ConfigurableFieldModel<string> consequence,
            ConfigurableFieldModel<string> impact,
            ConfigurableFieldModel<DateTime?> desiredDateTime,
            ConfigurableFieldModel<bool> verified,
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
        public ConfigurableFieldModel<MultiSelectList> ProcessesAffected { get; set; }

        [NotNull]
        public List<int> ProcessAffectedIds { get; set; }

        [LocalizedDisplay("Departments affected")]
        public ConfigurableFieldModel<MultiSelectList> DepartmentsAffected { get; set; }

        [NotNull]
        public List<int> DepartmentAffectedIds { get; set; }

        [LocalizedDisplay("Description")]
        public ConfigurableFieldModel<string> Description { get; set; }

        [LocalizedDisplay("Business benefits")]
        public ConfigurableFieldModel<string> BusinessBenefits { get; set; }

        [LocalizedDisplay("Consequence")]
        public ConfigurableFieldModel<string> Consequence { get; set; }

        [LocalizedDisplay("Impact")]
        public ConfigurableFieldModel<string> Impact { get; set; }

        [LocalizedDisplay("Desired date")]
        public ConfigurableFieldModel<DateTime?> DesiredDate { get; set; }

        [LocalizedDisplay("Verified")]
        public ConfigurableFieldModel<bool> Verified { get; set; }

        [LocalizedDisplay("Attached Files")]
        public AttachedFilesContainerModel AttachedFilesContainer { get; set; }

        [LocalizedDisplay("Approved")]
        public SelectList Approved { get; set; }

        public RegistrationApprovalResult ApprovedValue { get; set; }

        [LocalizedDisplay("Explanation")]
        public string ApprovableExplanation { get; set; }

        public DateTime? ApprovedDateAndTime { get; set; }

        public string ApprovedUser { get; set; }
    }
}