namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class OrganizationFieldsSettingsModel
    {
        public OrganizationFieldsSettingsModel()
        {
        }

        public OrganizationFieldsSettingsModel(FieldSettingModel departmentFieldSettingModel, FieldSettingModel unitFieldSettingModel)
        {
            this.DepartmentFieldSettingModel = departmentFieldSettingModel;
            this.UnitFieldSettingModel = unitFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Department")]
        public FieldSettingModel DepartmentFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Unit")]
        public FieldSettingModel UnitFieldSettingModel { get; set; }
    }
}