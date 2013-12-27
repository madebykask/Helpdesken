namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class RegistrationFieldSettingGroupModel
    {
        public RegistrationFieldSettingGroupModel(
            FieldSettingModel name,
            FieldSettingModel phone,
            FieldSettingModel email,
            FieldSettingModel company,
            FieldSettingModel processAffected,
            FieldSettingModel departmentAffected,
            StringFieldSettingModel description,
            StringFieldSettingModel businessBenefits,
            StringFieldSettingModel consequence,
            FieldSettingModel impact,
            FieldSettingModel desiredDate,
            FieldSettingModel verified,
            FieldSettingModel attachedFile,
            FieldSettingModel approval,
            FieldSettingModel explanation)
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

            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Company = company;
            this.ProcessAffected = processAffected;
            this.DepartmentAffected = departmentAffected;
            this.Description = description;
            this.BusinessBenefits = businessBenefits;
            this.Consequence = consequence;
            this.Impact = impact;
            this.DesiredDate = desiredDate;
            this.Verified = verified;
            this.AttachedFile = attachedFile;
            this.Approval = approval;
            this.Explanation = explanation;
        }

        public FieldSettingModel Name { get; private set; }

        public FieldSettingModel Phone { get; private set; }

        public FieldSettingModel Email { get; private set; }

        public FieldSettingModel Company { get; private set; }

        public FieldSettingModel ProcessAffected { get; private set; }

        public FieldSettingModel DepartmentAffected { get; private set; }

        public StringFieldSettingModel Description { get; private set; }

        public StringFieldSettingModel BusinessBenefits { get; private set; }

        public StringFieldSettingModel Consequence { get; private set; }

        public FieldSettingModel Impact { get; private set; }

        public FieldSettingModel DesiredDate { get; private set; }

        public FieldSettingModel Verified { get; private set; }

        public FieldSettingModel AttachedFile { get; private set; }

        public FieldSettingModel Approval { get; private set; }

        public FieldSettingModel Explanation { get; private set; }
    }
}
