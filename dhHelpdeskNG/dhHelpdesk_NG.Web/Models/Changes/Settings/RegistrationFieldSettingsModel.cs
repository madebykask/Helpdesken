namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class RegistrationFieldSettingsModel
    {
        public RegistrationFieldSettingsModel()
        {
        }

        public RegistrationFieldSettingsModel(
            FieldSettingModel owner,
            FieldSettingModel affectedProcesses,
            FieldSettingModel affectedDepartments,
            StringFieldSettingModel description,
            StringFieldSettingModel businessBenefits,
            StringFieldSettingModel consequence,
            FieldSettingModel impact,
            FieldSettingModel desiredDate,
            FieldSettingModel verified,
            FieldSettingModel attachedFiles,
            FieldSettingModel approval,
            FieldSettingModel rejectExplanation)
        {
            this.Owner = owner;
            this.AffectedProcesses = affectedProcesses;
            this.AffectedDepartments = affectedDepartments;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDate;
            this.Verified = verified;
            this.AttachedFiles = attachedFiles;
            this.Approval = approval;
            this.RejectExplanation = rejectExplanation;
        }

        [NotNull]
        [LocalizedDisplay("Owner")]
        public FieldSettingModel Owner { get; set; }

        [NotNull]
        [LocalizedDisplay("Affected Processes")]
        public FieldSettingModel AffectedProcesses { get; set; }

        [NotNull]
        [LocalizedDisplay("Affected Departments")]
        public FieldSettingModel AffectedDepartments { get; set; }

        [NotNull]
        [LocalizedDisplay("Description")]
        public StringFieldSettingModel Description { get; set; }

        [NotNull]
        [LocalizedDisplay("Business Benefits")]
        public StringFieldSettingModel BusinessBenefits { get; set; }

        [NotNull]
        [LocalizedDisplay("Consequence")]
        public StringFieldSettingModel Consequence { get; set; }

        [NotNull]
        [LocalizedDisplay("Impact")]
        public FieldSettingModel Impact { get; set; }

        [NotNull]
        [LocalizedDisplay("Desired date")]
        public FieldSettingModel DesiredDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Verified")]
        public FieldSettingModel Verified { get; set; }

        [NotNull]
        [LocalizedDisplay("Attached Files")]
        public FieldSettingModel AttachedFiles { get; set; }

        [NotNull]
        [LocalizedDisplay("Approval")]
        public FieldSettingModel Approval { get; set; }

        [NotNull]
        [LocalizedDisplay("Reject Explanation")]
        public FieldSettingModel RejectExplanation { get; set; }
    }
}
