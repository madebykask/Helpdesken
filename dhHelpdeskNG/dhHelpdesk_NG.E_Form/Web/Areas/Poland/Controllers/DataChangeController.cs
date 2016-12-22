using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using ECT.Web.Controllers;
using ECT.Web.Models;
using ECT.Web.Pdf;
using ECT.Web.Pdf.Poland;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using System.Reflection;
using System.Collections;

namespace ECT.Web.Areas.Poland.Controllers
{
    public class DataChangeController : BaseController
    {
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
            var model = new FormModel(XmlPath);
            var element = model.GetElement(node);

            if(element == null && element.Descendants("option").Any())
                return Json(new object { });

            var options = element.Descendants("option").Where(x => x.Attribute(dependentAttribute) != null
                && x.Attribute(dependentAttribute).Value == dependentAttributeValue
                || string.IsNullOrEmpty(dependentAttributeValue)).Select(x => x.Value).ToArray();

            return Json(new { options });
        }

        public ActionResult New()
        {
            var model = new FormModel(XmlPath)
            {
                Contract = new Contract
                {
                    Initiator = SessionFacade.User.FirstName + " " + SessionFacade.User.Surname
                }
            };

            var companies = _contractRepository.GetCompanies(model.CustomerId, SessionFacade.User.Id).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId, SessionFacade.User.Id, null).ToList();
            if(units.Any())
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

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

            var companies = _contractRepository.GetCompanies(model.CustomerId, SessionFacade.User.Id).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId, SessionFacade.User.Id, null).ToList();
            if(units.Any())
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

            var dictionary = formCollection.ToDictionary();
            model.TempSave(ref dictionary);

            if(model.IsValid(ref dictionary, 10))
            {
                var caseId = _contractRepository.Save(model.FormId
                    , 0
                    , SessionFacade.User.Id
                    , formCollection["EmployeeFirstName"]
                    , formCollection["EmployeeLastName"]
                    , null
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
                    || (SessionFacade.User.WorkingGroups.FirstOrDefault(x => x.Id == contract.WorkingGroupId) == null)
            };

            //LoadSave_GV(id, int.Parse(contract.EmployeeNumber));
            //model.GvValues = _globalViewRepository.GetGvDataDictionary(id);

            model.PopulateForm(_contractRepository.GetFormDictionary(id, model.FormId));

            var companies = _contractRepository.GetCompanies(model.CustomerId, SessionFacade.User.Id).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId, SessionFacade.User.Id, null).ToList();
            if(units.Any())
            {
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());
                model.PopulateFormElementWithOptions("EmployeeUnit", units.ToDictionary());
            }

            if(contract.Company != null)
                model.SetAnswer("Company", contract.Company.Key);

            if(contract.Unit != null)
                model.SetAnswer("Unit", contract.Unit.Key);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection, string[] uploads)
        {
            var contract = _contractRepository.Get(id);

            if(contract == null)
                throw new HttpException(404, "Page not found");

            var model = new FormModel(XmlPath)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"]) 
                    || (SessionFacade.User.WorkingGroups.FirstOrDefault(x => x.Id == contract.WorkingGroupId) == null)
            };

            var companies = _contractRepository.GetCompanies(model.CustomerId, SessionFacade.User.Id).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId, SessionFacade.User.Id, null).ToList();
            if(units.Any())
            {
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());
                model.PopulateFormElementWithOptions("EmployeeUnit", units.ToDictionary());
            }

            var dictionary = formCollection.ToDictionary();

            model.TempSave(ref dictionary);
            model.ActiveTab = formCollection["activeTab"];

            var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
            var actionState = formCollection["actionState"];
            model.ActiveStatus = actionState;

            if(actionStateChange && !string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState), SessionFacade.User.WorkingGroups.Select(x => x.Name).ToArray()))
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
                    model.Status = int.Parse(actionState);
                    _contractRepository.UpdateCaseStateSecondary(model.Contract.Id, model.Status, SessionFacade.User.FullName);
                    model.Contract = _contractRepository.Get(id);
                }
            }

            _contractRepository.Save(model.FormId, model.Contract.Id, dictionary);
            _fileService.SaveUploads(uploads, model.Contract.Id, UploadPath, Session.SessionID);

            if(!Request.IsAjaxRequest())
                return View(model);

            var view = string.Empty;

            if(model.GetErrorMessages().Any() || TempData["Error"] != null)
                view = RenderRazorViewToString("_Edit", model);

            return Json(new
            {
                View = view,
                CancelCase = SessionFacade.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
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

            var model = new FormModel(XmlPath)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };

            model.PopulateForm(_contractRepository.GetFormDictionary(caseId, model.FormId));

            var answer = SessionFacade.User.FirstName + " " + SessionFacade.User.Surname;
            var dictionary = new Dictionary<string, string> { { id, answer } };

            model.SetAnswer(id, answer);

            dictionary.Add(id + "ConcludedOn", concludedOn);
            model.SetAnswer(id + "ConcludedOn", concludedOn);

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

        public ActionResult Contract(string id, int caseId)
        {
            if(string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            var contract = _contractRepository.Get(caseId);

            if(contract == null)
                throw new HttpException(404, "Page not found");

            var model = new FormModel(XmlPath)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };

            model.PopulateForm(_contractRepository.GetFormDictionary(caseId, model.FormId));

            var units = _contractRepository.GetUnits(model.CustomerId, SessionFacade.User.Id, null).ToList();
            if(units.Any())
            {
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());
                model.PopulateFormElementWithOptions("EmployeeUnit", units.ToDictionary());
            }

            var lang = "en";
            if(id.IndexOf("Polish") > 0)
                lang = "pl";

            var page = GetNewHirePdfPage(model.Contract.Company.Key, lang);

            var workStream = new MemoryStream();

            int Right_Margin = 115;
            int Top_margin = 50;
            var document = new Document(PageSize.A4, 42, 45, 70, 70);
            if(model.Contract.Company.Key == "OI")
            {
                document = new Document(PageSize.A4, 40, Right_Margin, Top_margin, 70);
            }


            var pdfWriter = PdfWriter.GetInstance(document, workStream);
            pdfWriter.PageEvent = page;
            pdfWriter.CloseStream = false;
            document.Open();

            var html = RenderRazorViewToString("~/Areas/Poland/Views/DataChange/Pdfs/" + id + ".cshtml", model, false);

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

        private PolandPdfPage GetNewHirePdfPage(string searchKey, string language)
        {
            var newHirePdfPage = new PolandPdfPage();
            var company = _contractRepository.GetCompany(searchKey);
            newHirePdfPage.CurrentCompany = company;
            newHirePdfPage.Language = language;

            return newHirePdfPage;
        }

        private void LoadSave_GV(int caseId, int employeeNumber)
        {
            string curKeyValue = "";
            string curValueValue = "";

            var AllMapFields = _globalViewRepository.GetAllGVFieldsName().ToList();

            var basicInfo = _globalViewRepository.GetBasicData(employeeNumber); // id = Person Number
            foreach (PropertyInfo property in basicInfo.GetType().GetProperties())
            {
                curKeyValue = "";
                curValueValue = "";

                var basicInfo_value = property.GetValue(basicInfo, null);

                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    IEnumerable<PersonalAddress> address = (IEnumerable<PersonalAddress>)basicInfo_value;
                    var personAddress_value = address.ToArray<PersonalAddress>();
                    if (address != null)
                    {
                        for (int j = 0; j < address.Count(); j++)
                        {
                            foreach (PropertyInfo pro in personAddress_value[j].GetType().GetProperties())
                            {
                                curKeyValue = "";
                                curValueValue = "";

                                var dynamicfields_value = pro.GetValue(personAddress_value[j], null);

                                foreach (PropertyInfo df in dynamicfields_value.GetType().GetProperties())
                                {
                                    var curProp = df.GetValue(dynamicfields_value, null);

                                    if (df.Name == "FKey")
                                        curKeyValue = curProp.ToString();
                                    else
                                        if (df.Name == "FValue")
                                            curValueValue = curProp.ToString();
                                }
                                int FormField_Id = AllMapFields.Where(m => m.Form_Id == 27 && m.GVFieldName == curKeyValue).Select(s => s.FormField_Id).FirstOrDefault();
                                if (FormField_Id != 0)
                                    _globalViewRepository.SaveGlobalViewFields(caseId, FormField_Id, curValueValue); //Save Data in special table 
                            }
                        }
                    }
                }

                else
                {

                    foreach (PropertyInfo bd in basicInfo_value.GetType().GetProperties())
                    {
                        var curProp = bd.GetValue(basicInfo_value, null);

                        if (bd.Name == "FKey")
                            curKeyValue = curProp.ToString();
                        else
                            if (bd.Name == "FValue")
                                curValueValue = curProp.ToString();
                    }

                    int FormField_Id = AllMapFields.Where(m => m.Form_Id == 27 && m.GVFieldName == curKeyValue).Select(s => s.FormField_Id).FirstOrDefault();
                    if (FormField_Id != 0)
                        _globalViewRepository.SaveGlobalViewFields(caseId, FormField_Id, curValueValue);
                }



            } // Save Basic Info

            var ConditionsOfEmployment = _globalViewRepository.GetConditionsOfEmployment(employeeNumber); // id = Person Number
            foreach (PropertyInfo property in ConditionsOfEmployment.GetType().GetProperties())
            {
                curKeyValue = "";
                curValueValue = "";

                var ConditionsOfEmployment_value = property.GetValue(ConditionsOfEmployment, null);

                foreach (PropertyInfo df in ConditionsOfEmployment_value.GetType().GetProperties())
                {
                    var curProp = df.GetValue(ConditionsOfEmployment_value, null);

                    if (df.Name == "FKey")
                        curKeyValue = curProp.ToString();
                    else
                        if (df.Name == "FValue")
                            curValueValue = curProp.ToString();
                }

                int FormField_Id = AllMapFields.Where(m => m.Form_Id == 27 && m.GVFieldName == curKeyValue).Select(s => s.FormField_Id).FirstOrDefault();
                if (FormField_Id != 0)
                    _globalViewRepository.SaveGlobalViewFields(caseId, FormField_Id, curValueValue);

            }  // Save Conditions Of Employment 


            var PersonalandfamilyInfo = _globalViewRepository.GetPersonalAndFamilyInfo(employeeNumber); // id = Person Number
            foreach (PropertyInfo property in PersonalandfamilyInfo.GetType().GetProperties())
            {

                curKeyValue = "";
                curValueValue = "";

                var PersonalandfamilyInfo_value = property.GetValue(PersonalandfamilyInfo, null);

                if (typeof(IEnumerable<FamilyMemberInfo>).IsAssignableFrom(property.PropertyType))
                {
                    IEnumerable<FamilyMemberInfo> member = (IEnumerable<FamilyMemberInfo>)PersonalandfamilyInfo_value;
                    var member_value = member.ToArray<FamilyMemberInfo>();

                    for (int j = 0; j < member.Count(); j++)
                    {
                        foreach (PropertyInfo Fm in member_value[j].GetType().GetProperties())
                        {
                            curKeyValue = "";
                            curValueValue = "";

                            var dynamicfields_value = Fm.GetValue(member_value[j], null);

                            foreach (PropertyInfo df in dynamicfields_value.GetType().GetProperties())
                            {
                                var curProp = df.GetValue(dynamicfields_value, null);

                                if (df.Name == "FKey")
                                    curKeyValue = curProp.ToString();
                                else
                                    if (df.Name == "FValue")
                                        curValueValue = curProp.ToString();
                            }
                            int FormField_Id = AllMapFields.Where(m => m.Form_Id == 27 && m.GVFieldName == curKeyValue).Select(s => s.FormField_Id).FirstOrDefault();
                            if (FormField_Id != 0)
                                _globalViewRepository.SaveGlobalViewFields(caseId, FormField_Id, curValueValue); //Save Data in special table 

                        }
                    }
                } // if ended
                else
                {
                    if (typeof(IEnumerable<DocumentInfo>).IsAssignableFrom(property.PropertyType))
                    {

                        IEnumerable<DocumentInfo> IdDoc = (IEnumerable<DocumentInfo>)PersonalandfamilyInfo_value;
                        var IdDoc_value = IdDoc.ToArray<DocumentInfo>();

                        for (int j = 0; j < IdDoc.Count(); j++)
                        {

                            foreach (PropertyInfo pro in IdDoc_value[j].GetType().GetProperties())
                            {
                                curKeyValue = "";
                                curValueValue = "";

                                var IdOfEmployment_value = pro.GetValue(IdDoc_value[j], null);

                                foreach (PropertyInfo df in IdOfEmployment_value.GetType().GetProperties())
                                {
                                    var curProp = df.GetValue(IdOfEmployment_value, null);

                                    if (df.Name == "FKey")
                                        curKeyValue = curProp.ToString();
                                    else
                                        if (df.Name == "FValue")
                                            curValueValue = curProp.ToString();
                                }
                                int FormField_Id = AllMapFields.Where(m => m.Form_Id == 27 && m.GVFieldName == curKeyValue).Select(s => s.FormField_Id).FirstOrDefault();
                                if (FormField_Id != 0)
                                    _globalViewRepository.SaveGlobalViewFields(caseId, FormField_Id, curValueValue); //Save Data in special table 
                            }
                        }
                    }
                    else
                    {
                        foreach (PropertyInfo df in PersonalandfamilyInfo_value.GetType().GetProperties())
                        {
                            var curProp = df.GetValue(PersonalandfamilyInfo_value, null);

                            if (df.Name == "FKey")
                                curKeyValue = curProp.ToString();
                            else
                                if (df.Name == "FValue")
                                    curValueValue = curProp.ToString();
                        }
                        int FormField_Id = AllMapFields.Where(m => m.Form_Id == 27 && m.GVFieldName == curKeyValue).Select(s => s.FormField_Id).FirstOrDefault();
                        if (FormField_Id != 0)
                            _globalViewRepository.SaveGlobalViewFields(caseId, FormField_Id, curValueValue); //Save Data in special table 
                    }
                }   // else ended                                             
            }  // Save Personel Family Info       

        } //End LoadSave_GV  
    }
}