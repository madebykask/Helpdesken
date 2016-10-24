namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    using DH.Helpdesk.Domain;
    public sealed class OtherOverview
    {
        public OtherOverview(
                string fileName, 
                decimal? caseNumber, 
                Case caseInfo,
                string info, 
                string status)
        {
            this.Status = status;
            this.Info = info;
            this.CaseNumber = caseNumber;
            this.CaseInfo = caseInfo;
            this.FileName = fileName;
        }

        public string FileName { get; private set; }
        
        public decimal? CaseNumber { get; private set; }

        public Case CaseInfo { get; private set; }
        
        public string Info { get; private set; }
        
        public string Status { get; private set; }    
    }
}