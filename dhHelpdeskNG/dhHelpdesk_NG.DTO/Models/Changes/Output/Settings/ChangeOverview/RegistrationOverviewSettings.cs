namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistrationOverviewSettings
    {
        public RegistrationOverviewSettings(
            FieldOverviewSetting name,
            FieldOverviewSetting phone,
            FieldOverviewSetting email,
            FieldOverviewSetting company,
            FieldOverviewSetting owner,
            FieldOverviewSetting affectedProcesses,
            FieldOverviewSetting affectedDepartments,
            FieldOverviewSetting description,
            FieldOverviewSetting businessBenefits,
            FieldOverviewSetting consequence,
            FieldOverviewSetting impact,
            FieldOverviewSetting desiredDate,
            FieldOverviewSetting verified,
            FieldOverviewSetting attachedFiles,
            FieldOverviewSetting approval,
            FieldOverviewSetting rejectExplanation)
        {
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Company = company;
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
        public FieldOverviewSetting Name { get; private set; }

        [NotNull]
        public FieldOverviewSetting Phone { get; private set; }

        [NotNull]
        public FieldOverviewSetting Email { get; private set; }

        [NotNull]
        public FieldOverviewSetting Company { get; private set; }

        [NotNull]
        public FieldOverviewSetting Owner { get; private set; }

        [NotNull]
        public FieldOverviewSetting AffectedProcesses { get; private set; }

        [NotNull]
        public FieldOverviewSetting AffectedDepartments { get; private set; }

        [NotNull]
        public FieldOverviewSetting Description { get; private set; }

        [NotNull]
        public FieldOverviewSetting BusinessBenefits { get; private set; }

        [NotNull]
        public FieldOverviewSetting Consequence { get; private set; }

        [NotNull]
        public FieldOverviewSetting Impact { get; private set; }

        [NotNull]
        public FieldOverviewSetting DesiredDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting Verified { get; private set; }

        [NotNull]
        public FieldOverviewSetting AttachedFiles { get; private set; }

        [NotNull]
        public FieldOverviewSetting Approval { get; private set; }

        [NotNull]
        public FieldOverviewSetting RejectExplanation { get; private set; }
    }
}
