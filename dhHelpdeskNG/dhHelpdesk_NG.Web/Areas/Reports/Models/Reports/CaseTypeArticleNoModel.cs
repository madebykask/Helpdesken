namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports
{
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;

    public sealed class CaseTypeArticleNoModel
    {
        public CaseTypeArticleNoModel(
                CaseTypeArticleNoData data, 
                bool isShowCaseTypeDetails, 
                bool isShowPercents)
        {
            this.IsShowPercents = isShowPercents;
            this.IsShowCaseTypeDetails = isShowCaseTypeDetails;
            this.Data = data;
        }

        public CaseTypeArticleNoData Data { get; private set; }

        public bool IsShowCaseTypeDetails { get; private set; }

        public bool IsShowPercents { get; private set; }
    }
}