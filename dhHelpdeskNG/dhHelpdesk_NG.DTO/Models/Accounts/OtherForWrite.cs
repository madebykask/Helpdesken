namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    public class OtherForWrite : Other
    {
        public OtherForWrite(decimal caseNumber, string info, string fileName, byte[] content)
            : base(caseNumber, info, fileName)
        {
            this.Content = content;
        }

        public byte[] Content { get; private set; }
    }
}