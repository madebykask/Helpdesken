namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    public class OrganizationFields
    {
        public OrganizationFields(string departmentName, string domainName, string unitName)
        {
            this.DepartmentName = departmentName;
            this.DomainName = domainName;
            this.UnitName = unitName;
        }

        public string DepartmentName { get; set; }

        public string DomainName { get; set; }

        public string UnitName { get; set; }
    }
}