
namespace DH.Helpdesk.Domain
{
    public interface ISearch
    {
        bool Ascending { get; set; }
        int MaxResults { get; set; }
        int Page { get; set; }
        int PageIndex { get; }
        string SortBy { get; set; }
        string IdsForLastSearch { get; set; }
    }

    public class Search : ISearch
    {
        public int MaxResults { get; set; }
        public int Page { get; set; }
        public string IdsForLastSearch { get; set; }

        public int PageIndex
        {
            get { return this.Page - 1; }
        }

        public string SortBy { get; set; }
        public bool Ascending { get; set; }

        public Search()
        {
            this.MaxResults = 20;
            this.Page = 1;
        }
    }
}
