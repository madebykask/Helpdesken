﻿using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using System.Web.Mvc;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Dal.Enums;

namespace DH.Helpdesk.Web.Controllers
{
    public class FileHandlerController : BaseController
    {
        private readonly ICaseService _caseService;
        private readonly IMasterDataService _masterDataService;
        private readonly ILogService _logService;

        public FileHandlerController(
            ICaseService caseService,
            IMasterDataService masterDataService,
            ILogService logService)
            : base(masterDataService)
        {
            _masterDataService = masterDataService;
            _caseService = caseService;
            _logService = logService;
        }

        /// <summary>
        /// Use this if Virtual Directory is used
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public string CaseFileLinkVD(string id, string fileName)
        {
            var link = "";
            var absolute = RequestExtension.GetAbsoluteUrl();

            var c = _caseService.GetCaseBasic(int.Parse(id));
            var basePath = string.Empty;
            if (c != null)
            {
                basePath = _masterDataService.GetVirtualDirectoryPath(c.CustomerId);
                if (!basePath.EndsWith("/"))
                    basePath = basePath + "/";
            }

            link = absolute + basePath + c.CaseNumber + "/" + EncodeStr(fileName);
            return link;
        }

        [HttpGet]
        public string LogFileLinkVD(string id, string fileName, LogFileType logType)
        {
            var link = "";

            var absolute = RequestExtension.GetAbsoluteUrl();
            var l = _logService.GetLogById(int.Parse(id));
            var basePath = string.Empty;

            if (l != null)
            {
                var c = _caseService.GetCaseBasic(l.CaseId);
                if (c != null)
                {
                    basePath = _masterDataService.GetVirtualDirectoryPath(c.CustomerId);
                    if (!basePath.EndsWith("/"))
                    {
                        basePath = basePath + "/";
                    }
                }
                var logFolder = logType.GetFolderPrefix();
                link = absolute + basePath + logFolder + id + "/" + EncodeStr(fileName);
            }
           
            return link;
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
