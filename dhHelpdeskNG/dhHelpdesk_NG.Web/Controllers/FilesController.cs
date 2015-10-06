using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Controllers
{
    public class FilesController : BaseController
    {
        private readonly ICaseService _caseService;
        private readonly IMasterDataService _masterDataService;
        private readonly ILogService _logService;

        public FilesController(ICaseService caseService,
            IMasterDataService masterDataService,
            ILogService logService)
            : base(masterDataService)
        {
            this._masterDataService = masterDataService;
            this._caseService = caseService;
            this._logService = logService;
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

            var c = this._caseService.GetCaseById(int.Parse(id));
            var basePath = string.Empty;
            if (c != null)
            {
                basePath = _masterDataService.GetVirtualDirectoryPath(c.Customer_Id);
                if (!basePath.EndsWith("/"))
                {
                    basePath = basePath + "/";
                }
            }

            link = absolute + basePath + c.CaseNumber + "/" + fileName;

            var UriLink = new Uri(link);
            return UriLink.ToString();
        }

        [HttpGet]
        public string LogFileLinkVD(string id, string fileName)
        {
            var link = "";

            var absolute = RequestExtension.GetAbsoluteUrl();
            var l = this._logService.GetLogById(int.Parse(id));
            var basePath = string.Empty;

            if (l != null)
            {
                var c = this._caseService.GetCaseById(l.CaseId);
                if (c != null)
                {
                    basePath = _masterDataService.GetVirtualDirectoryPath(c.Customer_Id);
                    if (!basePath.EndsWith("/"))
                    {
                        basePath = basePath + "/";
                    }
                }
                link = absolute + basePath + "L" + id + "/" + fileName;
            }
            var UriLink = new Uri(link);
            return UriLink.ToString();
        }
    }
}
