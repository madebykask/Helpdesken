namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    public class Other
    {
        public Other(decimal caseNumber, string info, string fileName, byte[] content)
        {
            this.CaseNumber = caseNumber;
            this.Info = info;
            this.FileName = fileName;
            this.Content = content;
        }

        public decimal CaseNumber { get; private set; }

        public string Info { get; private set; }

        public string FileName { get; private set; }

        public byte[] Content { get; private set; }

        public static Other CreateDefault()
        {
            return new Other(0, null, null, null);
        }
    }
}