namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettingsModel
    {
        public OrganizationFieldsSettingsModel(FieldSettingModel departmentFieldSettingModel, FieldSettingModel unitFieldSettingModel)
        {
            this.DepartmentFieldSettingModel = departmentFieldSettingModel;
            this.UnitFieldSettingModel = unitFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel DepartmentFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel UnitFieldSettingModel { get; set; }
    }
}