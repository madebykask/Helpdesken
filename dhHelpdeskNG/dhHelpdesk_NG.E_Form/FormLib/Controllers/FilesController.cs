using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ECT.Core;
using ECT.Core.FileStore;
using ECT.Core.Service;
using ECT.Model.Abstract;

namespace ECT.FormLib.Controllers
{
    public class FilesController : FormLibBaseController
    {
        private readonly IContractRepository _contractRepository;
        private readonly IGlobalViewRepository _globalViewRepository;
        private readonly IFileService _fileService;

        public FilesController(IContractRepository contractRepository
            , IGlobalViewRepository globalViewRepository
            , IUserRepository userRepository
                        , IFileService fileService)
            : base(userRepository)
        {
            _contractRepository = contractRepository;
            _globalViewRepository = globalViewRepository;
            _fileService = fileService;
        }

        [HttpPost]
        public ActionResult Upload(int? chunk, string name)
        {
            var fileUpload = Request.Files[0];
            chunk = chunk ?? 0;

            if(!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            DeleteOldFiles(uploadPath);

            var uploadSubPath = Path.Combine(uploadPath, Session.SessionID);
            var uploadedFilePath = Path.Combine(uploadSubPath, name);

            if(!Directory.Exists(uploadSubPath))
                Directory.CreateDirectory(uploadSubPath);

            using(var fs = new FileStream(uploadedFilePath, chunk == 0 ? FileMode.Create : FileMode.Append))
            {
                var buffer = new byte[fileUpload.InputStream.Length];
                fileUpload.InputStream.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, buffer.Length);
            }

            return Content("Success", "text/plain");
        }

        public JsonResult Files(string session, int caseId)
        {
            var files = "";
            return Json(files, JsonRequestBehavior.AllowGet);
        }

        public void Index(string name, string extension, string caseNumber)
        {
            var folder = _fileService.GetFolderPath(caseNumber);

            var path = Path.Combine(folder, caseNumber, name + extension);

            if(!FileStore.Exists(path))
                throw new HttpException(404, "file not found");

            var mimetype = FileStore.GetMimeTypeSupport(path);
            var bytes = System.IO.File.ReadAllBytes(path);

            //return File(path, mimetype);

            var encoding = Encoding.UTF8;
            var request = Request;
            var response = Response;
            var fileName = Path.GetFileName(path);

            response.Clear();
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", request.Browser.Browser.ToUpper() == "IE" ? HttpUtility.UrlEncode(fileName, encoding) : fileName));  
            response.ContentType = mimetype;
            response.Charset = encoding.WebName;
            response.HeaderEncoding = encoding;
            response.ContentEncoding = encoding;
            response.BinaryWrite(bytes);
            response.Flush();
            response.End();
        }

        [HttpPost]
        public void Delete(int id, string caseNumber)
        {
            var caseFile = _contractRepository.GetCaseFile(id);
            if(caseFile == null) return;

            var folder = _fileService.GetFolderPath(caseNumber);
            var path = Path.Combine(folder, caseNumber, caseFile.FileName);

            try
            {
                _contractRepository.DeleteCaseFile(id);
            }
            finally
            {
                FileStore.RemoveFile(path);
            }
        }

        // TODO: Move out of controller --> better place
        public DateTime? ConvertToDateTime(string value, string format)
        {
            DateTime dt;
            if(DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out dt))
                return dt;
            return null;
        }

        private void DeleteOldFiles(string path)
        {
            // Remove files older than 2 days....
            var deleteDirectories = new List<string>();
            foreach(var directory in Directory.GetDirectories(path))
            {
                var dt = Directory.GetLastWriteTime(directory);
                if(DateTime.Now.Subtract(dt).TotalDays > 2)
                    deleteDirectories.Add(directory);
            }

            foreach(var directory in deleteDirectories)
                Directory.Delete(directory, true);
        }
    }
}
