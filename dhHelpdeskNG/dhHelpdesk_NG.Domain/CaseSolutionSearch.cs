
using System.Collections.Generic;

namespace DH.Helpdesk.Domain
{
    public interface ICaseSolutionSearch : ISearch
    {
        int CustomerId { get; set; }
        string SearchCss { get; set; }
        List<int> CategoryIds { get; set; }

        List<int> SubStatusIds { get; set; }

        List<int> WgroupIds { get; set; }

        List<int> PriorityIds { get; set; }

        List<int> StatusIds { get; set; }

        List<int> ProductAreaIds { get; set; }

        List<int> UserWGroupIds { get; set; }

        List<int> TemplateProductAreaIds { get; set; }

        List<int> ApplicationIds { get; set; }
    }

    public class CaseSolutionSearch : Search, ICaseSolutionSearch
    {
        public int CustomerId { get; set; }
        public string SearchCss { get; set; }
        public List<int> CategoryIds { get; set; }

        public List<int> SubStatusIds { get; set; }

        public List<int> WgroupIds { get; set; }

        public List<int> PriorityIds { get; set; }

        public List<int> StatusIds { get; set; }

        public List<int> ProductAreaIds { get; set; }

        public List<int> UserWGroupIds { get; set; }

        public List<int> TemplateProductAreaIds { get; set; }

        public List<int> ApplicationIds { get; set; }
    }
}
