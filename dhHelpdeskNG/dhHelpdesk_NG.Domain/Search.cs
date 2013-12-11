
namespace dhHelpdesk_NG.Domain
{
    public interface ISearch
    {
        bool Ascending { get; set; }
        int MaxResults { get; set; }
        int Page { get; set; }
        int PageIndex { get; }
        string SortBy { get; set; }
    }

    public class Search : ISearch
    {
        public int MaxResults { get; set; }
        public int Page { get; set; }

        public int PageIndex
        {
            get { return Page - 1; }
        }

        public string SortBy { get; set; }
        public bool Ascending { get; set; }

        public Search()
        {
            MaxResults = 20;
            Page = 1;
        }
    }
}
