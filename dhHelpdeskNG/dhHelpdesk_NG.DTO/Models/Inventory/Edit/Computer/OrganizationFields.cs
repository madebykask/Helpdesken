namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFields
    {
        public OrganizationFields(int? regionId, int? departmentId, int? domainId, int? unitId)
        {
            RegionId = regionId;
            this.DepartmentId = departmentId;
            this.DomainId = domainId;
            this.UnitId = unitId;
        }

        [IsId]
        public int? RegionId { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        [IsId]
        public int? DomainId { get; set; }

        [IsId]
        public int? UnitId { get; set; }

        public static OrganizationFields CreateDefault()
        {
            return new OrganizationFields(null, null, null, null);
        }
    }
}