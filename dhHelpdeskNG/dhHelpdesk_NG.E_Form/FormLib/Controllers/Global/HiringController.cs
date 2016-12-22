using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Controllers;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.FormLib.Areas.Global.Controllers
{
    public class HiringController : FormLibBaseController
    {
        public const string xmlPath = "global/hiring.xml";

        private readonly IContractRepository _contractRepository;
        private readonly IFileService _fileService;

        public HiringController(IContractRepository contractRepository 
            , IFileService fileService
            , IUserRepository userRepository)
            : base(userRepository, contractRepository, fileService)
        {
            _fileService = fileService;
            _contractRepository = contractRepository;
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

            //dictionary["WatchDate"] = _contractRepository.GetWatchDate(model.CustomerId, formCollection["ContractStartDate"], 3, 1);

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
                        model.ReplaceNode("state", "global/templates/hiringbasiconestate.xml");
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
                model.ReplaceNode("state", "global/templates/hiringbasiconestate.xml");
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
            PopulateWithOptions(ref model);

            if (model.Contract.ChildCaseNumbers.FirstOrDefault() != 0)
            {
                model.ReplaceNode("state", "global/templates/hiringbasiconestate.xml");
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

            //dictionary["WatchDate"] = _contractRepository.GetWatchDate(model.CustomerId, formCollection["ContractStartDate"], 3, 1);

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
                CancelCase = !FormLibUtils.IsSelfService() 
                    && FormLibSessions.User.WorkingGroups.FirstOrDefault(x => x.Id == model.Contract.WorkingGroupId) == null
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
