namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFields
    {
        public OrganizationFields(int? departmentId, int? unitId)
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