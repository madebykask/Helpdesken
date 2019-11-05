namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class OrganizationFieldsSettingsModel
    {
        public OrganizationFieldsSettingsModel()
        {
        }

        public OrganizationFieldsSettingsModel(
            FieldSettingModel regionFieldSettingModel,
            FieldSettingModel departmentFieldSettingModel,
            FieldSettingModel domainFieldSettingModel,
            FieldSettingModel unitFieldSettingModel)
        {
            RegionFieldSettingModel = regionFieldSettingModel;
            this.DepartmentFieldSettingModel = departmentFieldSettingModel;
            this.DomainFieldSettingModel = domainFieldSettingModel;
            this.UnitFieldSettingModel = unitFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Område")]
        public FieldSettingModel RegionFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Department")]
        public FieldSettingModel DepartmentFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Domän")]
        public FieldSettingModel DomainFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Enhet")]
        public FieldSettingModel UnitFieldSettingModel { get; set; }
    }
}