namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    public sealed class OtherHistoryFields
    {
        public OtherHistoryFields(
                string fileName, 
                decimal? caseNumber, 
                string info, 
                string status)
        {
            this.Status = status;
            this.Info = info;
            this.CaseNumber = caseNumber;
            this.FileName = fileName;
        }

        public string FileName { get; private set; }
        
        public decimal? CaseNumber { get; private set; }
        
        public string Info { get; private set; }
        
        public string Status { get; private set; } 
    }
}