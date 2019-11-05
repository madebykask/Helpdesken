namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsModel
    {
        public OrganizationFieldsModel()
        {
        }

        public OrganizationFieldsModel(
            ConfigurableFieldModel<int?> regionId,
            ConfigurableFieldModel<int?> deparmentId,
            ConfigurableFieldModel<int?> domainId,
            ConfigurableFieldModel<int?> unitId)
        {
            RegionId = regionId;
            this.DepartmentId = deparmentId;
            this.DomainId = domainId;
            this.UnitId = unitId;
        }

        [NotNull]
        public ConfigurableFieldModel<int?> RegionId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> DepartmentId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> DomainId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> UnitId { get; set; }
    }
}