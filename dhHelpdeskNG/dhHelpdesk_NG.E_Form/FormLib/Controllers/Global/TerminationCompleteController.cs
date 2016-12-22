using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Models;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using System;
using System.Collections.Generic;

namespace ECT.FormLib.Areas.Global.Controllers
{
    public class TerminationCompleteController : FormLib.Controllers.FormLibBaseController
    {
        public const string xmlPath = "global/terminationcomplete.xml";

        private readonly IContractRepository _contractRepository;
        private readonly IFileService _fileService;

        public TerminationCompleteController(IContractRepository contractRepository
             , IUserRepository userRepository
            , IFileService fileService)
            : base(userRepository)
        {
            _contractRepository = contractRepository;
            _fileService = fileService;
        }

        public override ActionResult New()
        {
            var model = FormModelFactory.InitNew(xmlPath);
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
            return View(model);
        }

        [HttpPost]
        public override ActionResult New(FormCollection formCollection, string[] uploads)
        {
            var model = FormModelFactory.InitNew(xmlPath);
            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);

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

                    model.Status = oldStatus;
                    InitSubProcess(model, actionState, caseId, dictionary);
                    model.Status = newStatus;

                    if (model.Contract.ChildCaseNumbers.FirstOrDefault() != 0)
                    {
                        model.ReplaceNode("state", "global/templates/terminationbasiconestate.xml");
                    }

                    return Json(new
                    {
                        View = RenderRazorViewToString("_Edit", model),
                        CancelCase = FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
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
            if (model.Contract.ChildCaseNumbers.FirstOrDefault() != 0)
            {
                model.ReplaceNode("state", "global/templates/terminationbasiconestate.xml");
            }

            model.PopulateForm(_contractRepository.GetFormDictionary(id, model.FormGuid));

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
            if (model.Contract.ChildCaseNumbers.FirstOrDefault() != 0)
            {
                model.ReplaceNode("state", "global/templates/terminationbasiconestate.xml");
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

            _fileService.SaveUploads(uploads, model.Contract.Id, uploadPath, Session.SessionID);

            if (!Request.IsAjaxRequest())
                return View(model);

            var view = string.Empty;

            if (model.GetErrorMessages().Any())
                view = RenderRazorViewToString("_Edit", model);

            return Json(new
            {
                View = view,
                CancelCase = FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
            });
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
