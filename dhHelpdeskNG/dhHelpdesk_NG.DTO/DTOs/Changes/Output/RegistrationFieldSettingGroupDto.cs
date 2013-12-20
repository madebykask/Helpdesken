namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class RegistrationFieldSettingGroupDto
    {
        public RegistrationFieldSettingGroupDto(
            FieldSettingDto name,
            FieldSettingDto phone,
            FieldSettingDto email,
            FieldSettingDto company,
            FieldSettingDto processAffected,
            FieldSettingDto departmentAffected,
            FieldSettingDto description,
            FieldSettingDto businessBenefits,
            FieldSettingDto consequence,
            FieldSettingDto impact,
            FieldSettingDto desiredDate,
            FieldSettingDto verified,
            FieldSettingDto attachedFile,
            FieldSettingDto approval,
            FieldSettingDto explanation)
        {
            ArgumentsValidator.NotNull(name, "name");
            ArgumentsValidator.NotNull(phone, "phone");
            ArgumentsValidator.NotNull(email, "email");
            ArgumentsValidator.NotNull(company, "company");
            ArgumentsValidator.NotNull(processAffected, "processAffected");
            ArgumentsValidator.NotNull(departmentAffected, "departmentAffected");
            ArgumentsValidator.NotNull(description, "description");
            ArgumentsValidator.NotNull(businessBenefits, "businessBenefits");
            ArgumentsValidator.NotNull(consequence, "consequence");
            ArgumentsValidator.NotNull(impact, "impact");
            ArgumentsValidator.NotNull(desiredDate, "desiredDate");
            ArgumentsValidator.NotNull(verified, "verified");
            ArgumentsValidator.NotNull(attachedFile, "attachedFile");
            ArgumentsValidator.NotNull(approval, "approval");
            ArgumentsValidator.NotNull(explanation, "explanation");

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

        public FieldSettingDto Name { get; private set; }

        public FieldSettingDto Phone { get; private set; }

        public FieldSettingDto Email { get; private set; }

        public FieldSettingDto Company { get; private set; }

        public FieldSettingDto ProcessAffected { get; private set; }

        public FieldSettingDto DepartmentAffected { get; private set; }

        public FieldSettingDto Description { get; private set; }

        public FieldSettingDto BusinessBenefits { get; private set; }

        public FieldSettingDto Consequence { get; private set; }

        public FieldSettingDto Impact { get; private set; }

        public FieldSettingDto DesiredDate { get; private set; }

        public FieldSettingDto Verified { get; private set; }

        public FieldSettingDto AttachedFile { get; private set; }

        public FieldSettingDto Approval { get; private set; }

        public FieldSettingDto Explanation { get; private set; }
    }
}
