namespace DH.Helpdesk.Web.Models.Case
{
    public sealed class RelatedCasesFullViewModel
    {
        public RelatedCasesFullViewModel(CaseSearchResultModel searchResult, string userId)
        {
            this.UserId = userId;
            this.SearchResult = searchResult;
        }

        public CaseSearchResultModel SearchResult { get; private set; }

        public string UserId { get; private set; }
    }
}