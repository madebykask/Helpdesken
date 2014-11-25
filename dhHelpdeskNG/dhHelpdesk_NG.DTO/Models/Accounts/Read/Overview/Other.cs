namespace DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview
{
    public class Other
    {
        public Other(decimal caseNumber, string info, string fileName)
        {
            this.CaseNumber = caseNumber;
            this.Info = info;
            this.FileName = fileName;
        }

        public decimal CaseNumber { get; private set; }

        public string Info { get; private set; }

        public string FileName { get; private set; }
    }
}