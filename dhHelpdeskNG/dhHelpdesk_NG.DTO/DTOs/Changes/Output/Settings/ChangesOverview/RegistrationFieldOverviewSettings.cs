namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangesOverview
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class RegistrationFieldOverviewSettings
    {
        public RegistrationFieldOverviewSettings(
            FieldOverviewSetting description,
            FieldOverviewSetting businessBenefits,
            FieldOverviewSetting consequence,
            FieldOverviewSetting impact,
            FieldOverviewSetting desiredDate,
            FieldOverviewSetting verified,
            FieldOverviewSetting approval,
            FieldOverviewSetting explanation)
        {
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDate;
            this.Verified = verified;
            this.Approval = approval;
            this.Explanation = explanation;
        }

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
        public FieldOverviewSetting Explanation { get; private set; }
    }
}
