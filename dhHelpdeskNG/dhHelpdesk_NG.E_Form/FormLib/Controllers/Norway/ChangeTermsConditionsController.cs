using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Controllers;
using ECT.FormLib.Models;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using System;
using ECT.FormLib.Pdfs;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using System.Collections.Generic;

namespace ECT.FormLib.Areas.Norway.Controllers
{
    public class ChangeTermsConditionsController : ECT.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {
        //public const string xmlPath = "netherlands/changetermsconditions.xml";

        private readonly IContractRepository _contractRepository;
        //private readonly IFileService _fileService;

        public ChangeTermsConditionsController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
            _contractRepository = contractRepository;
            //_fileService = fileService;
        }

        public JsonResult Typeahead(string query, string node, string dependentAttribute, string dependentAttributeValue)
        {
            var model = new FormModel(mainXmlPath);
            var element = model.GetElement(node);

            if (element.Attribute("source") != null)
            {
                var options1 = element.Descendants("option").Select(x => x.Value).ToArray();
                return Json(new { options1 });
            }
            else
            {
                if (element == null && element.Descendants("option").Any())
                    return Json(new object { });

                var options = element.Descendants("option").Where(x => x.Attribute(dependentAttribute) != null
                    && x.Attribute(dependentAttribute).Value == dependentAttributeValue
                    || string.IsNullOrEmpty(dependentAttributeValue)).Select(x => x.Value).ToArray();

                return Json(new { options });
            }
        }

        public JsonResult HomeCostCentre(string query, string node)
        {
            var model = new FormModel(mainXmlPath);
            var element = model.GetElement(node);
            if(element == null && element.Descendants("option").Any())
                return Json(new object { });
            var option = element.Descendants("option").Where(x => x.Attribute("NewDepartment") != null);
            option = option.Where(x => x.Attribute("NewDepartment").Value == query);

            if(option.FirstOrDefault() == null)
                return Json(new object { });
            return Json(new { option.FirstOrDefault().Value });
        }

        [HttpPost]
        public ActionResult ChangePrint(string id, int caseId, string path, string allowanceNr, string query)
        {
            if (allowanceNr == "1")
                allowanceNr = "";

            var response = new AjaxResponse();

            var contract = _contractRepository.Get(caseId);

            if (contract == null)
            {
                response.Exception = new HttpException(404, "Page not found").Message;
                return Json(response);
            }

            var model = new FormModel(mainXmlPath)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
            var dictionary = HttpUtility.ParseQueryString(query).ToDictionary();

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

            response.Result = "";

            return Json(response);
        }

        [HttpPost]
        public JsonResult Print(string id, int caseId, string path, string allowanceNr)
        {

            if (allowanceNr == "1")
            {
                allowanceNr = "";
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
            model.MultiNr = allowanceNr;


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
            model.DocumentData = _contractRepository.GetDocumentData(caseId);

            response.Result = ""; // Show who printed out the document. Shouldnt be used anymore FormLibSessions.User.FullName;

            return Json(response);
        }

        public ActionResult Contract(string id, int caseId, string path, string allowanceNr)
        {
            if (allowanceNr == "1")
            {
                allowanceNr = "";
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
            model.Company = _contractRepository.GetCompany(model.Contract.Company.Key);

            var department = _contractRepository.GetDepartmentByKey(model.Contract.Unit.Key, model.Form.CustomerId);
            model.Department = department;

            //Get document data
            model.DocumentData = _contractRepository.GetDocumentData(caseId);

            var lang = "en";

            var oldHidden = _contractRepository.GetFormDictionary(caseId, model.FormGuid).Where(x => x.Key.Contains("OLD_"))
.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.Replace(Environment.NewLine, ""))).ToList();
            TempData["oldValues"] = oldHidden;

            //NYTT
            model.Contract.BaseUrl = _contractRepository.GetSiteUrl(model.Contract.CaseNumber, this.BaseUrl());

            var page = GetPdfPage(model.Form.CustomerId, model.Contract.Unit.Key, lang, id, model.GetAnswer("Prefixes"), department);           
            page.Contract = model.Contract;

            model.MultiNr = allowanceNr;


            //Kan göras bättre... /TAN
            model.DocumentData.TypeCode = page.contractsConfig.DocumentTypeCode;
            model.DocumentData.BarCodeUse = page.contractsConfig.BarcodeUse;
            model.DocumentData.HeaderUse = page.contractsConfig.HeaderUse;
            model.DocumentData.FooterUse = page.contractsConfig.FooterUse;
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

            var norwayWinnovative = new NorwayWinovativePdfPage();
            byte[] outPdfBuffer = norwayWinnovative.GeneratePdf(model, id, page.contractsConfig.Pages, page.contractsConfig, currentPageUrl, baseUrl).ToArray();


            return this.Pdf(outPdfBuffer);
        }

        private NorwayPdfPage GetPdfPage(int customerId, string searchKey, string language, string id, string prefixes, Department department)
        {
            var pdfPage = new NorwayPdfPage();
            
            if (department != null)
            pdfPage.department = department;
            pdfPage.Language = language;
            pdfPage.id = id;
            pdfPage.contractsConfig = NorwayPdfPage.Get(id);

            if (!string.IsNullOrEmpty(prefixes))
            {
                pdfPage.prefixes = prefixes + " ";
            }

            return pdfPage;
        }
    }
}
