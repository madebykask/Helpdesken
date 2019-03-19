using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using DH.Helpdesk.BusinessData.Models.Case.CaseLogs;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Models.Case.Logs;
using DH.Helpdesk.Services.BusinessLogic.Settings;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Common.Tools.Files;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.ActionResults;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using DH.Helpdesk.WebApi.Infrastructure.Filters;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/case")]
    public class CaseLogsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly ILogService _caselLogService;
        private readonly IMapper _mapper;
        private readonly ISettingsLogic _settingsLogic;
        private readonly ILogFileService _logFileService;
        private readonly ICaseFileService _caseFileService;
        private readonly ITemporaryFilesCache _userTemporaryFilesStorage;


        public CaseLogsController(
            IUserService userService, 
            ILogService caselLogService, 
            ILogFileService logFileService,
            ICaseFileService caseFileService,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            IMapper mapper, 
            ISettingsLogic settingsLogic)
        {
            _caseFileService = caseFileService;
            _logFileService = logFileService;
            _userService = userService;
            _caselLogService = caselLogService;
            _mapper = mapper;
            _settingsLogic = settingsLogic;
            _userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);
        }

        [HttpGet]
        [Route("{caseId:int}/logs")]
        [CheckUserCasePermissions(CaseIdParamName = "caseId", CheckBody = true)]
        public async Task<IHttpActionResult> Get([FromUri]int caseId, [FromUri]int cid)
        {
            var currentUser = _userService.GetUser(UserId);
            var includeInternalLogs = currentUser.CaseInternalLogPermission.ToBool();

            var logEntities = await _caselLogService.GetLogsByCaseIdAsync(caseId, includeInternalLogs).ConfigureAwait(false);
            
            var model = MapLogsToModel(logEntities);

            return Ok(model);
        }

        private IList<CaseLogOutputModel> MapLogsToModel(IList<CaseLogData> logs)
        {
            var items = new List<CaseLogOutputModel>();
            foreach (var log in logs)
            {
                //create two external and internal items out of one if has both properties
                if (!string.IsNullOrEmpty(log.ExternalText) && !string.IsNullOrEmpty(log.InternalText))
                {
                    var internalText = log.InternalText;

                    //create external log item
                    log.InternalText = null;
                    var itemModel = _mapper.Map<CaseLogOutputModel>(log);
                    items.Add(itemModel);

                    //create internal
                    log.ExternalText = null;
                    log.InternalText = internalText;
                    itemModel = _mapper.Map<CaseLogOutputModel>(log);
                    items.Add(itemModel);
                }
                else
                {
                    var itemModel = _mapper.Map<CaseLogOutputModel>(log);
                    items.Add(itemModel);
                }

            }

            return items;
        }

        //ex: /api/Case/123/LogFile/1203?cid=1
        [HttpGet]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        [Route("{caseId:int}/logfile/{fileId:int}")] 
        public Task<IHttpActionResult> Get([FromUri]int caseId, [FromUri]int fileId, [FromUri]int cid, bool? inline = false)
        {
            byte[] content = null;
            var basePath = _settingsLogic.GetFilePath(cid);
            var fileInfo = _logFileService.GetFileDetails(fileId);
            var isCaseFile = fileInfo.IsCaseFile ?? false;

            if (isCaseFile)
            {
                content = _caseFileService.GetFileContentByIdAndFileName(caseId, basePath, fileInfo.FileName);
            }
            else
            {
                var logFile = _logFileService.GetFileContentById(fileId, basePath);
                content = logFile.Content;
            }

            if (content == null)
                SendResponse($"The case file '{fileInfo.FileName}' was not found", HttpStatusCode.NotFound);

            IHttpActionResult res = new FileResult(fileInfo.FileName, content, Request, inline ?? false);
            return Task.FromResult(res);
        }

        [HttpPost]
        [Route("{caseId}/logfile/")]
        [CheckUserCasePermissions(CaseIdParamName = "caseId")]
        public async Task<IHttpActionResult> UploadLogFile([FromUri]string caseId, [FromUri]int cid)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var now = DateTime.Now;

            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            var stream = filesReadToProvider.Contents.FirstOrDefault();
            if (stream != null)
            {
                var fileBytes = await stream.ReadAsByteArrayAsync();
                var fileName = stream.Headers.ContentDisposition.FileName.Unquote().Trim();

                //fix name
                if (_userTemporaryFilesStorage.FileExists(fileName, caseId, ModuleName.Log))
                {
                    fileName = $"{now}-{fileName}"; // handle on the client file name change
                }

                _userTemporaryFilesStorage.AddFile(fileBytes, fileName, caseId, ModuleName.Log);
                return Ok(fileName);
            }

            return BadRequest("Failed to upload a file");
        }

        [HttpDelete]
        [Route("{caseKey}/templogfile")]
        [SkipCustomerAuthorization] //skip check for new case
        public IHttpActionResult DeleteTempLogFile([FromUri]string caseKey, [FromUri]string fileName)
        {
            //todo: make async
            //todo: check if UriDecode is required for fileName
            var fileNameSafe = (fileName ?? string.Empty).Trim();
            if (!string.IsNullOrEmpty(fileNameSafe))
            {
                _userTemporaryFilesStorage.DeleteFile(fileNameSafe, caseKey, ModuleName.Log);
            }
            return Ok(true);
        }
    }
}