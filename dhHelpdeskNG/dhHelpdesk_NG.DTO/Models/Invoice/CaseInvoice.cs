namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System.Web.Script.Serialization;

    using DH.Helpdesk.BusinessData.Models.Case.Output;

    public sealed class CaseInvoice
    {
        public CaseInvoice(
                int id, 
                int caseId, 
                CaseOverview @case, 
                CaseInvoiceOrder[] orders)
        {
            this.Orders = orders;
            this.Case = @case;
            this.CaseId = caseId;
            this.Id = id;
        }

        public CaseInvoice(
                int id,
                int caseId,
                CaseInvoiceOrder[] orders) : 
                this(id, caseId, null, orders)
        {
        }

        public int Id { get; private set; }

        public int CaseId { get; private set; }

        [ScriptIgnore]
        public CaseOverview Case { get; private set; }

        public CaseInvoiceOrder[] Orders { get; private set; }

        public bool IsNew()
        {
            return this.Id <= 0;
        }
    }
}