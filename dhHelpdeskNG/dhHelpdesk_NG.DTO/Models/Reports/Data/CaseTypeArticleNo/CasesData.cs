namespace DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo
{
    public sealed class CasesData
    {
        public CasesData(int number, int productAreaId, int caseTypeId)
        {
            this.CaseTypeId = caseTypeId;
            this.ProductAreaId = productAreaId;
            this.Number = number;
        }

        public int ProductAreaId { get; private set; }

        public int CaseTypeId { get; private set; }

        public int Number { get; private set; }
    }
}