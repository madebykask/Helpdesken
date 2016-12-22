using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Controllers;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;
using System;
using DH.Helpdesk.EForm.FormLib.Pdfs;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using System.Collections.Generic;

namespace DH.Helpdesk.EForm.FormLib.Areas.SouthKorea.Controllers
{
    public class AbsencesController : FormLibBaseController
    {
        private readonly IContractRepository _contractRepository;

        public AbsencesController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
            _contractRepository = contractRepository;
        }

        public JsonResult Typeahead(string query, string node, string dependentAttribute, string dependentAttributeValue)
        {
            var model = new FormModel(mainXmlPath);
            var element = model.GetElement(node);

            if (element == null && element.Descendants("option").Any())
                return Json(new object { });

            var options = element.Descendants("option").Where(x => x.Attribute(dependentAttribute) != null
                && x.Attribute(dependentAttribute).Value == dependentAttributeValue
                || string.IsNullOrEmpty(dependentAttributeValue)).Select(x => x.Value).ToArray();

            return Json(new { options });
        }

        [HttpPost]
        public JsonResult Print(string id, int caseId, string path, string absenceNr)
        {
            if (absenceNr == "1")
            {
                absenceNr = "";
            }

            var response = new AjaxResponse();

            if (string.IsNullOrEmpty(id))
                response.Exception = new ArgumentNullException("id").Message;

            var contract = _contractRepository.Get(caseId);

            if (contract == null)
            {
                response.Exception = new HttpException(404, "Page not found").Message;
                return Json(response);
            }

            var xp = (!string.IsNullOrEmpty(path) ? path + ".xml" : mainXmlPath);
            var model = new FormModel(xp)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };

            model.MultiNr = absenceNr;
            model.PopulateForm(_contractRepository.GetFormDictionary(caseId, model.FormGuid));

            var answer = FormLibSessions.User.FullName;
            var dictionary = new Dictionary<string, string> { { id, answer } };
            model.SetAnswer(id, answer);

            _contractRepository.SaveNew(model.FormGuid
                    , model.Contract.Id
                    , FormLibSessions.User.Id
                    , FormLibSessions.User.RegUserId
                    , model.Status
                    , FormLibUtils.GetSource()
                    , model.Language
                    , Request.UserHostAddress
                    , null
                    , dictionary
                    , FormLibSessions.User.FullName);

            _contractRepository.SaveFileViewLog(new FileViewLog
            {
                CaseId = model.Contract.Id,
                UserId = FormLibSessions.User.Id,
                CreatedDate = DateTime.UtcNow,
                FileSource = 3,
                FileName = id,
                FilePath = "..."
            });

            ////Get document data
            //model.DocumentData = _contractRepository.GetDocumentData(caseId);

            response.Result = ""; // Show who printed out the document. Shouldnt be used anymore FormLibSessions.User.FullName;

            return Json(response);
        }

        public ActionResult Contract(string id, int caseId, string path, string absenceNr)
        {
            if (absenceNr == "1")
            {
                absenceNr = "";
            }

            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            var contract = _contractRepository.Get(caseId);

            if (contract == null)
                throw new HttpException(404, "Page not found");

            path = (!string.IsNullOrEmpty(path) ? path + ".xml" : mainXmlPath);
            var model = new FormModel(path)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
            model.PopulateForm(_contractRepository.GetFormDictionary(caseId, model.FormGuid));

            var units = _contractRepository.GetUnits(model.Form.CustomerId, FormLibSessions.User.Id, null).ToList();
            if (units.Any())
            {
                model.PopulateFormElementWithOptions("BusinessUnit", units.ToDictionary());
            }

            model.Company = _contractRepository.GetCompany(model.Contract.Company.Key);

            var department = _contractRepository.GetDepartmentByKey(model.Contract.Unit.Key, model.Form.CustomerId);
            model.Department = department;

            //Get document data
            model.DocumentData = _contractRepository.GetDocumentData(caseId);

            var lang = "en";

//            var oldHidden = _contractRepository.GetFormDictionary(caseId, model.FormGuid).Where(x => x.Key.Contains("OLD_"))
//.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.Replace(Environment.NewLine, ""))).ToList();
//            TempData["oldValues"] = oldHidden;
        
            var page = GetPdfPage(model.Form.CustomerId, model.Contract.Unit.Key, lang, id, absenceNr, department, model);
            page.Contract = model.Contract;

            model.MultiNr = absenceNr;

            var workStream = new MemoryStream();

            //var document = new Document(PageSize.A4, 42, 45, 55, 50);
            var document = new Document(PageSize.A4, iTextSharp.text.Utilities.MillimetersToPoints(15), 43, iTextSharp.text.Utilities.MillimetersToPoints(83), iTextSharp.text.Utilities.MillimetersToPoints(35));
            var pdfWriter = PdfWriter.GetInstance(document, workStream);

            pdfWriter.PageEvent = page;
            pdfWriter.CloseStream = false;

            document.Open();

            var html = RenderRazorViewToString("~/FormLibContent/Areas/southkorea/Views/Absences/Pdfs/" + id + ".cshtml", model, false);
            using (TextReader htmlViewReader = new StringReader(html))
            {
                using (var htmlWorker = new HTMLWorkerExtended(document))
                {
                    string VerdanauniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "VERDANA.TTF");
                    iTextSharp.text.FontFactory.Register(VerdanauniTff);
                    htmlWorker.Open();
                    var styleSheet = new iTextSharp.text.html.simpleparser.StyleSheet();
                    styleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.FACE, "Verdana Unicode MS");
                    styleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, BaseFont.IDENTITY_H);
                    htmlWorker.SetStyleSheet(styleSheet);
                    htmlWorker.Parse(htmlViewReader);
                }
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return this.Pdf(byteInfo);
        }

        private SouthKoreaPdfPage GetPdfPage(int customerId, string searchKey, string language, string id, string absenceNr, Department department, FormModel model)
        {
            var pdfPage = new SouthKoreaPdfPage();

            pdfPage.model = model;
            pdfPage.department = department;
            pdfPage.Language = language;
            pdfPage.id = id;

            return pdfPage;
        }
    
    }
}