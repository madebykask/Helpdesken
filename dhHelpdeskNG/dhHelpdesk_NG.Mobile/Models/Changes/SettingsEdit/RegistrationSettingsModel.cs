namespace DH.Helpdesk.Mobile.Models.Changes.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public sealed class RegistrationSettingsModel
    {
        public RegistrationSettingsModel()
        {
        }

        public RegistrationSettingsModel(
            FieldSettingModel name,
            FieldSettingModel phone,
            FieldSettingModel email,
            FieldSettingModel company,
            FieldSettingModel owner,
            FieldSettingModel affectedProcesses,
            FieldSettingModel affectedDepartments,
            TextFieldSettingModel description,
            TextFieldSettingModel businessBenefits,
            TextFieldSettingModel consequence,
            FieldSettingModel impact,
            FieldSettingModel desiredDate,
            FieldSettingModel verified,
            FieldSettingModel attachedFiles,
            FieldSettingModel approval,
            FieldSettingModel rejectExplanation)
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
        [LocalizedDisplay("Name")]
        public FieldSettingModel Name { get; set; }

        [NotNull]
        [LocalizedDisplay("Phone")]
        public FieldSettingModel Phone { get; set; }

        [NotNull]
        [LocalizedDisplay("E-mail")]
        public FieldSettingModel Email { get; set; }

        [NotNull]
        [LocalizedDisplay("Company")]
        public FieldSettingModel Company { get; set; }

        [NotNull]
        [LocalizedDisplay("Owner")]
        public FieldSettingModel Owner { get; set; }

        [NotNull]
        [LocalizedDisplay("Affected Processes")]
        public FieldSettingModel AffectedProcesses { get; set; }

        [NotNull]
        [LocalizedDisplay("Affected Departments")]
        public FieldSettingModel AffectedDepartments { get; set; }

        [NotNull]
        [LocalizedDisplay("Description")]
        public TextFieldSettingModel Description { get; set; }

        [NotNull]
        [LocalizedDisplay("Business Benefits")]
        public TextFieldSettingModel BusinessBenefits { get; set; }

        [NotNull]
        [LocalizedDisplay("Consequence")]
        public TextFieldSettingModel Consequence { get; set; }

        [NotNull]
        [LocalizedDisplay("Impact")]
        public FieldSettingModel Impact { get; set; }

        [NotNull]
        [LocalizedDisplay("Desired date")]
        public FieldSettingModel DesiredDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Verified")]
        public FieldSettingModel Verified { get; set; }

        [NotNull]
        [LocalizedDisplay("Attached Files")]
        public FieldSettingModel AttachedFiles { get; set; }

        [NotNull]
        [LocalizedDisplay("Approval")]
        public FieldSettingModel Approval { get; set; }

        [NotNull]
        [LocalizedDisplay("Reject Explanation")]
        public FieldSettingModel RejectExplanation { get; set; }
    }
}