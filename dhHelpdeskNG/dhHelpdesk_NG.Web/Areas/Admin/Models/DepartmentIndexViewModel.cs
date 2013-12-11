using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class DepartmentIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Department> Departments { get; set; }
    }
}