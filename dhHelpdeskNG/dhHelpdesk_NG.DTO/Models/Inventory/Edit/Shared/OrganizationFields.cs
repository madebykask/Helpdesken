namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared
{
    public class OrganizationFields
    {
        public OrganizationFields(int? domain, int? unit)
        {
            this.Domain = domain;
            this.Unit = unit;
        }

        public int? Domain { get; set; }

        public int? Unit { get; set; }
    }
}