namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System.Web.Script.Serialization;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseInvoiceArticle
    {
        public CaseInvoiceArticle(int? articleId, InvoiceArticle article, decimal? ppu)
        {
            this.Ppu = ppu;
            this.Article = article;
            this.ArticleId = articleId;
        }

        public CaseInvoiceArticle(
                int id, 
                int caseId, 
                CaseOverview @case, 
                int? articleId, 
                InvoiceArticle article,
                string name, 
                int? amount,
                decimal? ppu,
                short position,
                bool isInvoiced)
        {
            this.Ppu = ppu;
            this.Article = article;
            this.ArticleId = articleId;
            this.Position = position;
            this.Amount = amount;
            this.IsInvoiced = isInvoiced;
            this.Name = name;
            this.Case = @case;
            this.CaseId = caseId;
            this.Id = id;
        }

        public int Id { get; private set; }

        public int CaseId { get; private set; }

        [ScriptIgnore]
        public CaseOverview Case { get; private set; }

        public int? ArticleId { get; private set; }

        public InvoiceArticle Article { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        public int? Amount { get; private set; }

        public decimal? Ppu { get; private set; }

        public short Position { get; private set; }

        public bool IsInvoiced { get; private set; }

        public bool IsNew()
        {
            return this.Id <= 0;
        }

        public void MakeInvoiced()
        {
            this.IsInvoiced = true;
        }
    }
}