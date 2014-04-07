namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsModel
    {
        public OrganizationFieldsModel(int? domainId, int? unitId)
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