using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Controllers;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.FormLib.Pdfs;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;

namespace DH.Helpdesk.EForm.FormLib.Areas.Poland.Controllers
{
    public class DataChangeController : FormLibBaseController
    {
        public const string xmlPath = "poland/datachange.xml";

        private readonly IContractRepository _contractRepository;
        private readonly IGlobalViewRepository _globalViewRepository;
        private readonly IFileService _fileService;

        public DataChangeController(IContractRepository contractRepository
             , IGlobalViewRepository globalViewRepository
             , IUserRepository userRepository
             , IFileService fileService)
            : base(userRepository)
        {
            _globalViewRepository = globalViewRepository;
            _contractRepository = contractRepository;
            _fileService = fileService;
        }

        public JsonResult Typeahead(string query, string node, string dependentAttribute, string dependentAttributeValue)
        {
            var model = new FormModel(xmlPath);
            var element = model.GetElement(node);

            if(element == null && element.Descendants("option").Any())
                return Json(new object { });

            var options = element.Descendants("option").Where(x => x.Attribute(dependentAttribute) != null
                && x.Attribute(dependentAttribute).Value == dependentAttributeValue
                || string.IsNullOrEmpty(dependentAttributeValue)).Select(x => x.Value).ToArray();

            return Json(new { options });
        }

        public override ActionResult New()
        {
            var model = new FormModel(xmlPath) { };
            model.Contract = new Contract { Initiator = FormLibSessions.User.FullName };
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);

            PopulateWithOptions(ref model);

            return View(model);
        }

        [HttpPost]
        public override ActionResult New(FormCollection formCollection, string[] uploads)
        {
            var model = new FormModel(xmlPath) { };
            model.Contract = new Contract { Initiator = FormLibSessions.User.FullName };
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);

            PopulateWithOptions(ref model);

            var dictionary = formCollection.ToDictionary();
            model.TempSave(ref dictionary);

            if(model.IsValid(ref dictionary, 10))
            {
                var caseId = _contractRepository.SaveNew(model.FormGuid
                    , 0
                    , FormLibSessions.User.Id
                    , regUserId
                    , 10
                    , FormLibUtils.GetSource()
                    , model.Language
                    , Request.UserHostAddress
                    , null
                    , dictionary
                    , FormLibSessions.User.FullName);

                _fileService.SaveUploads(uploads, caseId, uploadPath, Session.SessionID);

                return RedirectToAction("edit", new { id = caseId });
            }

            return View(model);
        }

        public override ActionResult Edit(int id)
        {
            var contract = _contractRepository.Get(id);

            if(contract == null)
                throw new HttpException(404, "Page not found");

            var model = new FormModel(xmlPath)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
                    || (FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == contract.WorkingGroupId) == null)
            };

            model.PopulateForm(_contractRepository.GetFormDictionary(id, model.FormGuid));
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
            PopulateWithOptions(ref model);

            if(contract.Company != null)
                model.SetAnswer("Company", contract.Company.Key);

            if(contract.Unit != null)
                model.SetAnswer("Unit", contract.Unit.Key);

            return View(model);
        }

        [HttpPost]
        public override ActionResult Edit(int id, FormCollection formCollection, string[] uploads)
        {
            var contract = _contractRepository.Get(id);

            if(contract == null)
                throw new HttpException(404, "Page not found");

            var model = new FormModel(xmlPath)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
                    || (FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == contract.WorkingGroupId) == null)
            };

            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);

            PopulateWithOptions(ref model);

            var dictionary = formCollection.ToDictionary();

            model.TempSave(ref dictionary);
            model.ActiveTab = formCollection["activeTab"];

            var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
            var actionState = formCollection["actionState"];
            model.ActiveStatus = actionState;

            if(actionStateChange && !string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState), FormLibSessions.User.WorkingGroups.Select(x => x.Name).ToArray()))
            {
                var empty = false;
                if(model.Status < int.Parse(actionState) && (int.Parse(actionState) != 99))
                {
                    var sectionEmpty = true;
                    foreach(var value in model.GetAnswer("DataChangeSelection").Split(','))
                    {
                        if(value == "") continue;

                        empty = false;
                        sectionEmpty = true;
                        var nodes = model.GetDataChangeFields(value);
                        foreach(var node in nodes)
                        {
                            if(!node.StartsWith("#") && !string.IsNullOrEmpty(node))
                            {
                                if(!string.IsNullOrEmpty(model.GetAnswer(node)))
                                {
                                    sectionEmpty = false;
                                    break;
                                }
                            }
                        }

                        if(sectionEmpty)
                        {
                            empty = true;
                            break;
                        }
                    }
                }

                if(empty)
                {
                    TempData.Add("Error", model.Translate("At least one of the fields has to be completed (section cannot be empty if it was ticked)."));
                }
                else
                {
                    var actualStatus = int.Parse(model.GetStatusActionInternalValue(actionState));
                    model.Status = actualStatus;
                }
            }

            //if(actionStateChange && !string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
            //{
            //    var actualStatus = int.Parse(model.GetStatusActionInternalValue(actionState));
            //    model.Status = actualStatus;
            //}

            var lognote = "";
            //Set lognote to empty if errormessages
            if(model.GetErrorMessages().Any())
            {
                lognote = model.GetAnswer("InternalLogNote");
                dictionary["InternalLogNote"] = "";
                model.SetAnswer("InternalLogNote", "");
            }

            _contractRepository.SaveNew(model.FormGuid
                    , id
                    , FormLibSessions.User.Id
                    , regUserId
                    , model.Status
                    , FormLibUtils.GetSource()
                    , model.Language
                    , Request.UserHostAddress
                    , null
                    , dictionary
                    , FormLibSessions.User.FullName);

            //Set lognote back after save, or set it to empty if it saved without errormessage
            model.SetAnswer("InternalLogNote", lognote);

            _fileService.SaveUploads(uploads, model.Contract.Id, uploadPath, Session.SessionID);

            model.Contract = _contractRepository.Get(id);

            if(!Request.IsAjaxRequest())
                return View(model);

            var view = string.Empty;

            if(model.GetErrorMessages().Any() || TempData["Error"] != null)
                view = RenderRazorViewToString("_Edit", model);

            return Json(new
            {
                View = view,
                CancelCase = FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
            });
        }

        [HttpPost]
        public JsonResult Print(string id, int caseId, string concludedOn)
        {
            var response = new AjaxResponse();

            if(string.IsNullOrEmpty(id))
                response.Exception = new ArgumentNullException("id").Message;

            var contract = _contractRepository.Get(caseId);

            if(contract == null)
            {
                response.Exception = new HttpException(404, "Page not found").Message;
                return Json(response);
            }

            var model = new FormModel(xmlPath)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };

            model.PopulateForm(_contractRepository.GetFormDictionary(caseId, model.FormGuid));

            var answer = FormLibSessions.User.FullName;
            var dictionary = new Dictionary<string, string> { { id, answer } };

            model.SetAnswer(id, answer);

            if (concludedOn != null)
            {
                dictionary.Add(id + "ConcludedOn", concludedOn);
                model.SetAnswer(id + "ConcludedOn", concludedOn);
            }
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

            response.Result = FormLibSessions.User.FullName;

            return Json(response);
        }

        public ActionResult Contract(string id, int caseId)
        {
            if(string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            var contract = _contractRepository.Get(caseId);

            if(contract == null)
                throw new HttpException(404, "Page not found");

            var model = new FormModel(xmlPath)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };

            model.PopulateForm(_contractRepository.GetFormDictionary(caseId, model.FormGuid));

            var units = _contractRepository.GetUnits(model.CustomerId, FormLibSessions.User.Id, null).ToList();
            if(units.Any())
            {
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());
                model.PopulateFormElementWithOptions("EmployeeUnit", units.ToDictionary());
            }

            var lang = "en";
            if(id.IndexOf("Polish") > 0)
                lang = "pl";

            var HasSignature = true;

            //if (id == "AddendumPolish-1035" || id == "AddendumPolish-1040")
            //{
            //    HasSignature = false;
            //}

            var page = GetPdfPage(model.Contract.Company.Key, lang, HasSignature);

            var workStream = new MemoryStream();
            
            
            int Right_Margin = 45;
            int Top_margin = 45;
            var document = new Document(PageSize.A4, 42, 45, 70, 70);
            if (model.Contract.Company.Key == "OI")
            {
                document = new Document(PageSize.A4, 40, Right_Margin, Top_margin, 90);
            }

            //****  To Do: in the case of apply changes for Inter Ikea Header-Footer needs to be un commetn /SG
            //int Right_Margin = 115;
            //int Top_margin = 50;
            //var document = new Document(PageSize.A4, 42, 45, 70, 70);
            //if (model.Contract.Company.Key == "OI")
            //{
            //    document = new Document(PageSize.A4, 40, Right_Margin, Top_margin, 70);
            //}

            var pdfWriter = PdfWriter.GetInstance(document, workStream);
            pdfWriter.PageEvent = page;
            pdfWriter.CloseStream = false;
            document.Open();

            var html = RenderRazorViewToString("~/FormLibContent/Areas/Poland/Views/DataChange/Pdfs/" + id + ".cshtml", model, false);

            using(TextReader htmlViewReader = new StringReader(html))
            {
                using(var htmlWorker = new HTMLWorkerExtended(document))
                {
                    string VerdanauniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "VERDANA.TTF");
                    iTextSharp.text.FontFactory.Register(VerdanauniTff);
                    htmlWorker.Open();
                    var styleSheet = new iTextSharp.text.html.simpleparser.StyleSheet();
                    styleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.FACE, "Verdana Unicode MS");
                    styleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, BaseFont.IDENTITY_H);
                    htmlWorker.SetStyleSheet(styleSheet);
                    htmlWorker.Parse(htmlViewReader);
                    htmlWorker.Close();
                }
            }
            document.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return this.Pdf(byteInfo);
        }

        private PolandPdfPage GetPdfPage(string searchKey, string language, bool HasSignature)
        {
            var pdfPage = new PolandPdfPage();
            var company = _contractRepository.GetCompany(searchKey);
            pdfPage.CurrentCompany = company;
            pdfPage.Language = language;
            pdfPage.hasSignature = HasSignature;

            return pdfPage;
        }

        new private void PopulateWithOptions(ref FormModel model)
        {
            var companies = _contractRepository.GetCompanies(model.CustomerId, FormLibSessions.User.Id).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId, FormLibSessions.User.Id, null).ToList();
            if(units.Any())
            {
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());
                model.PopulateFormElementWithOptions("EmployeeUnit", units.ToDictionary());
            }
        }
    }
}
