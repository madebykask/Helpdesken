using System.Linq;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System.Collections.Generic;

    public class FilesModel
    {
        public FilesModel(string id, List<LogFileModel> files, bool virtualDirectory)
        {
            Id = id;
            Files = files.Select(x => new LogFileModel
            {
                Name = x.Name,
                Id = x.Id,
                IsExistLogFile = x.IsExistLogFile,
                IsExistCaseFile = x.IsExistCaseFile,
                ObjId = x.ObjId
            }).ToList();

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
    }
}
