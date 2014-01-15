namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class RegistrationFieldOverviewSettingGroupDto
    {
        public RegistrationFieldOverviewSettingGroupDto(
            FieldOverviewSettingDto description,
            FieldOverviewSettingDto businessBenefits,
            FieldOverviewSettingDto consequence,
            FieldOverviewSettingDto impact,
            FieldOverviewSettingDto desiredDate,
            FieldOverviewSettingDto verified,
            FieldOverviewSettingDto approval,
            FieldOverviewSettingDto explanation)
        {
            Description = description;
            BusinessBenefits = businessBenefits;
            Consequence = consequence;
            Impact = impact;
            DesiredDate = desiredDate;
            Verified = verified;
            Approval = approval;
            Explanation = explanation;
        }

        [NotNull]
        public FieldOverviewSettingDto Description { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto BusinessBenefits { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Consequence { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Impact { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto DesiredDate { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Verified { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Approval { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Explanation { get; private set; }
    }
}
