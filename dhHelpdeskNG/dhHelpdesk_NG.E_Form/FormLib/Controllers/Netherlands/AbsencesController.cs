using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Models;
using ECT.FormLib.Pdfs;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;

namespace ECT.FormLib.Areas.Netherlands.Controllers
{
    public class AbsencesController : FormLib.Controllers.FormLibBaseController
    {
        private readonly IContractRepository _contractRepository;

        public AbsencesController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
            _contractRepository = contractRepository;
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
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
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

            if(string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            var contract = _contractRepository.Get(caseId);

            if(contract == null)
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
            if(id.IndexOf("Dutch") > 0)
                lang = "nl";

            //NYTT
            model.Contract.BaseUrl = _contractRepository.GetSiteUrl(model.Contract.CaseNumber, this.BaseUrl());

            var page = GetPdfPage(model.Form.CustomerId, model.Contract.Unit.Key, lang, id, absenceNr, department);
            page.Contract = model.Contract;

            //Kan göras bättre... /TAN
            model.DocumentData.TypeCode = page.contractsConfig.DocumentTypeCode;
            model.DocumentData.BarCodeUse = page.contractsConfig.BarcodeUse;
            model.DocumentData.BarcodeShowOnMinStateSecondaryId = page.contractsConfig.BarcodeShowOnMinStateSecondaryId;
            model.DocumentData.EmployerSignatureText = page.contractsConfig.EmployerSignatureText;
            model.DocumentData.EmployeeSignatureText = page.contractsConfig.EmployeeSignatureText;
            model.DocumentData.Id = id;
            model.DocumentData.Process = page.contractsConfig.Process;

            //Loop through Pages and add Views if they exist (Appendix etc)
            if (page.contractsConfig.Pages.Count > 0)
            {
                foreach (ContractPage coP in page.contractsConfig.Pages)
                {
                    coP.Html = RenderRazorViewToString(coP.ViewName, model, false);
                }
            }

            // Get the base URL
            string currentPageUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
            string baseUrl = model.Contract.BaseUrl;

            byte[] outPdfBuffer = WinnovativePdfPage.GeneratePdf(model, id, page.contractsConfig.Pages, page.contractsConfig, currentPageUrl, baseUrl).ToArray();

            return this.Pdf(outPdfBuffer);

        }

        private NetherlandsPdfPage GetPdfPage(int customerId, string searchKey, string language, string id, string absenceNr, Department department)
        {
            var pdfPage = new NetherlandsPdfPage();
            
            pdfPage.department = department;                
            pdfPage.Language = language;
            pdfPage.id = id;
            pdfPage.contractsConfig = NetherlandsPdfPage.Get(id);

            return pdfPage;
        }
    }  
}

