namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System.Web.Script.Serialization;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseInvoiceArticle
    {
        public CaseInvoiceArticle(
                int id, 
                int caseId, 
                CaseOverview @case, 
                string name, 
                int unitId, 
                InvoiceArticleUnit unit, 
                decimal ppu, 
                bool isInvoiced)
        {
            this.IsInvoiced = isInvoiced;
            this.Ppu = ppu;
            this.Unit = unit;
            this.UnitId = unitId;
            this.Name = name;
            this.Case = @case;
            this.CaseId = caseId;
            this.Id = id;
        }

        [IsId]
        public int Id { get; private set; }

        [IsId]
        public int CaseId { get; private set; }

        [NotNull]
        [ScriptIgnore]
        public CaseOverview Case { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [IsId]
        public int UnitId { get; private set; }

        [NotNull]
        public InvoiceArticleUnit Unit { get; private set; }

        public decimal Ppu { get; private set; }

        public bool IsInvoiced { get; private set; }
    }
}