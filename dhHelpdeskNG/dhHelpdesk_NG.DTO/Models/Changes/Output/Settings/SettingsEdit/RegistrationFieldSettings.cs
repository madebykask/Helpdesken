namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistrationFieldSettings
    {
        public RegistrationFieldSettings(
            FieldSetting owner,
            FieldSetting affectedProcesses,
            FieldSetting affectedDepartments,
            TextFieldSetting description,
            TextFieldSetting businessBenefits,
            TextFieldSetting consequence,
            FieldSetting impact,
            FieldSetting desiredDate,
            FieldSetting verified,
            FieldSetting attachedFiles,
            FieldSetting approval,
            FieldSetting rejectExplanation)
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
        public FieldSetting Owner { get; private set; }

        [NotNull]
        public FieldSetting AffectedProcesses { get; private set; }

        [NotNull]
        public FieldSetting AffectedDepartments { get; private set; }

        [NotNull]
        public TextFieldSetting Description { get; private set; }

        [NotNull]
        public TextFieldSetting BusinessBenefits { get; private set; }

        [NotNull]
        public TextFieldSetting Consequence { get; private set; }

        [NotNull]
        public FieldSetting Impact { get; private set; }

        [NotNull]
        public FieldSetting DesiredDate { get; private set; }

        [NotNull]
        public FieldSetting Verified { get; private set; }

        [NotNull]
        public FieldSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldSetting Approval { get; private set; }

        [NotNull]
        public FieldSetting RejectExplanation { get; private set; }
    }
}
