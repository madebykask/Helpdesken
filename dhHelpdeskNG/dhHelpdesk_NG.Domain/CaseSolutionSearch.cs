
using System.Collections.Generic;

namespace DH.Helpdesk.Domain
{
    public interface ICaseSolutionSearch : ISearch
    {
        int CustomerId { get; set; }
        string SearchCss { get; set; }
        List<int> CategoryIds { get; set; }

        List<int> SubStatusIds { get; set; }
    }

    public class CaseSolutionSearch : Search, ICaseSolutionSearch
    {
        public int CustomerId { get; set; }
        public string SearchCss { get; set; }
        public List<int> CategoryIds { get; set; }

        public List<int> SubStatusIds { get; set; }
    }
}
