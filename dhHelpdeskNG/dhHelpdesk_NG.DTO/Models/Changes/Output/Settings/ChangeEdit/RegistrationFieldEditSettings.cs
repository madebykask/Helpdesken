namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistrationFieldEditSettings
    {
        public RegistrationFieldEditSettings(
            FieldEditSetting owner,
            FieldEditSetting affectedProcesses,
            FieldEditSetting affectedDepartments,
            TextFieldEditSetting description,
            TextFieldEditSetting businessBenefits,
            TextFieldEditSetting consequence,
            FieldEditSetting impact,
            FieldEditSetting desiredDate,
            FieldEditSetting verified,
            FieldEditSetting attachedFiles,
            FieldEditSetting approval,
            FieldEditSetting rejectExplanation)
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
        public FieldEditSetting Owner { get; private set; }

        [NotNull]
        public FieldEditSetting AffectedProcesses { get; private set; }

        [NotNull]
        public FieldEditSetting AffectedDepartments { get; private set; }

        [NotNull]
        public TextFieldEditSetting Description { get; private set; }

        [NotNull]
        public TextFieldEditSetting BusinessBenefits { get; private set; }

        [NotNull]
        public TextFieldEditSetting Consequence { get; private set; }

        [NotNull]
        public FieldEditSetting Impact { get; private set; }

        [NotNull]
        public FieldEditSetting DesiredDate { get; private set; }

        [NotNull]
        public FieldEditSetting Verified { get; private set; }

        [NotNull]
        public FieldEditSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldEditSetting Approval { get; private set; }

        [NotNull]
        public FieldEditSetting RejectExplanation { get; private set; }
    }
}
