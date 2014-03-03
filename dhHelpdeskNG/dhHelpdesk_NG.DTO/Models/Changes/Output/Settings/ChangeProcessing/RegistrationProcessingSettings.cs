namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistrationProcessingSettings
    {
        public RegistrationProcessingSettings(
            FieldProcessingSetting owner,
            FieldProcessingSetting affectedProcesses,
            FieldProcessingSetting affectedDepartments,
            FieldProcessingSetting description,
            FieldProcessingSetting businessBenefits,
            FieldProcessingSetting consequence,
            FieldProcessingSetting impact,
            FieldProcessingSetting desiredDate,
            FieldProcessingSetting verified,
            FieldProcessingSetting attachedFiles,
            FieldProcessingSetting approval,
            FieldProcessingSetting rejectExplanation)
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
        public FieldProcessingSetting Owner { get; private set; }

        [NotNull]
        public FieldProcessingSetting AffectedProcesses { get; private set; }

        [NotNull]
        public FieldProcessingSetting AffectedDepartments { get; private set; }

        [NotNull]
        public FieldProcessingSetting Description { get; private set; }

        [NotNull]
        public FieldProcessingSetting BusinessBenefits { get; private set; }

        [NotNull]
        public FieldProcessingSetting Consequence { get; private set; }

        [NotNull]
        public FieldProcessingSetting Impact { get; private set; }

        [NotNull]
        public FieldProcessingSetting DesiredDate { get; private set; }

        [NotNull]
        public FieldProcessingSetting Verified { get; private set; }

        [NotNull]
        public FieldProcessingSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldProcessingSetting Approval { get; private set; }

        [NotNull]
        public FieldProcessingSetting RejectExplanation { get; private set; }
    }
}
