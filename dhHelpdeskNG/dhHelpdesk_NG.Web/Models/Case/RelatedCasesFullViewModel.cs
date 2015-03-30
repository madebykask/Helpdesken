namespace DH.Helpdesk.Web.Models.Case
{
    public sealed class RelatedCasesFullViewModel
    {
        public RelatedCasesFullViewModel(
            CaseSearchResultModel searchResult, 
            string userId, 
            int caseId)
        {
            this.CaseId = caseId;
            this.UserId = userId;
            this.SearchResult = searchResult;
        }

        public CaseSearchResultModel SearchResult { get; private set; }

        public string UserId { get; private set; }

        public int CaseId { get; private set; }
    }
}