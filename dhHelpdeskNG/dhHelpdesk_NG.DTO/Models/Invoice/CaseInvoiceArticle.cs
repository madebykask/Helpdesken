namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System.Web.Script.Serialization;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseInvoiceArticle
    {
        public CaseInvoiceArticle()
        {
        }

        public CaseInvoiceArticle(
                int id, 
                int caseId, 
                CaseOverview @case, 
                int? number,
                string name, 
                int? amount,
                int? unitId, 
                InvoiceArticleUnit unit, 
                decimal? ppu, 
                short position,
                bool isInvoiced)
        {
            this.Position = position;
            this.Amount = amount;
            this.Number = number;
            this.IsInvoiced = isInvoiced;
            this.Ppu = ppu;
            this.Unit = unit;
            this.UnitId = unitId;
            this.Name = name;
            this.Case = @case;
            this.CaseId = caseId;
            this.Id = id;
        }

        public int Id { get; private set; }

        public int CaseId { get; private set; }

        [ScriptIgnore]
        public CaseOverview Case { get; private set; }

        public int? Number { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public int? Amount { get; private set; }

        public int? UnitId { get; private set; }

        public InvoiceArticleUnit Unit { get; private set; }

        public decimal? Ppu { get; private set; }

        public short Position { get; private set; }

        public bool IsInvoiced { get; private set; }

        public bool IsNew()
        {
            return this.Id <= 0;
        }
    }
}