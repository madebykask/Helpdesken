namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared
{
    public class OrganizationFields
    {
        public OrganizationFields(string domainName, string unitName)
        {
            this.DomainName = domainName;
            this.UnitName = unitName;
        }

        public string DomainName { get; set; }

        public string UnitName { get; set; }
    }
}