using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Infrastructure.Extensions
{
    public static class LogFileExtensions
    {
        public static string GetFolderPrefix(this LogFile logFile)
        {
            var logType = logFile.ParentLogType ?? logFile.LogType;
            return logType.GetFolderPrefix();
        }
    }
}
