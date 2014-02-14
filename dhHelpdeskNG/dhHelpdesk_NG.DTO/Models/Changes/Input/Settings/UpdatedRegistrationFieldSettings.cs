namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedRegistrationFieldSettings
    {
        public UpdatedRegistrationFieldSettings(
            UpdatedFieldSetting owner,
            UpdatedFieldSetting affectedProcesses,
            UpdatedFieldSetting affectedDepartments,
            UpdatedTextFieldSetting description,
            UpdatedTextFieldSetting businessBenefits,
            UpdatedTextFieldSetting consequence,
            UpdatedFieldSetting impact,
            UpdatedFieldSetting desiredDate,
            UpdatedFieldSetting verified,
            UpdatedFieldSetting attachedFiles,
            UpdatedFieldSetting approval,
            UpdatedFieldSetting rejectExplanation)
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
        public UpdatedFieldSetting Owner { get; private set; }

        [NotNull]
        public UpdatedFieldSetting AffectedProcesses { get; private set; }

        [NotNull]
        public UpdatedFieldSetting AffectedDepartments { get; private set; }

        [NotNull]
        public UpdatedTextFieldSetting Description { get; private set; }

        [NotNull]
        public UpdatedTextFieldSetting BusinessBenefits { get; private set; }

        [NotNull]
        public UpdatedTextFieldSetting Consequence { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Impact { get; private set; }

        [NotNull]
        public UpdatedFieldSetting DesiredDate { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Verified { get; private set; }

        [NotNull]
        public UpdatedFieldSetting AttachedFiles { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Approval { get; private set; }

        [NotNull]
        public UpdatedFieldSetting RejectExplanation { get; private set; }
    }
}
