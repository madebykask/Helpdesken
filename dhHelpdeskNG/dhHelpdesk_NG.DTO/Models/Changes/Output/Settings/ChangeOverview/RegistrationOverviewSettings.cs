namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistrationOverviewSettings
    {
        public RegistrationOverviewSettings(
            FieldOverviewSetting owner,
            FieldOverviewSetting description,
            FieldOverviewSetting businessBenefits,
            FieldOverviewSetting consequence,
            FieldOverviewSetting impact,
            FieldOverviewSetting desiredDate,
            FieldOverviewSetting verified,
            FieldOverviewSetting approval,
            FieldOverviewSetting rejectExplanation)
        {
            this.Owner = owner;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDate;
            this.Verified = verified;
            this.Approval = approval;
            this.RejectExplanation = rejectExplanation;
        }

        [NotNull]
        public FieldOverviewSetting Owner { get; private set; }

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
        public FieldOverviewSetting Approval { get; private set; }

        [NotNull]
        public FieldOverviewSetting RejectExplanation { get; private set; }
    }
}
