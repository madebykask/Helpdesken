namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFields
    {
        public OrganizationFields(int? departmentId, int? domainId, int? unitId)
        {
            this.DepartmentId = departmentId;
            this.DomainId = domainId;
            this.UnitId = unitId;
        }

        [IsId]
        public int? DepartmentId { get; set; }

        [IsId]
        public int? DomainId { get; set; }

        [IsId]
        public int? UnitId { get; set; }

        public static OrganizationFields CreateDefault()
        {
            return new OrganizationFields(null, null, null);
        }
    }
}