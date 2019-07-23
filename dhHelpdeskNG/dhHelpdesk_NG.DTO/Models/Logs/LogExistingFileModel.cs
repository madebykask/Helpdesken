
namespace DH.Helpdesk.BusinessData.Models.Logs
{
    public class LogExistingFileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsExistCaseFile { get; set; }
        public bool IsExistLogFile { get; set; }
        public int? LogId { get; set; }
        public int CaseId { get; set; }
    }
}
