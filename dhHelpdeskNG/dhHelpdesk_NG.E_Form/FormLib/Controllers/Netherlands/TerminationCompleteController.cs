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
    public class TerminationCompleteController : FormLib.Controllers.FormLibBaseController
    {
        public const string xmlPath = "netherlands/terminationcomplete.xml";

        private readonly IContractRepository _contractRepository;
        private readonly IFileService _fileService;

        public TerminationCompleteController(IContractRepository contractRepository
             , IUserRepository userRepository
            , IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
            _contractRepository = contractRepository;
            _fileService = fileService;
        }

        public override ActionResult New()
        {
            var model = FormModelFactory.InitNew(xmlPath);
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
            PopulateWithOptions(ref model);

            return View(model);
        }

        [HttpPost]
        public override ActionResult New(FormCollection formCollection, string[] uploads)
        {
            var model = FormModelFactory.InitNew(xmlPath);
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
            PopulateWithOptions(ref model);

            var dictionary = formCollection.ToDictionary();
            model.TempSave(ref dictionary);

            model.ActiveTab = formCollection["activeTab"];

            var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
            var actionState = formCollection["actionState"];
            model.ActiveStatus = actionState;    

            int caseId;

            if (actionStateChange)
            {
                if (!string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
                {
                    var oldStatus = model.Status;
                    var newStatus = int.Parse(model.GetStatusActionInternalValue(actionState));

                    caseId = _contractRepository.SaveNew(model.FormGuid
                                                        , 0
                                                        , FormLibSessions.User.Id
                                                        , FormLibSessions.User.RegUserId
                                                        , newStatus
                                                        , FormLibUtils.GetSource()
                                                        , model.Language
                                                        , Request.UserHostAddress
                                                        , null
                                                        , dictionary
                                                        , FormLibSessions.User.FullName);

                    _fileService.SaveUploads(uploads, caseId, uploadPath, Session.SessionID);

                    model.Contract = _contractRepository.Get(caseId);

                    //Get document data
                    model.DocumentData = _contractRepository.GetDocumentData(caseId);

                    model.Status = oldStatus;
                    InitSubProcess(model, actionState, caseId, dictionary);
                    model.Status = newStatus;

                    if (model.Contract.ChildCaseNumbers.FirstOrDefault() != 0)
                    {
                        model.ReplaceNode("state", "netherlands/templates/terminationbasiconestate.xml");
                    }

                    return Json(new
                    {
                        View = RenderRazorViewToString("_Edit", model),
                        CancelCase = !FormLibUtils.IsSelfService()
                                        && FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
                    });
                }

                return Json(new
                {
                    View = RenderRazorViewToString("New", model),
                    CancelCase = false,
                    Uploads = uploads
                });
            }

            caseId = _contractRepository.SaveNew(model.FormGuid
                    , 0
                    , FormLibSessions.User.Id
                    , FormLibSessions.User.RegUserId
                    , FormLibUtils.GetCaseSaveState()
                    , FormLibUtils.GetSource()
                    , model.Language
                    , Request.UserHostAddress
                    , null
                    , dictionary
                    , FormLibSessions.User.FullName);

            _fileService.SaveUploads(uploads, caseId, uploadPath, Session.SessionID);

            return RedirectToAction("edit", new { id = caseId });
        }

        public override ActionResult Edit(int id)
        {
            var contract = _contractRepository.Get(id);

            if (contract == null)
                throw new HttpException(404, "Page not found");

            // Use for from Line manger portal to hide och show communicate window.
            if(contract.FinishingDate == null)
            {
                ViewBag.CurrentCaseId = contract.Id;
            }

            var model = FormModelFactory.InitEdit(xmlPath, contract, !string.IsNullOrEmpty(Request.QueryString["locked"]));
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
            PopulateWithOptions(ref model);

            if (model.Contract.ChildCaseNumbers.FirstOrDefault() != 0)
            {
                model.ReplaceNode("state", "netherlands/templates/terminationbasiconestate.xml");
            }

            model.PopulateForm(_contractRepository.GetFormDictionary(id, model.FormGuid));

            //Get document data
            model.DocumentData = _contractRepository.GetDocumentData(id);

            model.SetAnswer("LastName", contract.Surname);
            model.SetAnswer("FirstName", contract.FirstName);

            

            return View(model);
        }

        [HttpPost]
        public override ActionResult Edit(int id, FormCollection formCollection, string[] uploads)
        {
            var contract = _contractRepository.Get(id);

            if (contract == null)
                throw new HttpException(404, "Page not found");

            // Use for from Line manger portal to hide och show communicate window.
            if (contract.FinishingDate == null)
            {
                ViewBag.CurrentCaseId = contract.Id;
            }

            var model = FormModelFactory.InitEdit(xmlPath, contract, !string.IsNullOrEmpty(Request.QueryString["locked"]));
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
            PopulateWithOptions(ref model);

            if (model.Contract.ChildCaseNumbers.FirstOrDefault() != 0)
            {
                model.ReplaceNode("state", "netherlands/templates/terminationbasiconestate.xml");
            }

            var dictionary = formCollection.ToDictionary();

            model.TempSave(ref dictionary);
            model.ActiveTab = formCollection["activeTab"];

            var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
            var actionState = formCollection["actionState"];

            model.ActiveStatus = actionState;

            if(actionStateChange && !string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
            {
                InitSubProcess(model, actionState, id, dictionary);
                model.Status = int.Parse(model.GetStatusActionInternalValue(actionState));
            }

            _contractRepository.SaveNew(model.FormGuid
                    , id
                    , FormLibSessions.User.Id
                    , FormLibSessions.User.RegUserId
                    , model.Status
                    , FormLibUtils.GetSource()
                    , model.Language
                    , Request.UserHostAddress
                    , null
                    , dictionary
                    , FormLibSessions.User.FullName);

            model.Contract = _contractRepository.Get(id);

            //Get document data
            model.DocumentData = _contractRepository.GetDocumentData(id);

            _fileService.SaveUploads(uploads, model.Contract.Id, uploadPath, Session.SessionID);

            if (!Request.IsAjaxRequest())
                return View(model);

            var view = string.Empty;

            if (model.GetErrorMessages().Any())
                view = RenderRazorViewToString("_Edit", model);

            return Json(new
            {
                View = view,
                CancelCase = !FormLibUtils.IsSelfService()
                              && FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
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

            var xp = (!string.IsNullOrEmpty(path) ? path + ".xml" : xmlPath);
            var model = new FormModel(xp)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
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

        public ActionResult Contract(string id, int caseId, string path)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            var contract = _contractRepository.Get(caseId);

            if (contract == null)
                throw new HttpException(404, "Page not found");

            path = (!string.IsNullOrEmpty(path) ? path + ".xml" : xmlPath);
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
            if (id.IndexOf("Dutch") > 0)
                lang = "nl";

            //NYTT
            model.Contract.BaseUrl = _contractRepository.GetSiteUrl(model.Contract.CaseNumber, this.BaseUrl());

            var page = GetPdfPage(model.Form.CustomerId, model.Contract.Unit.Key, lang, id, department);
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

        private NetherlandsPdfPage GetPdfPage(int customerId, string searchKey, string language, string id, Department department)
        {
            var pdfPage = new NetherlandsPdfPage();
           
            pdfPage.department = department;
            pdfPage.Language = language;
            pdfPage.id = id;
            pdfPage.contractsConfig = NetherlandsPdfPage.Get(id);

            return pdfPage;
        }

        private void InitSubProcess(FormModel model, string actionState, int parentCaseId, IDictionary<string, string> dictionary)
        {
            if(model.Contract.ChildCaseNumbers.FirstOrDefault() != 0)
                return;

            var initFormGuid = model.GetInitFormGuid(actionState);
            if(initFormGuid != Guid.Empty)
            {
                _contractRepository.SaveNew(initFormGuid
                    , 0
                    , FormLibSessions.User.Id
                    , FormLibSessions.User.RegUserId
                    , FormLibUtils.GetCaseSaveState()
                    , FormLibUtils.GetSource()
                    , model.Language
                    , Request.UserHostAddress
                    , parentCaseId
                    , dictionary
                    , FormLibSessions.User.FullName);
            }
        }

     }
}
