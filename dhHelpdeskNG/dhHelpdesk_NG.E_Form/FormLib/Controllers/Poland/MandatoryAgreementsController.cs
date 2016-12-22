using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Controllers;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;

namespace DH.Helpdesk.EForm.FormLib.Areas.Poland.Controllers
{
    public class MandatoryAgreementsController : FormLibBaseController
    {
        public const string xmlPath = "poland/mandatoryagreements.xml";

        private readonly IContractRepository _contractRepository;
        private readonly IFileService _fileService;

        public MandatoryAgreementsController(IContractRepository contractRepository
             , IUserRepository userRepository
            , IFileService fileService)
            : base(userRepository)
        {
            _contractRepository = contractRepository;
            _fileService = fileService;
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
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
                    || (FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == contract.WorkingGroupId) == null),
                Contract = contract,
                Status = contract.StateSecondaryId
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
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
                    || (FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == contract.WorkingGroupId) == null),
                Contract = contract,
                Status = contract.StateSecondaryId
            };
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);

            PopulateWithOptions(ref model);

            var dictionary = formCollection.ToDictionary();

            model.TempSave(ref dictionary);
            model.ActiveTab = formCollection["activeTab"];

            var actionStateChange = formCollection["actionStateChange"] != null || Request.IsAjaxRequest();
            var actionState = formCollection["actionState"];
            model.ActiveStatus = actionState;

            if(actionStateChange && !string.IsNullOrEmpty(actionState) && model.IsValid(ref dictionary, int.Parse(actionState)))
            {
                var actualStatus = int.Parse(model.GetStatusActionInternalValue(actionState));
                model.Status = actualStatus;
            }

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

            if(model.GetErrorMessages().Any())
                view = RenderRazorViewToString("_Edit", model);

            return Json(new
            {
                View = view,
                CancelCase = FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
            });
        }

        new private void PopulateWithOptions(ref FormModel model)
        {
            var companies = _contractRepository.GetCompanies(model.CustomerId, FormLibSessions.User.Id).ToList();
            if(companies.Any())
                model.PopulateFormElementWithOptions("Company", companies.ToDictionary());

            var units = _contractRepository.GetUnits(model.CustomerId, FormLibSessions.User.Id, null).ToList();
            if(units.Any())
                model.PopulateFormElementWithOptions("Unit", units.ToDictionary());
        }
    }
}
