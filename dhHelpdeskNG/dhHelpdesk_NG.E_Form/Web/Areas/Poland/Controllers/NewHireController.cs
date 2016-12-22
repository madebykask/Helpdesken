using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Model.Abstract;
using Model.Entities;
using Web.Areas.Poland.Models;
using Web.Controllers;
using Web.Models;
using Web.Pdf.Poland;

namespace Web.Areas.Poland.Controllers
{
    public class NewHireController : BaseController
    {
        private readonly IContractRepository _contractRepository;

        public NewHireController(IContractRepository contractRepository
            , IUserRepository userRepository)
            : base(userRepository)
        {
            _contractRepository = contractRepository;
        }

        public ActionResult New()
        {
            var model = new NewHire(XmlPath) { Status = 00 };

            model.SetAnswer("CountryCode", "Poland");
            model.Contract = new Contract { Initiator = SessionFacade.User.FirstName + " " + SessionFacade.User.Surname };

            var companies = _contractRepository.GetCompanies(model.CustomerId).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId, 0).ToList();
            if(units.Any())
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

            SessionFacade.SetModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(FormCollection formCollection)
        {
            var model = SessionFacade.GetModel<NewHire>();

            if(model == null)
                return RedirectToAction("index", "dead");

            var dictionary = formCollection.ToDictionary();
            model.TempSave(ref dictionary);

            if(model.IsValid(ref dictionary, 10))
            {
                var caseId = _contractRepository.Save(model.FormId
                    , 0
                    , SessionFacade.User.Id
                    , int.Parse(formCollection["Company"])
                    , int.Parse(formCollection["Unit"])
                    , formCollection["EmployeeFirstName"]
                    , formCollection["EmployeeLastName"]
                    , formCollection["DateOfBirth"]
                    , dictionary);

                return RedirectToAction("edit", new { id = caseId });
            }

            SessionFacade.SetModel(model);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var contract = _contractRepository.Get(id);

            if(contract == null)
                throw new HttpException(404, "Page not found");

            var model = new NewHire(XmlPath)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };

            model.PopulateForm(_contractRepository.GetFormDictionary(id));

            var companies = _contractRepository.GetCompanies(model.CustomerId).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId, 0).ToList();
            if(units.Any())
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

            if(contract.Company != null)
                model.SetAnswer("Company", contract.Company.Id.ToString(CultureInfo.InvariantCulture));
            if(contract.Unit != null)
                model.SetAnswer("Unit", contract.Unit.Id.ToString(CultureInfo.InvariantCulture));
            model.SetAnswer("EmployeeLastName", contract.Surname);
            model.SetAnswer("EmployeeFirstName", contract.FirstName);

            SessionFacade.SetModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            var model = SessionFacade.GetModel<NewHire>();

            if(model == null)
                return RedirectToAction("index", "dead");

            var dictionary = formCollection.ToDictionary();

            model.TempSave(ref dictionary);
            model.ActiveTab = formCollection["activeTab"];

            _contractRepository.Save(model.FormId, model.Contract.Id, dictionary);

            var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
            var actionState = formCollection["actionState"];

            if(actionStateChange && !string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
            {
                model.Status = int.Parse(actionState);
                _contractRepository.UpdateCaseStateSecondary(model.Contract.Id, model.Status);
                model.Contract = _contractRepository.Get(id);
            }

            SessionFacade.SetModel(model);

            if(!Request.IsAjaxRequest())
                return View(model);

            var view = string.Empty;

            if(model.GetErrorMessages().Any())
                view = RenderRazorViewToString("_Edit", model);

            return Json(new { View = view });
        }

        [HttpPost]
        public JsonResult Print(string id)
        {
            var response = new AjaxResponse();
            var model = SessionFacade.GetModel<NewHire>();

            if(string.IsNullOrEmpty(id))
                response.Exception = new ArgumentNullException("id").Message;

            if(model == null)
            {
                response.Exception = new Exception("model").Message;
                return Json(response);
            }

            var answer = SessionFacade.User.FirstName + " " + SessionFacade.User.Surname;

            var dictionary = new Dictionary<string, string> { { id, answer } };

            model.SetAnswer(id, answer);
            _contractRepository.Save(model.FormId, model.Contract.Id, dictionary);

            _contractRepository.SaveFileViewLog(new FileViewLog
            {
                CaseId = model.Contract.Id, UserId = SessionFacade.User.Id, CreatedDate = DateTime.UtcNow, FileSource = 3, FileName = id, FilePath = "..."
            });

            response.Result = RenderRazorViewToString("_TabDocuments", model);

            return Json(response);
        }

        public ActionResult Contract(string id)
        {
            if(string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            var model = SessionFacade.GetModel<NewHire>();

            if(model == null)
                return RedirectToAction("index", "dead");

            var page = GetNewHirePdfPage(id);

            var workStream = new MemoryStream();
            var document = new Document(PageSize.A4, 42, 53, 100, 200);

            var pdfWriter = PdfWriter.GetInstance(document, workStream);
            pdfWriter.PageEvent = page;
            pdfWriter.CloseStream = false;

            document.Open();

            var html = RenderRazorViewToString("~/Areas/Poland/Views/NewHire/Pdfs/" + id + ".cshtml", model, false);
            var worker = new HTMLWorker(document);
            var stringReader = new StringReader(html);
            worker.Parse(stringReader);
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        private static NewHirePdfPage GetNewHirePdfPage(string id)
        {
            var newHirePdfPage = new NewHirePdfPage { CompanyName = string.Empty };

            if(id == "RetailContract")
                newHirePdfPage.CompanyName = "IKEA Retail  Sp. z o.o";

            return newHirePdfPage;
        }
    }
}
