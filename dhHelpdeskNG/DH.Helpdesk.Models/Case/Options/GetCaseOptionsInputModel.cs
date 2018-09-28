using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Models.Case.Options
{
    public class GetCaseOptionsInputModel
    {
        public int? RegionId { get; set; }
        public int? DepartmentId { get; set; }
        public int? IsAboutRegionId { get; set; }
        public int? IsAboutDepartmentId { get; set; }
        public int? CaseResponsibleUserId { get; set; }
    }
}
