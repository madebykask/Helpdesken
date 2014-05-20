namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsModel
    {
        public OrganizationFieldsModel(int? departmentId, ConfigurableFieldModel<string> unitId)
        {
            this.DepartmentId = departmentId;
            this.UnitId = unitId;
        }

        [IsId]
        public int? DepartmentId { get; set; }

        public ConfigurableFieldModel<string> UnitId { get; set; }
    }
}