namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistrationModel
    {
        public RegistrationModel()
        {
            this.AffectedProcessIds = new List<int>();
            this.AffectedDepartmentIds = new List<int>();
        }

        public RegistrationModel(
            string changeId,
            ContactsModel contacts,
            ConfigurableFieldModel<SelectList> owner,
            ConfigurableFieldModel<MultiSelectList> affectedProcesses,
            ConfigurableFieldModel<MultiSelectList> affectedDepartments,
            ConfigurableFieldModel<string> description,
            ConfigurableFieldModel<string> businessBenefits,
            ConfigurableFieldModel<string> consequence,
            ConfigurableFieldModel<string> impact,
            ConfigurableFieldModel<DateTime?> desiredDateAndTime,
            ConfigurableFieldModel<bool> verified,
            ConfigurableFieldModel<AttachedFilesModel> attachedFiles,
            ConfigurableFieldModel<SelectList> approval,
            DateTime? approvedDateAndTime,
            UserName approvedByUser,
            ConfigurableFieldModel<string> rejectExplanation)
        {
            this.ChangeId = changeId;
            this.Contacts = contacts;
            this.Owner = owner;
            this.AffectedProcesses = affectedProcesses;
            this.AffectedDepartments = affectedDepartments;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDateAndTime = desiredDateAndTime;
            this.Verified = verified;
            this.AttachedFiles = attachedFiles;
            this.Approval = approval;
            this.ApprovedDateAndTime = approvedDateAndTime;
            this.ApprovedByUser = approvedByUser;
            this.RejectExplanation = rejectExplanation;
        }

        [NotNullAndEmpty]
        public string ChangeId { get; set; }

        [NotNull]
        public ContactsModel Contacts { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Owner { get; private set; }

        [IsId]
        public int? OwnerId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<MultiSelectList> AffectedProcesses { get; private set; }

        [NotNull]
        public List<int> AffectedProcessIds { get; set; }

        [NotNull]
        public ConfigurableFieldModel<MultiSelectList> AffectedDepartments { get; private set; }

        [NotNull]
        public List<int> AffectedDepartmentIds { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Description { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> BusinessBenefits { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Consequence { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Impact { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> DesiredDateAndTime { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> Verified { get; set; }

        [NotNull]
        public ConfigurableFieldModel<AttachedFilesModel> AttachedFiles { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Approval { get; private set; }

        public StepStatus ApprovalValue { get; set; }

        public DateTime? ApprovedDateAndTime { get; private set; }

        public UserName ApprovedByUser { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<string> RejectExplanation { get; set; }
    }
}