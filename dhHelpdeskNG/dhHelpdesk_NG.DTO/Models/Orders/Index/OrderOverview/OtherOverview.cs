namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public sealed class OtherOverview
    {
        public OtherOverview(
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