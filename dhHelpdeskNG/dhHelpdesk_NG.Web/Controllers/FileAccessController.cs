using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums.FileViewLog;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Common.Extensions.ModuleName;

namespace DH.Helpdesk.Web.Controllers
{
    public class FileAccessController : BaseController
    {
        private readonly IFeatureToggleService _featureToggleService;
        private readonly IFileViewLogService _fileViewLogService;
        private readonly ICaseService _caseService;
        private readonly IMasterDataService _masterDataService;
        private readonly IFilesStorage _filesStorage;

        public FileAccessController(IMasterDataService masterDataService,
            IFeatureToggleService featureToggleService,
            IFileViewLogService fileViewLogService, ICaseService caseService,
            IFilesStorage filesStorage) :
            base(masterDataService)
        {
            _masterDataService = masterDataService;
            _featureToggleService = featureToggleService;
            _fileViewLogService = fileViewLogService;
            _caseService = caseService;
            _filesStorage = filesStorage;
        }

        [HttpPost]
        public ActionResult LogFileAccess(int caseId, string fileName, CaseFileType type, int? logId = null)
        {
            if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_LOG_VIEW_CASE_FILE))
            {
                var c = _caseService.GetCaseBasic(caseId);
                var filePath = "";
                if (c != null)
                {
                    var basePath = _masterDataService.GetFilePath(c.CustomerId);
                    var topic = type.ToModuleName();
                    filePath = _filesStorage.ComposeFilePath(topic, logId ?? decimal.ToInt32(c.CaseNumber), basePath, "");
                }
                var userId = SessionFacade.CurrentUser?.Id ?? 0;
                _fileViewLogService.Log(caseId, userId, fileName, filePath, FileViewLogFileSource.Helpdesk, FileViewLogOperation.View);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}