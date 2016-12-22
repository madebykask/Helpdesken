using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.FileStore;
using ECT.Model.Abstract;


namespace ECT.Web.Controllers
{
    public class FilesController : BaseController
    {
        private readonly IContractRepository _contractRepository;
        private readonly IGlobalViewRepository _globalViewRepository;      

        public FilesController(IContractRepository contractRepository 
            , IGlobalViewRepository globalViewRepository
            , IUserRepository userRepository)
            : base(userRepository)
        {
            _contractRepository = contractRepository;
            _globalViewRepository = globalViewRepository;
        }

        public ActionResult GlobalViewUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult  GlobalViewUpload(int? chunk, string name)
        {
            var file = Request.Files[0];
            var uploadPath = UploadPath;
            chunk = chunk ?? 0;

            var success = true;
            var message = "Success";

            string[] FileContents = null;

            if (file.ContentLength > 0)
            {
                var FileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), FileName);
                file.SaveAs(path);

                FileContents = System.IO.File.ReadAllLines(path);
                string LDate = FileContents[0].Replace("\t", string.Empty).Replace(" ", string.Empty).Substring(0, 10);
                DateTime? CurUpdateDate = ConvertToDateTime(LDate, "dd.MM.yyyy");

                string Description = FileContents[2].Replace("\t", string.Empty).Substring(0, 18);

                List<string> FieldsName = FileContents[4].Split('\t').ToList(); // Fields Name List                     

                //if (FieldsName[FieldsName.Count - 1] == "\t")
                //{
                //    FieldsName.RemoveAt(FieldsName.Count - 1);
                //}

                int i = 0;
                int FieldCount = 0;
                string DistinationTable = string.Empty;
                switch (FieldsName.Count)
                {
                    case 32:
                        DistinationTable = "tblQualityCheckReport1_a2";
                        FieldCount = 31;
                        break;
                    case 56:
                        DistinationTable = "tblQualityCheckReport1_b2";
                        FieldCount = 55;
                        break;
                    case 46:
                        DistinationTable = "tblQualityCheckReport2_2";
                        FieldCount = 45;
                        break;
                }
                // check exist data

                if (_globalViewRepository.CheckAndDelete_QualityCheckFile(DistinationTable, CurUpdateDate.Value) == 0)
                {   
                    message = "Your files are older than versions in the system. Please update your files and try again.";
                    success = false;                    
                }
                else
                {  
                    if (DistinationTable != string.Empty) // check file format 
                    {
                        if (_globalViewRepository.CheckAndDelete_QualityCheckFile(DistinationTable, CurUpdateDate.Value) == 1)
                        {
                            int ErrorCount = 0;
                            List<string> ErrorInfo = new List<string>();
                            foreach (var _Record in FileContents)
                            {
                                i++;
                                if (i >= 7) //Because Records start from Line 7
                                {
                                    string CurRec = _Record;
                                    string[] FieldLists = CurRec.Split('\t'); // split text by TAB character   

                                    if (FieldLists.Length == FieldCount + 1)
                                    {
                                        _globalViewRepository.SaveQualityCheckFile(FieldLists, DistinationTable, FieldCount);
                                    }
                                    else
                                        if (FieldLists.Length == FieldCount) // Make a simillar records
                                        {
                                            CurRec = _Record + '\t';
                                            FieldLists = CurRec.Split('\t'); // split text by TAB character   
                                            _globalViewRepository.SaveQualityCheckFile(FieldLists, DistinationTable, FieldCount);
                                        }
                                        else
                                            if (FieldLists.Length <= FieldCount - 1) // Make a simillar records
                                            {
                                                CurRec = _Record;
                                                for (int k = 1; k <= FieldCount - FieldLists.Length + 1; k++)
                                                    CurRec = CurRec + '\t' + " ";


                                                FieldLists = CurRec.Split('\t'); // split text by TAB character   
                                                _globalViewRepository.SaveQualityCheckFile(FieldLists, DistinationTable, FieldCount);
                                            }
                                            else
                                            {
                                                ErrorInfo.Add(CurRec);
                                                ErrorCount += 1;
                                            }
                                }
                            }
                            _globalViewRepository.UpdateReportDate(DistinationTable, LDate);


                            switch (DistinationTable)
                            {
                                case "tblQualityCheckReport1_a2":
                                    ViewBag.tblQualityCheckReport1_a2 = ErrorCount;
                                    ViewBag.tblQualityCheckReport1_a2List = ErrorInfo;
                                    break;
                                case "tblQualityCheckReport1_b2":
                                    ViewBag.tblQualityCheckReport1_b2 = ErrorCount;
                                    ViewBag.tblQualityCheckReport1_b2List = ErrorInfo;
                                    break;
                                case "tblQualityCheckReport2_2":
                                    ViewBag.tblQualityCheckReport2_2 = ErrorCount;
                                    ViewBag.tblQualityCheckReport2_2List = ErrorInfo;
                                    break;
                            }
                        }

                    } // check file format 
                } // end else 
            } // file.ContentLength > 0            

            return Json(new { success = success, data = message });
        }
   
        [HttpPost]
        public ActionResult Upload(int? chunk, string name)
        {
            var fileUpload = Request.Files[0];
            var uploadPath = UploadPath;
            chunk = chunk ?? 0;

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

        public FileResult Index(string name, string extension, string caseNumber)
        {
            var globalSettings = _contractRepository.GetGlobalSettings();
            var path = globalSettings != null ? globalSettings.AttachedFileFolder : "";

           

            path = Path.Combine(path, caseNumber, name + extension);

            if(!FileStore.Exists(path))
                throw new HttpException(404, "file not found");

            var mimetype = FileStore.GetMimeTypeSupport(path);
            return File(path, mimetype);
        }

        [HttpPost]
        public void Delete(int id, string caseNumber)
        {
            var caseFile = _contractRepository.GetCaseFile(id);
            if(caseFile == null) return;             

            var globalSettings = _contractRepository.GetGlobalSettings();
            var path = globalSettings != null ? globalSettings.AttachedFileFolder : "";

            path = Path.Combine(path, caseNumber, caseFile.FileName);

            try
            {
                _contractRepository.DeleteCaseFile(id);
            }
            finally
            {
               FileStore.RemoveFile(path);
            }
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

        // TODO: Move out of controller --> better place
        public DateTime? ConvertToDateTime(string value, string format)
        {
            DateTime dt;
            if (DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out dt))
                return dt;
            return null;
        }
    }
}
