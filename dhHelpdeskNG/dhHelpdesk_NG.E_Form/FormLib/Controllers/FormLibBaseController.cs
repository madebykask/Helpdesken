//#define LOCAL_TEST_AV_LM

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Cache;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.Model.Entities;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.FormLib.Controllers
{
    public class FormLibBaseController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IFileService _fileService;

        protected string uploadPath;
        protected string regUserId = "";
        protected string mainXmlPath;

        public FormLibBaseController() { }

        public FormLibBaseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public FormLibBaseController(IUserRepository userRepository, IContractRepository contractRepository, IFileService fileService)
        {
            _userRepository = userRepository;
            _contractRepository = contractRepository;
            _fileService = fileService;
        }

        #region Virtual ActionResults

        public virtual JsonResult BusinessUnitData(string query)
        {
            //BU is called department in HelpDesk.
            Department department = new Department();
            var id = 0;
            id = Int32.Parse(query);
            department = _contractRepository.GetDepartmentById(id);

            return Json(department, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult New()
        {
            var model = FormModelFactory.InitNew(mainXmlPath);
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
            PopulateWithOptions(ref model);

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult New(FormCollection formCollection, string[] uploads)
        {
            var model = FormModelFactory.InitNew(mainXmlPath);
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);

            PopulateWithOptions(ref model);

            var dictionary = formCollection.ToDictionary();
            model.TempSave(ref dictionary);

            // Specific for change terms conditions
            var oldHidden = dictionary.Where(x => x.Key.Contains("OLD_"))
                .Select(x => new KeyValuePair<string, string>(x.Key, x.Value.Replace(Environment.NewLine, ""))).ToList();
            TempData["oldValues"] = oldHidden;

            model.ActiveTab = formCollection["activeTab"];

            var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
            var actionState = formCollection["actionState"];
            model.ActiveStatus = actionState;

            //if (dictionary.ContainsKey("CalculateWatchDate") == true && dictionary.ContainsKey("ChangeValidFrom") == true)
            //{
            //    dictionary["WatchDate"] = _contractRepository.GetWatchDate(model.Form.CustomerId, formCollection["ChangeValidFrom"], 3, 1);
            //}

            int caseId;

            if(actionStateChange)
            {
                if(!string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
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

                    model.Status = newStatus;

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

        public virtual ActionResult Edit(int id)
        {
            var contract = _contractRepository.Get(id);

            if(contract == null)
                throw new HttpException(404, "Page not found");

            // Use for from Line manger portal to hide och show communicate window.
            if(contract.FinishingDate == null)
            {
                ViewBag.CurrentCaseId = contract.Id;
            }

            var model = FormModelFactory.InitEdit(mainXmlPath, contract, !string.IsNullOrEmpty(Request.QueryString["locked"]));
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);

            PopulateWithOptions(ref model);

            var dictionary = _contractRepository.GetFormDictionary(id, model.FormGuid);
            model.PopulateForm(dictionary);

            //Get document data
            model.DocumentData = _contractRepository.GetDocumentData(id);

            // Specific for change terms conditions
            var oldHidden = dictionary.Where(x => x.Key.Contains("OLD_"))
                .Select(x => new KeyValuePair<string, string>(x.Key, x.Value.Replace(Environment.NewLine, ""))).ToList();
            TempData["oldValues"] = oldHidden;

            model.SetAnswer("LastName", contract.Surname);
            model.SetAnswer("FirstName", contract.FirstName);

            model.StaticFiles = _contractRepository.GetStaticDocuments(contract.ProductAreaId);
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(int id, FormCollection formCollection, string[] uploads)
        {
            var contract = _contractRepository.Get(id);

            if(contract == null)
                throw new HttpException(404, "Page not found");

            // Use for from Line manger portal to hide och show communicate window.
            if (contract.FinishingDate == null)
            {
                ViewBag.CurrentCaseId = contract.Id;
            }

            var model = FormModelFactory.InitEdit(mainXmlPath, contract, !string.IsNullOrEmpty(Request.QueryString["locked"]));
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
            PopulateWithOptions(ref model);

            var dictionary = formCollection.ToDictionary();
            model.TempSave(ref dictionary);

            // Specific for change terms conditions
            var oldHidden = dictionary.Where(x => x.Key.Contains("OLD_")).ToList();
            TempData["oldValues"] = oldHidden;

            model.ActiveTab = formCollection["activeTab"];

            var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
            var actionState = formCollection["actionState"];
            model.ActiveStatus = actionState;

            if(actionStateChange && !string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
            {
                model.Status = int.Parse(model.GetStatusActionInternalValue(actionState));
            }


            var lognote = "";
            //Set lognote to empty if errormessages
            if (model.GetErrorMessages().Any())
            {
                lognote = model.GetAnswer("InternalLogNote");
                dictionary["InternalLogNote"] = "";
                model.SetAnswer("InternalLogNote", "");
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

            //Set lognote back after save, or set it to empty if it saved without errormessage
            model.SetAnswer("InternalLogNote", lognote);
            //Get document data
            model.DocumentData = _contractRepository.GetDocumentData(id);

            _fileService.SaveUploads(uploads, model.Contract.Id, uploadPath, Session.SessionID);

            model.Contract = _contractRepository.Get(id);
            model.StaticFiles = _contractRepository.GetStaticDocuments(model.Contract.ProductAreaId);

            FormLibSessions.TestDataChanged = "MyValue1";

            if (!Request.IsAjaxRequest())
            {
                model.SetAnswer("InternalLogNote", "");
                return View(model);
            }

            var view = string.Empty;

            if (model.GetErrorMessages().Any())
            {
                view = RenderRazorViewToString("_Edit", model);
            }    

            return Json(new
            {
                View = view,
                CancelCase = !FormLibUtils.IsSelfService()
                    && FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
            });
        }

        public virtual ActionResult GetGlobalViewFields(int caseId, Guid formGuid)
        {
            var model = new FormModelGlobalView();
            model.FormModel = new FormModel(mainXmlPath);
            model.FormModel.DocumentData = _contractRepository.GetDocumentData(caseId);
            model.FormModel.Form = _contractRepository.GetFormByGuid(formGuid);


            var contract = _contractRepository.Get(caseId);

             // Not the nicest solution, TODO: change this if block
            if (contract.ChildCaseNumbers.Any())
            {
                model.FormFields = _contractRepository.GetFormFields(caseId, formGuid).Where(x => x.HCMData == true).Where(x => x.ParentGVFields == true).Select(x =>
                {
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "Company".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "Company".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "NewCompany".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "NewCompany".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "BusinessUnit".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "BusinessUnit".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "NewBusinessUnit".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "NewBusinessUnit".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "Department".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "Department".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "NewDepartment".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "NewDepartment".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "ServiceArea".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "ServiceArea".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "NewServiceArea".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "NewServiceArea".ToLower()).DocumentFieldValue : x.FormFieldValue;

                    //TODO: This is a quickfix to list values from checkbox. Since GetFormField is populated from DB, we don´t know that the element is a checkbox... /TAN
                    x.GVShow = true;
                    x.GVLabel = (x.FormFieldName.ToLower().Contains("_") && model.FormModel.GetElementType(x.FormFieldName).ToString().ToLower() == "checkbox") ? model.FormModel.GetAttributeValue(x.FormFieldName, "gvLabel") : "";
                    x.GVValue = (x.FormFieldName.ToLower().Contains("_") && model.FormModel.GetElementType(x.FormFieldName).ToString().ToLower() == "checkbox" && x.FormFieldValue == "1,0".ToLower()) ? model.FormModel.GetLabel(x.FormFieldName) : "";
                    x.GVShow = (x.FormFieldName.ToLower().Contains("_") && model.FormModel.GetElementType(x.FormFieldName).ToString().ToLower() == "checkbox" && x.FormFieldValue == "0".ToLower()) ? false : true;

                    return x;
                }).Where(x => x.GVShow == true).ToList();

                var view = RenderRazorViewToString("_GlobalViewData", model);

                return Json(new { View = view });
            }
            else
            {
                //set values that are int to string
                model.FormFields = _contractRepository.GetFormFields(caseId, formGuid).Where(x => x.HCMData == true).Select(x =>
                {
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "Company".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "Company".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "NewCompany".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "NewCompany".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "BusinessUnit".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "BusinessUnit".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "NewBusinessUnit".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "NewBusinessUnit".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "Department".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "Department".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "NewDepartment".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "NewDepartment".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "ServiceArea".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "ServiceArea".ToLower()).DocumentFieldValue : x.FormFieldValue;
                    x.FormFieldValue = (x.FormFieldName.ToLower() == "NewServiceArea".ToLower()) ? model.FormModel.DocumentData.DocumentFields.Find(dd => dd.DocumentFieldName.ToLower() == "NewServiceArea".ToLower()).DocumentFieldValue : x.FormFieldValue;

                    //TODO: This is a quickfix to list values from checkbox. Since GetFormField is populated from DB, we don´t know that the element is a checkbox... /TAN
                    x.GVShow = true;
                    x.GVLabel = (x.FormFieldName.ToLower().Contains("_") && model.FormModel.GetElementType(x.FormFieldName).ToString().ToLower() == "checkbox") ? model.FormModel.GetAttributeValue(x.FormFieldName, "gvLabel") : "";
                    x.GVValue = (x.FormFieldName.ToLower().Contains("_") && model.FormModel.GetElementType(x.FormFieldName).ToString().ToLower() == "checkbox" && x.FormFieldValue == "1,0".ToLower()) ? model.FormModel.GetLabel(x.FormFieldName) : "";
                    x.GVShow = (x.FormFieldName.ToLower().Contains("_") && model.FormModel.GetElementType(x.FormFieldName).ToString().ToLower() == "checkbox" && x.FormFieldValue == "0".ToLower()) ? false : true;

                    return x;
                }).Where(x => x.GVShow == true).ToList();

                var view = RenderRazorViewToString("_GlobalViewData", model);

                return Json(new { View = view });
            }
           
        }
        #endregion

        #region Public Methods

        public ActionResult GetBusinessUnits(int customerId, int companyId)
        {
            var businessUnits = _contractRepository.GetUnits(customerId, FormLibSessions.User.Id, companyId).ToList();

            //var currentLanguage = "";

            //if(FormLibSessions.User !=  null) 
            //    currentLanguage = FormLibSessions.User.Language;         

            //foreach(var option in businessUnits)
            //    option.Name  = FormLibI18N.Translate(option.Name, currentLanguage);

            return Json(businessUnits);
        }

        public JsonResult GetAllCompanies(int customerId) {
            var companies = _contractRepository.GetCompanies(customerId, null);
            return Json(companies, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBusinessUnits(int customerId, int companyId)
        {
            if (companyId == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var businessUnits = _contractRepository.GetUnits(customerId, null, companyId).ToList();
            return Json(businessUnits, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllFunctions(int businessUnitId)
        {
            if (businessUnitId == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var functions = _contractRepository.GetOUs(businessUnitId, null).ToList();
            return Json(functions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllDepartments(int functionId)
        {
            if (functionId == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var departments = _contractRepository.GetOUs(null, functionId).ToList();
            return Json(departments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFunctions(int businessUnitId)
        {
            var serviceAreas = _contractRepository.GetOUs(businessUnitId, null).ToList();

            //var currentLanguage = "";

            //if(FormLibSessions.User !=  null) 
            //    currentLanguage = FormLibSessions.User.Language;         

            //foreach(var ou in serviceAreas)
            //    ou.Name  = FormLibI18N.Translate(ou.Name, currentLanguage);
 
            return Json(serviceAreas);
        }

        public ActionResult GetDepartments(int serviceAreaId)
        {   
            var departments = _contractRepository.GetOUs(null, serviceAreaId).ToList();

            //var currentLanguage = "";

            //if(FormLibSessions.User !=  null) 
            //    currentLanguage = FormLibSessions.User.Language;         

            //foreach(var ou in departments)
            //    ou.Name  = FormLibI18N.Translate(ou.Name, currentLanguage);

            return Json(departments);
        }
    

        public void PopulateWithOptions(ref FormModel model)
        {
            var companies = _contractRepository.GetCompanies(0, FormLibSessions.User.Id).ToList();

            if(!(model.Form == null))
                companies = _contractRepository.GetCompanies(model.Form.CustomerId, FormLibSessions.User.Id).ToList();
            else
                companies = _contractRepository.GetCompanies(model.CustomerId, FormLibSessions.User.Id).ToList();

            if(companies.Any())
            {
                var dict = new Dictionary<string, string>();

                foreach(var option in companies)
                    dict.Add(option.Name, option.Id.ToString());

                model.PopulateFormElementWithOptions("Company", dict);
                model.PopulateFormElementWithOptions("NewCompany", dict);
            }

            var rd = HttpContext.Request.RequestContext.RouteData;
            var controllerName = rd.GetRequiredString("controller");

            if(controllerName.ToLower() == "changetermsconditions" || controllerName.ToLower() == "hiring")
            {
                if(FormLibUtils.IsSelfService() && FormLibSessions.User != null)
                {
                    model.SetAnswer("ReportsToLineManager", FormLibSessions.User.FullName);
                }
            }
        }

    

        // DL: 2015-01-26 - For UK only at this moment
        //TAN: 2015-12-07 - This should not be here, move to country specific...
        public JsonResult CountrySearchUK(string query)
        {
            var di = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath(FormLibConstants.Paths.XmlDirectory));
            var path = di.FullName;

            path = Path.Combine(di.FullName, "unitedkingdom/templates/Data_Country.xml");

            var xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(path);

            var xDoc = xmlDocument.ToXDocument();

            if(xDoc == null && xDoc.Descendants("option").Any())
                return Json(new object { });

            var options = xDoc.Descendants("option").Where(x => x.Attribute("value") != null
                && x.Attribute("value").Value.ToUpper().Contains(query.ToUpper())).Select(x => new { Value = x.Value }).ToArray();

            return Json(new { options }, JsonRequestBehavior.AllowGet);
        }

        // TAN: 2015-12-07 - Data_Common_Country.xml needs to be placed in "Xmls/{Country}/Data" folder and named: Data_Common_Country.xml for this to work
        public JsonResult CountrySearch(string query, Guid formGuid)
        {
            //Translate query to english so that we can search in xml

            //Get language from User
            string language = (!string.IsNullOrEmpty(FormLibSessions.User.Language) ? FormLibSessions.User.Language : "en");

            var model = new FormModelGlobalView();
            model.FormModel = new FormModel(mainXmlPath);


            model.FormModel.Form = _contractRepository.GetFormByGuid(formGuid);


            string queryText = query;

            //only do this if we need to translate
            if (language.ToLower() != "en")
            {
                queryText = model.FormModel.TranslateRevert(query);
            }

            var di = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath(FormLibConstants.Paths.XmlDirectory));
            var path = di.FullName;

            string area = ControllerContext.RouteData.DataTokens["area"].ToString();

            path = Path.Combine(di.FullName, area + "/data/Data_Common_Country.xml");

            var xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(path);

            var xDoc = xmlDocument.ToXDocument();

            if (xDoc == null && xDoc.Descendants("option").Any())
                return Json(new object { });

            //ID - returnera det engelska värdet
            //Name = returnera det översatta värdet

            var options = xDoc.Descendants("option").Where(x => x.Attribute("value") != null
                && x.Attribute("value").Value.ToUpper().StartsWith(query.ToUpper())).Select(x => new { Id = x.Value, Name = model.FormModel.Translate(x.Value) }).ToArray().OrderBy(x => x.Name);

            return Json(options, JsonRequestBehavior.AllowGet);
        }
     


        public string RenderRazorViewToString(string viewName, object model, bool partial = true)
        {
            var viewResult = partial ? ViewEngines.Engines.FindPartialView(ControllerContext, viewName) : ViewEngines.Engines.FindView(ControllerContext, viewName, null);

            if(viewResult == null || (viewResult != null && viewResult.View == null))
                throw new FileNotFoundException("View could not be found");

            ViewData.Model = model;
            using(var sw = new StringWriter())
            {
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            var rd = filterContext.HttpContext.Request.RequestContext.RouteData;
            mainXmlPath = rd.GetRequiredString("controller");

            if (rd.DataTokens["area"] != null)
                mainXmlPath = rd.DataTokens["area"] + "/" + mainXmlPath + ".xml";
            else
                mainXmlPath = mainXmlPath + ".xml";

            // Hook up so we fetch xmls from share or area specified
            //mainXmlPath = Path.Combine(@"C:\Temp\ect\FormLibContent\Xmls",mainXmlPath);

            if (!string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Request.QueryString["clearsession"]))
                Session.Clear();

            if (!string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Request.QueryString["userId"]))
                FormLibSessions.User = null;

            /*New authorize*/
            if (!FormLibUtils.IsSelfService())
            {
                if (FormLibSessions.User == null)
                {
                    var identity = User.Identity.Name;
                    var queryStringUserId = filterContext.RequestContext.HttpContext.Request.QueryString["userId"];

                    var user = _userRepository.Get(identity, !string.IsNullOrEmpty(queryStringUserId) ? queryStringUserId : null);

                    if (user != null)
                        FormLibSessions.User = user;
                    else
                        throw new HttpException(401, "Unauthorized access");
                }
            }
            else
            {
                //If we self service assume that the user is a line manager...

                if (FormLibSessions.CurrentUserIdentity == null && !string.IsNullOrEmpty(FormLibSessions.CurrentUserIdentity.EmployeeNumber))
                {
                    FormLibSessions.UserHasAccess = false;
                    filterContext.Result = new RedirectResult(Url.Action("Index", "Error", new { area = "", message = "You don't have access to the portal! ", errorCode = 401 }));
                }

                // this is set from selfservice...
                if (FormLibSessions.CustomerId > 0)
                {
                    var config = (EForm.FormLib.Configurable.AccessManagment)System.Configuration.ConfigurationManager.GetSection("formLibConfigurable/accessManagment");

                    var country = config.Countries.Where(x => x.HelpdeskCustomerId == FormLibSessions.CustomerId.ToString()).FirstOrDefault();

                    if (country == null || !FormLibSessions.CurrentUserIdentity.EmployeeNumber.StartsWith(country.EmployeePrefix))
                    {
                        FormLibSessions.UserHasAccess = false;
                        FormLibSessions.CurrentCoWorkers = null;
                        filterContext.Result = new RedirectResult(Url.Action("Index", "Error", new { area = "", message = "You don't have access to the portal! (user is not manager for country)", errorCode = 103 }));
                    }
                }

                if (FormLibSessions.User == null)
                {
                    var user = new Model.Entities.User();

                    regUserId = FormLibSessions.CurrentUserIdentity.UserId;

                    FormLibSessions.CurrentSystemUser = regUserId;
                    user.FirstName = FormLibSessions.CurrentUserIdentity.FirstName;
                    user.Surname = FormLibSessions.CurrentUserIdentity.LastName;

                    user.Id = 0;
                    user.RegUserId = regUserId;
                    user.WorkingGroups = new List<Model.Entities.WorkingGroup>();
                    user.WorkingGroups.Add(new Model.Entities.WorkingGroup { Id = -1, Name = "Line Manager" });
                    FormLibSessions.User = user;
                }
            }

            //if (!FormLibUtils.IsSelfService())
            //{
            //    if(!string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Request.QueryString["userId"]))
            //        FormLibSessions.User = null;

            //    if(FormLibSessions.User == null)
            //    {
            //        var identity = User.Identity.Name;
            //        var queryStringUserId = filterContext.RequestContext.HttpContext.Request.QueryString["userId"];

            //        var user = _userRepository.Get(identity, !string.IsNullOrEmpty(queryStringUserId) ? queryStringUserId : null);

            //        if(user != null)
            //            FormLibSessions.User = user;
            //        else
            //            throw new HttpException(401, "Unauthorized access");
            //    }
            //}
            //else
            //{
            //    // If we self service assume that the user is a line manager...

            //    if(FormLibSessions.CurrentUserIdentity == null && !string.IsNullOrEmpty(FormLibSessions.CurrentUserIdentity.EmployeeNumber))
            //    {
            //        FormLibSessions.UserHasAccess = false;
            //        filterContext.Result = new RedirectResult(Url.Action("Index", "Error", new { area = "", message = "You don't have access to the portal! ", errorCode = 401 }));
            //    }

            //    // this is set from selfservice...
            //    if(FormLibSessions.CustomerId > 0)
            //    {
            //        var config = (DH.Helpdesk.EForm.FormLib.Configurable.AccessManagment)System.Configuration.ConfigurationManager.GetSection("formLibConfigurable/accessManagment");

            //        var country = config.Countries.Where(x => x.HelpdeskCustomerId == FormLibSessions.CustomerId.ToString()).FirstOrDefault();

            //        if(country == null || !FormLibSessions.CurrentUserIdentity.EmployeeNumber.StartsWith(country.EmployeePrefix))
            //        {
            //            FormLibSessions.UserHasAccess = false;
            //            FormLibSessions.CurrentCoWorkers = null;
            //            filterContext.Result = new RedirectResult(Url.Action("Index", "Error", new { area = "", message = "You don't have access to the portal! (user is not manager for country)", errorCode = 103 }));
            //        }
            //    }

            //    if(FormLibSessions.User == null)
            //    {
            //        var user = new Model.Entities.User();

            //        regUserId = FormLibSessions.CurrentUserIdentity.UserId;

            //        FormLibSessions.CurrentSystemUser = regUserId;
            //        user.FirstName = FormLibSessions.CurrentUserIdentity.FirstName;
            //        user.Surname = FormLibSessions.CurrentUserIdentity.LastName;

            //        user.Id = 0;
            //        user.RegUserId = regUserId;
            //        user.WorkingGroups = new List<Model.Entities.WorkingGroup>();
            //        user.WorkingGroups.Add(new Model.Entities.WorkingGroup { Id = -1, Name = "Line Manager" });
            //        FormLibSessions.User = user;
            //    }
            //}

            base.OnAuthorization(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Request.QueryString["clearcache"]))
            {
                var cache = new CacheProvider();
                cache.InvalidateAll();
            }

            if(!string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Request.QueryString["customerId"]))
            {
                int i = 0;
                if(int.TryParse(filterContext.RequestContext.HttpContext.Request.QueryString["customerId"], out i))
                    FormLibSessions.CustomerId = i;
            }

            if(!string.IsNullOrEmpty(filterContext.RequestContext.HttpContext.Request.QueryString["language"]) && FormLibSessions.User != null)
                FormLibSessions.User.Language = filterContext.RequestContext.HttpContext.Request.QueryString["language"].ToLower();

            uploadPath = Server.MapPath("~/App_Data/FormLibUploads");
            base.OnActionExecuting(filterContext);
        }
    }
}
