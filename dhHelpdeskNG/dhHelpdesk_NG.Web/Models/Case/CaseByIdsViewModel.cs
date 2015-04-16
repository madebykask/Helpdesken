namespace DH.Helpdesk.Web.Models.Case
{
    public sealed class CaseByIdsViewModel
    {
        public CaseByIdsViewModel(
            CaseSearchResultModel searchResult, 
            string sortBy, 
            bool sortByAsc, 
            string caseIds)
        {
            this.CaseIds = caseIds;
            this.SortByAsc = sortByAsc;
            this.SortBy = sortBy;
            this.SearchResult = searchResult;
        }

        public CaseSearchResultModel SearchResult { get; private set; }

        public string SortBy { get; private set; }

        public bool SortByAsc { get; private set; }

        public string CaseIds { get; private set; }
    }
}