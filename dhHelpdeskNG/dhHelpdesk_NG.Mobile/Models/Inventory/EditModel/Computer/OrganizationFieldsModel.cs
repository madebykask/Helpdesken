namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsModel
    {
        public OrganizationFieldsModel()
        {
        }

        public OrganizationFieldsModel(
            ConfigurableFieldModel<int?> deparmentId,
            ConfigurableFieldModel<int?> domainId,
            ConfigurableFieldModel<int?> unitId)
        {
            this.DepartmentId = deparmentId;
            this.DomainId = domainId;
            this.UnitId = unitId;
        }

        [NotNull]
        public ConfigurableFieldModel<int?> DepartmentId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> DomainId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> UnitId { get; set; }
    }
}