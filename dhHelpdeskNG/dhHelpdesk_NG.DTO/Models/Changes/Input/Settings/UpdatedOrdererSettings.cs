namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedOrdererSettings
    {
        public UpdatedOrdererSettings(
            UpdatedFieldSetting id,
            UpdatedFieldSetting name,
            UpdatedFieldSetting phone,
            UpdatedFieldSetting cellPhone,
            UpdatedFieldSetting email,
            UpdatedFieldSetting department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        [NotNull]
        public UpdatedFieldSetting Id { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Name { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Phone { get; private set; }

        [NotNull]
        public UpdatedFieldSetting CellPhone { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Email { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Department { get; private set; }
    }
}
