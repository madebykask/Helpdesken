namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit.Contacts;

    public sealed class RegistrationModel
    {
        #region Constructors and Destructors

        public RegistrationModel()
        {
            this.AffectedProcessIds = new List<int>();
            this.AffectedDepartmentIds = new List<int>();
        }

        public RegistrationModel(
            string changeId,
            ContactsModel contacts,
            ConfigurableFieldModel<SelectList> owners,
            ConfigurableFieldModel<MultiSelectList> affectedProcesses,
            ConfigurableFieldModel<MultiSelectList> affectedDepartments,
            ConfigurableFieldModel<string> description,
            ConfigurableFieldModel<string> businessBenefits,
            ConfigurableFieldModel<string> consequence,
            ConfigurableFieldModel<string> impact,
            ConfigurableFieldModel<DateTime?> desiredDate,
            ConfigurableFieldModel<bool> verified,
            ConfigurableFieldModel<AttachedFilesModel> attachedFiles,
            ConfigurableFieldModel<SelectList> approvalResults,
            DateTime? approvedDateAndTime,
            UserName approvedByUser,
            ConfigurableFieldModel<string> rejectExplanation)
        {
            this.ChangeId = changeId;
            this.Contacts = contacts;
            this.Owners = owners;
            this.AffectedProcesses = affectedProcesses;
            this.AffectedDepartments = affectedDepartments;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDate;
            this.Verified = verified;
            this.AttachedFiles = attachedFiles;
            this.ApprovalResults = approvalResults;
            this.ApprovedDateAndTime = approvedDateAndTime;
            this.ApprovedByUser = approvedByUser;
            this.RejectExplanation = rejectExplanation;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public List<int> AffectedDepartmentIds { get; set; }

        [NotNull]
        public ConfigurableFieldModel<MultiSelectList> AffectedDepartments { get; set; }

        [NotNull]
        public List<int> AffectedProcessIds { get; set; }

        [NotNull]
        public ConfigurableFieldModel<MultiSelectList> AffectedProcesses { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> ApprovalResults { get; set; }

        public StepStatus ApprovalValue { get; set; }

        public UserName ApprovedByUser { get; set; }

        public DateTime? ApprovedDateAndTime { get; set; }

        [NotNull]
        public ConfigurableFieldModel<AttachedFilesModel> AttachedFiles { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> BusinessBenefits { get; set; }

        [NotNullAndEmpty]
        public string ChangeId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Consequence { get; set; }

        [NotNull]
        public ContactsModel Contacts { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Description { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> DesiredDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Impact { get; set; }

        [IsId]
        public int? OwnerId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Owners { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> RejectExplanation { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> Verified { get; set; }

        #endregion

        public bool HasShowableFields()
        {
            return this.AffectedDepartments.Show ||
                this.AffectedProcesses.Show ||
                this.ApprovalResults.Show ||
                this.AttachedFiles.Show ||
                this.BusinessBenefits.Show ||
                this.Consequence.Show ||
                !this.Contacts.IsUnshowable() ||
                this.Description.Show ||
                this.DesiredDate.Show ||
                this.Impact.Show ||
                this.Owners.Show ||
                this.RejectExplanation.Show ||
                this.Verified.Show;
        }
    }
}