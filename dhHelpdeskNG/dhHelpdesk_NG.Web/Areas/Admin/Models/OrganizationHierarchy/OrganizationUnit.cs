namespace DH.Helpdesk.Web.Areas.Admin.Models.OrganizationHierarchy
{
    public class OrganizationUnit
    {
        public int id { get; set; }

        public string name { get; set; }

        public int? departmentId { get; set; }
    }
}