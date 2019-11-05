using System.Linq;
using DH.Helpdesk.Common.Enums.Logs;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System.Collections.Generic;

    public class FilesModel
    {
        public FilesModel(string id, List<LogFileModel> files, bool virtualDirectory)
        {
            Id = id;
            Files = files?.ToList() ?? new List<LogFileModel>();
            VirtualDirectory = virtualDirectory;
        }

        public FilesModel() {}

        public string Id { get; set; }
        public List<LogFileModel> Files { get; set; }
        public bool VirtualDirectory { get; set; }
    }
    
    public class LogFileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsExistCaseFile { get; set; }
        public bool IsExistLogFile { get; set; }
        public bool IsExternal { get; set; }
        //CaseId for case files, LogId for log files
        public int? ObjId { get; set; }
        public LogFileType LogType { get; set; }
        public LogFileType? ParentLogType { get; set; }
    }
}
