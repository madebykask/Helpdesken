using System;
using DH.Helpdesk.Common.Enums.Logs;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseLogFileDto : CaseFileDto
    {
        public LogFileType LogType { get; set; }
        public LogFileType? ParentLogType { get; set; }

        public CaseLogFileDto(
            string basePath,
            string filename,
            int referenceId,
            bool isCaseFile)
            : base(basePath, filename, referenceId, isCaseFile)
        {
        }

        public CaseLogFileDto(
            byte[] content,
            string basePath,
            string filename,
            DateTime createdDate,
            int referenceId,
            int? userId,
            LogFileType logType,
            LogFileType? parentLogType)
            : base(content, basePath, filename, createdDate, referenceId, userId)
        {
            LogType = logType;
            ParentLogType = logType;
        }
    }
}