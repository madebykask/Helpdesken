using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ExtendedCase.Common.Enums;
using ExtendedCase.Logic.Services;
using ExtendedCase.Logic.Utils.Files;

namespace ExtendedCase.WebApi.Controllers
{
    [RoutePrefix("api/files")]
    public class FilesController : ApiController
    {
        private readonly IGlobalSettingsService _globalSettingService;
        private readonly ITemporaryFilesCache _userTemporaryFilesStorage;

        /// <summary>
        /// This file was copy pasted from DH.Helpdesk.WebApi - CaseFilesController. Because DH.Helpdesk.WebApi has authentication, but this one don't.
        /// When common auth is implemented, use DH.Helpdesk.WebApi controller instead
        /// </summary>
        /// <param name="globalSettingService"></param>
        public FilesController(IGlobalSettingsService globalSettingService)
        {
            _globalSettingService = globalSettingService;
            _userTemporaryFilesStorage = new TemporaryFilesCache(ModuleName.Cases);
        }


        [HttpPost]
        [Route("{caseKey:guid}/file")] // remember to update WebApiCorsPolicyProvider if url is changed
        public async Task<IHttpActionResult> UploadNewCaseFile([FromUri] Guid caseKey)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            var stream = filesReadToProvider.Contents.FirstOrDefault();
            if (stream != null)
            {
                var fileBytes = await stream.ReadAsByteArrayAsync();
                var fileName = stream.Headers.ContentDisposition.FileName.Trim().Trim('"').Trim();

                var extension = Path.GetExtension(fileName);

                if (!_globalSettingService.IsExtensionInWhitelist(extension))
                {
                    throw new ArgumentException($"File extension not valid for upload (not defined in whitelist): {extension}");
                }

                var counter = 1;
                var newFileName = fileName;
                while (_userTemporaryFilesStorage.FileExists(fileName, caseKey.ToString()))
                {
                    newFileName = $"{Path.GetFileNameWithoutExtension(fileName)} ({counter++}){Path.GetExtension(fileName)}";
                    fileName = newFileName;
                }

                _userTemporaryFilesStorage.AddFile(fileBytes, fileName, caseKey.ToString());
                return Ok(fileName);
            }

            return BadRequest("Failed to upload a file");
        }
    }
}
