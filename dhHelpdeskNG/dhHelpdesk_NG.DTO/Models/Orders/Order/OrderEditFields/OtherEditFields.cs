namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OtherEditFields
    {
        public OtherEditFields(
                string fileName, 
                decimal? caseNumber, 
                string info, 
                int? statusId)
        {
            this.StatusId = statusId;
            this.Info = info;
            this.CaseNumber = caseNumber;
            this.FileName = fileName;
        }

        [NotNull]
        public string FileName { get; private set; }
        
        public decimal? CaseNumber { get; private set; }
        
        public string Info { get; private set; }
        
        [IsId]
        public int? StatusId { get; private set; } 
    }
}