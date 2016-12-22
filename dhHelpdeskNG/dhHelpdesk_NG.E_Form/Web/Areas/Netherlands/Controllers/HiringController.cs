using ECT.Core.Service;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using ECT.Web.Controllers;
using ECT.Web.Models;
using ECT.Web.Pdf;
using ECT.Web.Pdf.Netherlands;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECT.Web.Areas.Netherlands.Controllers
{
    public class HiringController : BaseController
    {
        private readonly IContractRepository _contractRepository;
        private readonly IFileService _fileService;

        public HiringController(IContractRepository contractRepository
            , IFileService fileService
            , IUserRepository userRepository)
            : base(userRepository)
        {
            _fileService = fileService;
            _contractRepository = contractRepository;
        }

        //public JsonResult CostCenter(string query, string node)
        //{
        //    var model = new FormModel(XmlPath);
        //    var element = model.GetElement(node);
        //    if(element == null && element.Descendants("option").Any())
        //        return Json(new object { });
        //    var option = element.Descendants("option").Where(x => x.Attribute("Department") != null
        //        && x.Attribute("Department").Value == query).FirstOrDefault();
        //    if(option == null)
        //        return Json(new object { });
        //    return Json(new { option.Value });
        //}

        public ActionResult New()
        {
            var model = new FormModel(XmlPath)
            {
                Contract = new Contract
                {
                    Initiator = SessionFacade.User.FirstName + " " + SessionFacade.User.Surname
                }
            };

            //var companies = _contractRepository.GetCompanies(model.CustomerId, SessionFacade.User.Id).ToList();
            //if(companies.Any())
            //    model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            //var units = _contractRepository.GetUnits(model.CustomerId, SessionFacade.User.Id, null).ToList();
            //if(units.Any())
            //    model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

            return View(model);
        }

        [HttpPost]
        public ActionResult New(FormCollection formCollection, string[] uploads)
        {
            var model = new FormModel(XmlPath)
            {
                Contract = new Contract
                {
                    Initiator = SessionFacade.User.FirstName + " " + SessionFacade.User.Surname
                }
            };

            //var companies = _contractRepository.GetCompanies(model.CustomerId, SessionFacade.User.Id).ToList();
            //if (companies.Any())
            //    model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            //var units = _contractRepository.GetUnits(model.CustomerId, SessionFacade.User.Id, null).ToList();
            //if (units.Any())
            //    model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

            var dictionary = formCollection.ToDictionary();
            model.TempSave(ref dictionary);

            if(model.IsValid(ref dictionary, 10))
            {
                var caseId = _contractRepository.Save(model.FormId
                    , 0
                    , SessionFacade.User.Id
                    , formCollection["FirstName"]
                    , formCollection["LastName"]
                    , formCollection["DateOfBirth"]
                    , dictionary);

                _fileService.SaveUploads(uploads, caseId, UploadPath, Session.SessionID);

                return RedirectToAction("edit", new { id = caseId });
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var contract = _contractRepository.Get(id);

            if(contract == null)
                throw new HttpException(404, "Page not found");

            var model = new FormModel(XmlPath)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };

            model.PopulateForm(_contractRepository.GetFormDictionary(id, model.FormId));

            //var companies = _contractRepository.GetCompanies(model.CustomerId, SessionFacade.User.Id).ToList();
            //if (companies.Any())
            //    model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            //var units = _contractRepository.GetUnits(model.CustomerId, SessionFacade.User.Id, null).ToList();
            //if (units.Any())
            //    model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

            //if(contract.Company != null)
            //    model.SetAnswer("Company", contract.Company.Key);
            //if(contract.Unit != null)
            //    model.SetAnswer("Unit", contract.Unit.Key);

            model.SetAnswer("LastName", contract.Surname);
            model.SetAnswer("FirstName", contract.FirstName);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection, string[] uploads)
        {
            var contract = _contractRepository.Get(id);

            if (contract == null)
                throw new HttpException(404, "Page not found");

            var model = new FormModel(XmlPath)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };

            var companies = _contractRepository.GetCompanies(model.CustomerId, SessionFacade.User.Id).ToList();
            if (companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId, SessionFacade.User.Id, null).ToList();
            if (units.Any())
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

            var dictionary = formCollection.ToDictionary();

            model.TempSave(ref dictionary);
            model.ActiveTab = formCollection["activeTab"];

            var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
            var actionState = formCollection["actionState"];
            model.ActiveStatus = actionState;

            if (actionStateChange && !string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
            {
                model.Status = int.Parse(actionState);
                _contractRepository.UpdateCaseStateSecondary(model.Contract.Id, model.Status, SessionFacade.User.FullName);
                model.Contract = _contractRepository.Get(id);
            }

            _contractRepository.Save(model.FormId, model.Contract.Id, dictionary);
            _fileService.SaveUploads(uploads, model.Contract.Id, UploadPath, Session.SessionID);

            if (!Request.IsAjaxRequest())
                return View(model);

            var view = string.Empty;

            if (model.GetErrorMessages().Any())
                view = RenderRazorViewToString("_Edit", model);

            return Json(new
            {
                View = view,
                CancelCase = SessionFacade.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
            });
        }

        [HttpPost]
        public JsonResult Print(string id, int caseId, string path)
        {
            var response = new AjaxResponse();

            if (string.IsNullOrEmpty(id))
                response.Exception = new ArgumentNullException("id").Message;

            var contract = _contractRepository.Get(caseId);

            if (contract == null)
            {
                response.Exception = new HttpException(404, "Page not found").Message;
                return Json(response);
            }

            var xp = (!string.IsNullOrEmpty(path) ? path + ".xml" : XmlPath);
            var model = new FormModel(xp)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };

            model.PopulateForm(_contractRepository.GetFormDictionary(caseId, model.FormId));

            var answer = SessionFacade.User.FirstName + " " + SessionFacade.User.Surname;
            var dictionary = new Dictionary<string, string> { { id, answer } };
            model.SetAnswer(id, answer);



            _contractRepository.Save(model.FormId, model.Contract.Id, dictionary);

            _contractRepository.SaveFileViewLog(new FileViewLog
            {
                CaseId = model.Contract.Id,
                UserId = SessionFacade.User.Id,
                CreatedDate = DateTime.UtcNow,
                FileSource = 3,
                FileName = id,
                FilePath = "..."
            });

            response.Result = SessionFacade.User.FirstName + " " + SessionFacade.User.Surname;

            return Json(response);
        }

        public ActionResult Contract(string id, int caseId, string path)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            var contract = _contractRepository.Get(caseId);

            if (contract == null)
                throw new HttpException(404, "Page not found");

            path = (!string.IsNullOrEmpty(path) ? path + ".xml" : XmlPath);
            var model = new FormModel(path)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };

            var units = _contractRepository.GetUnits(model.CustomerId, null, null).ToList();
            if (units.Any())
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

            model.PopulateForm(_contractRepository.GetFormDictionary(caseId, model.FormId));
            model.Company = _contractRepository.GetCompany(model.Contract.Company.Key);

            var lang = "en";
            if (id.IndexOf("Dutch") > 0)
                lang = "nl";

            var page = GetNewHirePdfPage(model.Contract.Company.Key, lang);
            page.Contract = model.Contract;
            var workStream = new MemoryStream();
            
            var document = new Document(PageSize.A4, 42, 45, 90, 60);        
            var pdfWriter = PdfWriter.GetInstance(document, workStream);

            pdfWriter.PageEvent = page;
            pdfWriter.CloseStream = false;

            document.Open();

            var html = RenderRazorViewToString("~/Areas/Netherlands/Views/Hiring/Pdfs/" + id + ".cshtml", model, false);
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

        private NetherlandsPdfPage GetNewHirePdfPage(string searchKey, string language)
        {
            var newHirePdfPage = new NetherlandsPdfPage();
            var company = _contractRepository.GetCompany(searchKey);
            //newHirePdfPage.CurrentCompany = company;
            newHirePdfPage.Language = language;

            return newHirePdfPage;
        }
    }
    
}
