namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettingsModel
    {
        public OrganizationFieldsSettingsModel(
            FieldSettingModel departmentFieldSettingModel,
            FieldSettingModel domainFieldSettingModel,
            FieldSettingModel unitFieldSettingModel)
        {
            this.DepartmentFieldSettingModel = departmentFieldSettingModel;
            this.DomainFieldSettingModel = domainFieldSettingModel;
            this.UnitFieldSettingModel = unitFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel DepartmentFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel DomainFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel UnitFieldSettingModel { get; set; }
    }
}