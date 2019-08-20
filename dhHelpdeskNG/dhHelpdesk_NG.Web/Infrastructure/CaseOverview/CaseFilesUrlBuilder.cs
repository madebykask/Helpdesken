using System.Linq;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Dal.Enums;

namespace DH.Helpdesk.Web.Infrastructure.CaseOverview
{
    public class CaseFilesUrlBuilder
    {
        private readonly string _virtualDirPath; 
        private readonly string _absolutePath;

        public string VirtualDirPath => _virtualDirPath;
        public string AbsolutePath => _absolutePath;

        #region ctor()

        public CaseFilesUrlBuilder(string virtualDirDirPath, string absolutePath)
        {
            _absolutePath = absolutePath;
            _virtualDirPath = virtualDirDirPath;
        }

        #endregion

        public string BuildCaseFileLinkVD(int caseNumber, string fileName)
        {
            var fileUrl = BuildPath(_absolutePath, _virtualDirPath, caseNumber.ToString(), EncodeStr(fileName));
            return fileUrl;
        }

        public string BuildLogFileLinkVD(int logId, string fileName, LogFileType logType)
        {
            var logFolder = 
                logType == LogFileType.Internal ? $"{ModuleName.LogInternal}{logId}" : $"{ModuleName.Log}{logId}";

            var fileUrl = BuildPath(_absolutePath, _virtualDirPath, logFolder, EncodeStr(fileName));
            return fileUrl;
        }

        private string BuildPath(params string[] urlTokens)
        {
            var fixedUrlTokens = urlTokens.Select(u => u.Trim('/')).ToList();
            var urlPath = string.Join("/", fixedUrlTokens);
            return urlPath;
        }

        //todo: move to separate class and use Uri.EscapeDataString for IE, Edge.
        private string EncodeStr(string str)
        {
            str = str.Replace("%", "%25");
            str = str.Replace("@", "%40");
            str = str.Replace("#", "%23");
            str = str.Replace("¤", "%C2%A4");
            str = str.Replace("$", "%24");
            str = str.Replace("{", "%7B");
            str = str.Replace("}", "%7D");
            str = str.Replace("[", "%5B");
            str = str.Replace("]", "%5D");
            str = str.Replace(",", "%2C");
            str = str.Replace("'", "%27");
            str = str.Replace(";", "%3B");
            str = str.Replace("+", "%2B");

            return str;
        }
    }
}