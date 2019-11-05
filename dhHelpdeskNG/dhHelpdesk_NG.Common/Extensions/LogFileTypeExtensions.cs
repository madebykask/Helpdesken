using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Logger;

namespace DH.Helpdesk.Common.Extensions
{
    public static class LogFileTypeExtensions
    {
        public static string GetFolderPrefix(this LogFileType type)
        {
            return type == LogFileType.External ? Enums.ModuleName.Log : Enums.ModuleName.LogInternal;
        }
    }
}
