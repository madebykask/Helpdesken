namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedOrdererFieldSettingGroupDto
    {
        public UpdatedOrdererFieldSettingGroupDto(
            UpdatedFieldSettingDto id,
            UpdatedFieldSettingDto name,
            UpdatedFieldSettingDto phone,
            UpdatedFieldSettingDto cellPhone,
            UpdatedFieldSettingDto email,
            UpdatedFieldSettingDto department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        [NotNull]
        public UpdatedFieldSettingDto Id { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Name { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Phone { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto CellPhone { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Email { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Department { get; private set; }
    }
}
