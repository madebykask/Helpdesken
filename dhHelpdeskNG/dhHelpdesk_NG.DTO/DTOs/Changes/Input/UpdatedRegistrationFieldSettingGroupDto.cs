namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class UpdatedRegistrationFieldSettingGroupDto
    {
        public UpdatedRegistrationFieldSettingGroupDto(
            UpdatedFieldSettingDto name,
            UpdatedFieldSettingDto phone,
            UpdatedFieldSettingDto email,
            UpdatedFieldSettingDto company,
            UpdatedFieldSettingDto processAffected,
            UpdatedFieldSettingDto departmentAffected,
            UpdatedStringFieldSettingDto description,
            UpdatedStringFieldSettingDto businessBenefits,
            UpdatedStringFieldSettingDto consequence,
            UpdatedFieldSettingDto impact,
            UpdatedFieldSettingDto desiredDate,
            UpdatedFieldSettingDto verified,
            UpdatedFieldSettingDto attachedFile,
            UpdatedFieldSettingDto approval,
            UpdatedFieldSettingDto explanation)
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

        public UpdatedFieldSettingDto Name { get; private set; }

        public UpdatedFieldSettingDto Phone { get; private set; }

        public UpdatedFieldSettingDto Email { get; private set; }

        public UpdatedFieldSettingDto Company { get; private set; }

        public UpdatedFieldSettingDto ProcessAffected { get; private set; }

        public UpdatedFieldSettingDto DepartmentAffected { get; private set; }

        public UpdatedStringFieldSettingDto Description { get; private set; }

        public UpdatedStringFieldSettingDto BusinessBenefits { get; private set; }

        public UpdatedStringFieldSettingDto Consequence { get; private set; }

        public UpdatedFieldSettingDto Impact { get; private set; }

        public UpdatedFieldSettingDto DesiredDate { get; private set; }

        public UpdatedFieldSettingDto Verified { get; private set; }

        public UpdatedFieldSettingDto AttachedFile { get; private set; }

        public UpdatedFieldSettingDto Approval { get; private set; }

        public UpdatedFieldSettingDto Explanation { get; private set; }
    }
}
