namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsModel
    {
        public OrganizationFieldsModel(int? departmentId, int? unitId)
        {
            this.DepartmentId = departmentId;
            this.UnitId = unitId;
        }

        [IsId]
        public int? DepartmentId { get; set; }

        [IsId]
        public int? UnitId { get; set; }
    }
}