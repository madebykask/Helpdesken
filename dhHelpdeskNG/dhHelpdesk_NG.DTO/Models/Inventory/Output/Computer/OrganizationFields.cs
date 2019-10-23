namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    public class OrganizationFields
    {
        public OrganizationFields(string regionName, string departmentName, string domainName, string unitName)
        {
            RegionName = regionName;
            this.DepartmentName = departmentName;
            this.DomainName = domainName;
            this.UnitName = unitName;
        }
        public string RegionName { get; set; }
        public string DepartmentName { get; set; }
        public string DomainName { get; set; }
        public string UnitName { get; set; }
    }
}