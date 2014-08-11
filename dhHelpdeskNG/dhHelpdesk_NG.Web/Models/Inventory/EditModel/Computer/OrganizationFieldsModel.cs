namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsModel
    {
        public OrganizationFieldsModel()
        {
        }

        public OrganizationFieldsModel(int? deparmentId, int? domainId, int? unitId)
        {
            this.DepartmentId = deparmentId;
            this.DomainId = domainId;
            this.UnitId = unitId;
        }

        [IsId]
        public int? DepartmentId { get; set; }

        [IsId]
        public int? DomainId { get; set; }

        [IsId]
        public int? UnitId { get; set; }
    }
}