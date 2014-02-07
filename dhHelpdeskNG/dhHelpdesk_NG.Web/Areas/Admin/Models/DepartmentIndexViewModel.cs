namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class DepartmentIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Department> Departments { get; set; }
    }
}