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
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using System.Collections.Generic;

namespace DH.Helpdesk.EForm.FormLib.Areas.Netherlands.Controllers
{
    public class ChangeTermsConditionsController : FormLibBaseController
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

        //public override ActionResult New()
        //{
        //    //var model = FormModelFactory.InitNew(xmlPath);
        //    //TempData["oldValues"] = null;
        //    //PopulateWithOptions(ref model);

        //    var model = FormModelFactory.InitNew(xmlPath);

        //    PopulateWithOptions(ref model);

        //    return View(model);

        //    //return View(model);
        //}

        //[HttpPost]
        //public ActionResult New(FormCollection formCollection, string[] uploads)
        //{
        //    var model = FormModelFactory.InitNew(xmlPath);

        //    var dictionary = formCollection.ToDictionary();
        //    model.TempSave(ref dictionary);

        //    var oldHidden = dictionary.Where(x => x.Key.Contains("OLD_"))
        //        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value.Replace(Environment.NewLine, ""))).ToList();

        //    TempData["oldValues"] = oldHidden;

        //    model.ActiveTab = formCollection["activeTab"];

        //    var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
        //    var actionState = formCollection["actionState"];
        //    model.ActiveStatus = actionState;

        //    int caseId;

        //    if (actionStateChange)
        //    {
        //        if (!string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
        //        {
        //            var oldStatus = model.Status;
        //            var newStatus = int.Parse(model.GetStatusActionInternalValue(actionState));

        //            caseId = _contractRepository.Save(model.FormGuid
        //                                                , 0
        //                                                , FormLibSessions.User.Id
        //                                                , formCollection["FirstName"] ?? ""
        //                                                , formCollection["LastName"] ?? ""
        //                                                , formCollection["DateOfBirth"] ?? ""
        //                                                , newStatus
        //                                                , regUserId
        //                                                , dictionary);

        //            _fileService.SaveUploads(uploads, caseId, uploadPath, Session.SessionID);
        //            model.Contract = _contractRepository.Get(caseId);

        //            model.Status = newStatus;

        //            return Json(new
        //            {
        //                View = RenderRazorViewToString("_Edit", model),
        //                CancelCase = !FormLibUtils.IsSelfService()
        //                              && FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
        //            });
        //        }

        //        return Json(new
        //        {
        //            View = RenderRazorViewToString("New", model),
        //            CancelCase = false
        //        });
        //    }
        //    caseId = _contractRepository.Save(model.FormGuid
        //                                            , 0
        //                                            , FormLibSessions.User.Id
        //                                            , formCollection["FirstName"] ?? ""
        //                                            , formCollection["LastName"] ?? ""
        //                                            , formCollection["DateOfBirth"] ?? ""
        //                                            , FormLibUtils.GetCaseSaveState()
        //                                            , regUserId
        //                                            , dictionary);

        //    _fileService.SaveUploads(uploads, caseId, uploadPath, Session.SessionID);
        //    return RedirectToAction("edit", new { id = caseId });
        //}

        //public ActionResult Edit(int id)
        //{
        //    var contract = _contractRepository.Get(id);

        //    if(contract == null)
        //        throw new HttpException(404, "Page not found");

        //    var model = FormModelFactory.InitEdit(xmlPath, contract, !string.IsNullOrEmpty(Request.QueryString["locked"]));

        //    model.PopulateForm(_contractRepository.GetFormDictionary(id, model.FormGuid));

        //    var oldHidden = _contractRepository.GetFormDictionary(id, model.FormGuid).Where(x => x.Key.Contains("OLD_"))
        //       .Select(x => new KeyValuePair<string, string>(x.Key, x.Value.Replace(Environment.NewLine, ""))).ToList();
        //    TempData["oldValues"] = oldHidden;
            
        //    PopulateWithOptions(ref model);

        //    model.SetAnswer("LastName", contract.Surname);
        //    model.SetAnswer("FirstName", contract.FirstName);

        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection formCollection, string[] uploads)
        //{
        //    var contract = _contractRepository.Get(id);

        //    if (contract == null)
        //        throw new HttpException(404, "Page not found");

        //    var model = FormModelFactory.InitEdit(xmlPath, contract, !string.IsNullOrEmpty(Request.QueryString["locked"]));

        //    var dictionary = formCollection.ToDictionary();
        //    model.TempSave(ref dictionary);

        //    var oldHidden = dictionary.Where(x => x.Key.Contains("OLD_"))
        //        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value.Replace(Environment.NewLine, ""))).ToList();
        //    TempData["oldValues"] = oldHidden;


        //    model.ActiveTab = formCollection["activeTab"];

        //    var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
        //    var actionState = formCollection["actionState"];
        //    model.ActiveStatus = actionState;

        //    if (actionStateChange && !string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
        //    {
        //        model.Status = int.Parse(model.GetStatusActionInternalValue(actionState));
        //        _contractRepository.UpdateCaseStateSecondary(model.Contract.Id, model.Status, FormLibSessions.User.FullName);
        //        model.Contract = _contractRepository.Get(id);
        //    }

        //    _contractRepository.Save(model.FormGuid, model.Contract.Id, dictionary);
        //    _fileService.SaveUploads(uploads, model.Contract.Id, uploadPath, Session.SessionID);

        //    if (!Request.IsAjaxRequest())
        //        return View(model);

        //    var view = string.Empty;

        //    if (model.GetErrorMessages().Any())
        //        view = RenderRazorViewToString("_Edit", model);

        //    return Json(new
        //    {
        //        View = view,
        //        CancelCase = !FormLibUtils.IsSelfService()
        //            && FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
        //    });
        //}

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

            var xp = (!string.IsNullOrEmpty(path) ? path + ".xml" : mainXmlPath);
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
                    , regUserId
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


            response.Result = "";// Show who printed out the document. Shouldnt be used anymore FormLibSessions.User.FullName;

            return Json(response);
        }


        public ActionResult Contract(string id, int caseId, string path)
        {
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
            if (id.IndexOf("Dutch") > 0)
                lang = "nl";

            var oldHidden = _contractRepository.GetFormDictionary(caseId, model.FormGuid).Where(x => x.Key.Contains("OLD_"))
          .Select(x => new KeyValuePair<string, string>(x.Key, x.Value.Replace(Environment.NewLine, ""))).ToList();
            TempData["oldValues"] = oldHidden;

            var page = GetPdfPage(model.Form.CustomerId, model.Contract.Unit.Key, lang, id, department);
            page.Contract = model.Contract;
            var workStream = new MemoryStream();

            var document = new Document(PageSize.A4, 42, 45, 55, 50);
            var pdfWriter = PdfWriter.GetInstance(document, workStream);

            pdfWriter.PageEvent = page;
            pdfWriter.CloseStream = false;

            document.Open();

            var html = RenderRazorViewToString("~/FormLibContent/Areas/Netherlands/Views/changetermsconditions/Pdfs/" + id + ".cshtml", model, false);
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

        private NetherlandsPdfPage GetPdfPage(int customerId, string searchKey, string language, string id, Department department)
        {
            var pdfPage = new NetherlandsPdfPage();
            
            if (department != null)
            pdfPage.department = department;
            pdfPage.Language = language;
            pdfPage.id = id;
         

            return pdfPage;
        }

        //private void PopulateWithOptions(ref FormModel model)
        //{
        //    var companies = _contractRepository.GetCompanies(model.CustomerId, FormLibSessions.User.Id).ToList();
        //    if (companies.Any())
        //        model.PopulateFormElementWithOptions("Department", companies.ToDictionary());

        //    var units = _contractRepository.GetUnits(model.CustomerId, FormLibSessions.User.Id, null).ToList();
        //    if (units.Any())
        //    {
        //        model.PopulateFormElementWithOptions("BusinessUnit", units.ToDictionary());                
        //    }
        //}
    }
}
