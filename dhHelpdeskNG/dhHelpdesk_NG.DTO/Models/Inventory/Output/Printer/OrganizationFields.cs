namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer
{
    public class OrganizationFields
    {
        public OrganizationFields(string departmentName, string unitName)
        {
            this.DepartmentName = departmentName;
            this.UnitName = unitName;
        }

        public string DepartmentName { get; set; }

        public string UnitName { get; set; }
    }
}