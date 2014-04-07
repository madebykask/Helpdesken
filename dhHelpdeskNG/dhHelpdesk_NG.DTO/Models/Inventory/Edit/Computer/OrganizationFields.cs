namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFields
    {
        public OrganizationFields(int? domainId, int? unitId)
        {
            this.DomainId = domainId;
            this.UnitId = unitId;
        }

        [IsId]
        public int? DomainId { get; set; }

        [IsId]
        public int? UnitId { get; set; }
    }
}