using Model.Abstract;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Poland.Models;
using Web.Controllers;

namespace Web.Areas.Poland.Controllers
{
    public class MassRequestController : BaseController
    {
        private readonly IContractRepository _contractRepository;
        public MassRequestController(IContractRepository contractRepository
             , IUserRepository userRepository)
            : base(userRepository)
        {
            _contractRepository = contractRepository;
        }

        public ActionResult New()
        {
            var model = new MassRequest(XmlPath)
             {
                 Contract = new Contract { Initiator = SessionFacade.User.FirstName + " " + SessionFacade.User.Surname }
             };

            var companies = _contractRepository.GetCompanies(model.CustomerId).ToList();
            model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            if (companies.Any())
            {
                var company = companies.FirstOrDefault();
                if (company != null)
                {
                    var units = _contractRepository.GetUnits(model.CustomerId, company.Id).ToList();
                    if (units.Any())
                        model.PopulateFormElementWithOptions("Unit", units.ToDictionary());
                }
            }

            model.SetAnswer("CountryCode", "Poland");

            SessionFacade.SetModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(FormCollection formCollection, IEnumerable<HttpPostedFileBase> fileBase)
        {
            var model = SessionFacade.GetModel<MassRequest>();

            if (model == null)
                return RedirectToAction("index", "dead");

            var dictionary = formCollection.ToDictionary();
            model.TempSave(ref dictionary);

            var globalViewSearch = _contractRepository.GlobalViewSearch(formCollection["EmployeeNumber"]);
            var globalViews = globalViewSearch as GlobalView[] ?? globalViewSearch.ToArray();
            if (!globalViews.Any() || globalViews.Count() != 1)
            {
                model.AddError("EmployeeNumber", "Please select a valid employee no.");
            }

            if (model.IsValid(ref dictionary, 10))
            {
                var employee = globalViews.FirstOrDefault();
                if (employee != null)
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

                    var globalSettings = _contractRepository.GetGlobalSettings();
                    var folder = globalSettings.AttachedFileFolder;
                    folder = System.IO.Path.GetTempPath();

                    foreach (var file in fileBase)
                    {
                        var fileName = file.FileName;
                        file.SaveAs(Path.Combine(folder, fileName));

                        _contractRepository.SaveCaseFile(new CaseFile
                        {
                            CaseId = caseId,
                            FileName = fileName
                        });
                    }

                    return RedirectToAction("edit", new { id = caseId });
                }
            }

            SessionFacade.SetModel(model);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var contract = _contractRepository.Get(id);

            if (contract == null)
                throw new HttpException(404, "Page not found");

            var model = new MassRequest(XmlPath)
            {
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"]),
                Contract = contract,
                Status = contract.StateSecondaryId
            };

            model.PopulateForm(_contractRepository.GetFormDictionary(id));
            var companies = _contractRepository.GetCompanies(model.CustomerId).ToList();

            if (companies.Any())
            {
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());
                var company = companies.FirstOrDefault();
                if (company != null)
                {
                    var units = _contractRepository.GetUnits(model.CustomerId, company.Id).ToList();
                    if (units.Any())
                        model.PopulateFormElementWithOptions("Unit", units.ToDictionary());
                }
            }

            if (contract.Company != null)
                model.SetAnswer("Company", contract.Company.Id.ToString());
            if (contract.Unit != null)
                model.SetAnswer("Unit", contract.Unit.Id.ToString());

            SessionFacade.SetModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            var model = SessionFacade.GetModel<MassRequest>();

            if (model == null)
                return RedirectToAction("index", "dead");

            var dictionary = formCollection.ToDictionary();

            model.TempSave(ref dictionary);
            model.ActiveTab = formCollection["activeTab"];

            _contractRepository.Save(model.FormId, model.Contract.Id, dictionary);

            var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
            var actionState = formCollection["actionState"];

            if (actionStateChange && !string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
            {
                model.Status = int.Parse(actionState);
                _contractRepository.UpdateCaseStateSecondary(model.Contract.Id, model.Status);
                model.Contract = _contractRepository.Get(id);
            }

            SessionFacade.SetModel(model);

            if (Request.IsAjaxRequest())
            {
                var view = string.Empty;

                if (model.GetErrorMessages().Any())
                    view = RenderRazorViewToString("_Edit", model);

                return Json(new { View = view });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(string query)
        {
            var result = _contractRepository.GlobalViewSearch(query);
            return Json(result);
        }

    }
}
