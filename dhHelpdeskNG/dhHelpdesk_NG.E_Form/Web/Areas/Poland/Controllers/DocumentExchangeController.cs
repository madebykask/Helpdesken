using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.FileStore;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using ECT.Web.Controllers;
using ECT.Web.Models;

namespace ECT.Web.Areas.Poland.Controllers
{
    public class DocumentExchangeController : BaseController
    {
        private readonly IContractRepository _contractRepository;

        public DocumentExchangeController(IContractRepository contractRepository
             , IUserRepository userRepository)
            : base(userRepository)
        {
            _contractRepository = contractRepository;
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

            var companies = _contractRepository.GetCompanies(model.CustomerId).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId).ToList();
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

            var companies = _contractRepository.GetCompanies(model.CustomerId).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId).ToList();
            if(units.Any())
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

            var dictionary = formCollection.ToDictionary();
            model.TempSave(ref dictionary);

            var globalViewSearch = _contractRepository.GlobalViewSearch(formCollection["EmployeeNumber"]);
            var globalViews = globalViewSearch as GlobalView[] ?? globalViewSearch.ToArray();
            if(!globalViews.Any() || globalViews.Count() != 1)
            {
                model.AddError("EmployeeNumber", "Please select a valid employee no.");
            }

            if(model.IsValid(ref dictionary, 10))
            {
                var employee = globalViews.FirstOrDefault();
                if(employee != null)
                {
                    var caseId = _contractRepository.Save(model.FormId
                        , 0
                        , SessionFacade.User.Id
                        , int.Parse(formCollection["Company"])
                        , int.Parse(formCollection["Unit"])
                        , employee.Name
                        , employee.Surname
                        , null
                        , dictionary);

                    if(uploads != null)
                    {
                        var contract = _contractRepository.Get(caseId);
                        var globalSettings = _contractRepository.GetGlobalSettings();
                        var folder = globalSettings.AttachedFileFolder;
                        var path = Path.Combine(folder, contract.CaseNumber);
                        var uploadPath = Server.MapPath("~/App_Data/Uploads");

                        foreach(var file in uploads)
                        {
                            var sourcePath = Path.Combine(uploadPath, Session.SessionID, file);
                            var destinationPath = Path.Combine(path, file);

                            if(FileStore.Move(sourcePath, destinationPath))
                            {
                                _contractRepository.SaveCaseFile(new CaseFile
                                {
                                    CaseId = caseId,
                                    FileName = file
                                });
                            }
                        }
                    }

                    return RedirectToAction("edit", new { id = caseId });
                }
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
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"]),
                Contract = contract,
                Status = contract.StateSecondaryId
            };

            model.PopulateForm(_contractRepository.GetFormDictionary(id, model.FormId));

            var companies = _contractRepository.GetCompanies(model.CustomerId).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId).ToList();
            if(units.Any())
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

            if(contract.Company != null)
                model.SetAnswer("Company", contract.Company.Id.ToString(CultureInfo.InvariantCulture));
            if(contract.Unit != null)
                model.SetAnswer("Unit", contract.Unit.Id.ToString(CultureInfo.InvariantCulture));

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
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"]),
                Contract = contract,
                Status = contract.StateSecondaryId
            };

            var companies = _contractRepository.GetCompanies(model.CustomerId).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId).ToList();
            if(units.Any())
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());

            var dictionary = formCollection.ToDictionary();

            model.TempSave(ref dictionary);
            model.ActiveTab = formCollection["activeTab"];

            _contractRepository.Save(model.FormId, model.Contract.Id, dictionary);

            if(uploads != null)
            {
                var globalSettings = _contractRepository.GetGlobalSettings();
                var folder = globalSettings.AttachedFileFolder;
                var path = Path.Combine(folder, contract.CaseNumber);
                var uploadPath = Server.MapPath("~/App_Data/Uploads");

                foreach(var file in uploads)
                {
                    var sourcePath = Path.Combine(uploadPath, Session.SessionID, file);
                    var destinationPath = Path.Combine(path, file);

                    if(FileStore.Move(sourcePath, destinationPath))
                    {
                        _contractRepository.SaveCaseFile(new CaseFile
                        {
                            CaseId = id,
                            FileName = file
                        });
                    }
                }
            }

            var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
            var actionState = formCollection["actionState"];
            model.ActiveStatus = actionState;

            if(actionStateChange && !string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
            {
                model.Status = int.Parse(actionState);
                _contractRepository.UpdateCaseStateSecondary(model.Contract.Id, model.Status);
                model.Contract = _contractRepository.Get(id);
            }

            if(!Request.IsAjaxRequest())
                return View(model);

            var view = string.Empty;

            if(model.GetErrorMessages().Any())
                view = RenderRazorViewToString("_Edit", model);

            return Json(new
            {
                View = view,
                CancelCase = SessionFacade.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
            });
        }
    }
}
