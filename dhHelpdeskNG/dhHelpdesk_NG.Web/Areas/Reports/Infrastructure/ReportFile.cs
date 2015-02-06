namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure
{
    public class ReportFile
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