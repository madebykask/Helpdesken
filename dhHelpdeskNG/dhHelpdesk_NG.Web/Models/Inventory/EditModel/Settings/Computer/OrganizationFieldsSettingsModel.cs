namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

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
        [LocalizedDisplay("Department")]
        public FieldSettingModel DepartmentFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Domain")]
        public FieldSettingModel DomainFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Unit")]
        public FieldSettingModel UnitFieldSettingModel { get; set; }
    }
}