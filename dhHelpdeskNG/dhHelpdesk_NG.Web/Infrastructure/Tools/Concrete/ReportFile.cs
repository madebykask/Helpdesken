namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    public sealed class ReportFile
    {
        public ReportFile(string objectId, string fileName)
        {
            this.FileName = fileName;
            this.ObjectId = objectId;
        }

        public string ObjectId { get; private set; }

        public string FileName { get; private set; }
    }
}