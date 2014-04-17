namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFields
    {
        public OrganizationFields(int? departmentId, string unitId)
        {
            this.DepartmentId = departmentId;
            this.UnitId = unitId;
        }

        [IsId]
        public int? DepartmentId { get; set; }

        public string UnitId { get; set; }
    }
}