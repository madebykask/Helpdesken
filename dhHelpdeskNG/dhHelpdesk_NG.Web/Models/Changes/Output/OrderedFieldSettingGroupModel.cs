namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class OrderedFieldSettingGroupModel
    {
        public OrderedFieldSettingGroupModel(
            FieldSettingModel id,
            FieldSettingModel name,
            FieldSettingModel phone,
            FieldSettingModel cellPhone,
            FieldSettingModel email,
            FieldSettingModel department)
        {
            ArgumentsValidator.NotNull(id, "id");
            ArgumentsValidator.NotNull(name, "name");
            ArgumentsValidator.NotNull(phone, "phone");
            ArgumentsValidator.NotNull(cellPhone, "cellPhone");
            ArgumentsValidator.NotNull(email, "email");
            ArgumentsValidator.NotNull(department, "department");

            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        public FieldSettingModel Id { get; private set; }

        public FieldSettingModel Name { get; private set; }

        public FieldSettingModel Phone { get; private set; }

        public FieldSettingModel CellPhone { get; private set; }

        public FieldSettingModel Email { get; private set; }

        public FieldSettingModel Department { get; private set; }
    }
}
