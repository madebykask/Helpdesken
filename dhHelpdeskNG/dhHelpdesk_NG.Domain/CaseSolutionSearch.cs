
using System.Collections.Generic;

namespace DH.Helpdesk.Domain
{
    public interface ICaseSolutionSearch : ISearch
    {
        int CustomerId { get; set; }
        string SearchCss { get; set; }
        List<int> CategoryIds { get; set; }

        List<string> SubStatusIds { get; set; }

        List<string> WgroupIds { get; set; }

        List<string> PriorityIds { get; set; }

        List<string> StatusIds { get; set; }

        List<string> ProductAreaIds { get; set; }

        List<string> UserWGroupIds { get; set; }

        List<string> TemplateProductAreaIds { get; set; }

        List<string> ApplicationIds { get; set; }
    }

    public class CaseSolutionSearch : Search, ICaseSolutionSearch
    {
        public int CustomerId { get; set; }
        public string SearchCss { get; set; }
        public List<int> CategoryIds { get; set; }

        public List<string> SubStatusIds { get; set; }

        public List<string> WgroupIds { get; set; }

        public List<string> PriorityIds { get; set; }

        public List<string> StatusIds { get; set; }

        public List<string> ProductAreaIds { get; set; }

        public List<string> UserWGroupIds { get; set; }

        public List<string> TemplateProductAreaIds { get; set; }

        public List<string> ApplicationIds { get; set; }
    }
}
