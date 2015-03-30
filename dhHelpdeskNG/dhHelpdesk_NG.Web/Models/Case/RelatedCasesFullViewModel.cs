namespace DH.Helpdesk.Web.Models.Case
{
    public sealed class RelatedCasesFullViewModel
    {
        public RelatedCasesFullViewModel(
            CaseSearchResultModel searchResult, 
            string userId, 
            int caseId, 
            string sortBy, 
            bool sortByAsc)
        {
            this.SortByAsc = sortByAsc;
            this.SortBy = sortBy;
            this.CaseId = caseId;
            this.UserId = userId;
            this.SearchResult = searchResult;
        }

        public CaseSearchResultModel SearchResult { get; private set; }

        public string UserId { get; private set; }

        public int CaseId { get; private set; }

        public string SortBy { get; private set; }

        public bool SortByAsc { get; private set; }
    }
}