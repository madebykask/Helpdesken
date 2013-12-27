namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class RegistrationFieldSettingGroupDto
    {
        public RegistrationFieldSettingGroupDto(
            FieldSettingDto name,
            FieldSettingDto phone,
            FieldSettingDto email,
            FieldSettingDto company,
            FieldSettingDto processAffected,
            FieldSettingDto departmentAffected,
            StringFieldSettingDto description,
            StringFieldSettingDto businessBenefits,
            StringFieldSettingDto consequence,
            FieldSettingDto impact,
            FieldSettingDto desiredDate,
            FieldSettingDto verified,
            FieldSettingDto attachedFile,
            FieldSettingDto approval,
            FieldSettingDto explanation)
        {
            Name = name;
            Phone = phone;
            Email = email;
            Company = company;
            ProcessAffected = processAffected;
            DepartmentAffected = departmentAffected;
            Description = description;
            BusinessBenefits = businessBenefits;
            Consequence = consequence;
            Impact = impact;
            DesiredDate = desiredDate;
            Verified = verified;
            AttachedFile = attachedFile;
            Approval = approval;
            Explanation = explanation;
        }

        [NotNull]
        public FieldSettingDto Name { get; private set; }

        [NotNull]
        public FieldSettingDto Phone { get; private set; }

        [NotNull]
        public FieldSettingDto Email { get; private set; }

        [NotNull]
        public FieldSettingDto Company { get; private set; }

        [NotNull]
        public FieldSettingDto ProcessAffected { get; private set; }

        [NotNull]
        public FieldSettingDto DepartmentAffected { get; private set; }

        [NotNull]
        public StringFieldSettingDto Description { get; private set; }

        [NotNull]
        public StringFieldSettingDto BusinessBenefits { get; private set; }

        [NotNull]
        public StringFieldSettingDto Consequence { get; private set; }

        [NotNull]
        public FieldSettingDto Impact { get; private set; }

        [NotNull]
        public FieldSettingDto DesiredDate { get; private set; }

        [NotNull]
        public FieldSettingDto Verified { get; private set; }

        [NotNull]
        public FieldSettingDto AttachedFile { get; private set; }

        [NotNull]
        public FieldSettingDto Approval { get; private set; }

        [NotNull]
        public FieldSettingDto Explanation { get; private set; }
    }
}
