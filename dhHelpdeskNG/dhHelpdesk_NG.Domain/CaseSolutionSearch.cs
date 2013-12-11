
namespace dhHelpdesk_NG.Domain
{
    public interface ICaseSolutionSearch : ISearch
    {
        int CustomerId { get; set; }
        string SearchCss { get; set; }
    }

    public class CaseSolutionSearch : Search, ICaseSolutionSearch
    {
        public int CustomerId { get; set; }
        public string SearchCss { get; set; }
    }
}
