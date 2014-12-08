namespace DH.Helpdesk.BusinessData.Models.Accounts.Read
{
    public class OtherForRead : Other
    {
        public OtherForRead(decimal caseNumber, string info, string fileName, byte[] content)
            : base(caseNumber, info, fileName)
        {
            this.Content = content;
        }

        public byte[] Content { get; set; }
    }
}