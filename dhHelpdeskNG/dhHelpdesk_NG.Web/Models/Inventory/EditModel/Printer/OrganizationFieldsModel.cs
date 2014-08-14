namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsModel
    {
        public OrganizationFieldsModel(ConfigurableFieldModel<int?> departmentId, ConfigurableFieldModel<string> unitId)
        {
            this.DepartmentId = departmentId;
            this.UnitId = unitId;
        }

        [NotNull]
        public ConfigurableFieldModel<int?> DepartmentId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> UnitId { get; set; }
    }
}