using DH.Helpdesk.BusinessData.Models.Checklists.Output;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Models.CheckLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Controllers
{
    public class CheckListController : BaseController
    {
        
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _WorkingGroupService;
        private readonly ICheckListsService _CheckListsService;
        private readonly ICheckListServiceService _CheckListServiceService;
        private readonly IChecklistActionService _CheckListActionService;


        public CheckListController(
            IUserService userService,
            ICheckListsService checkListsService,
            IWorkingGroupService workingGroupService,
            ICheckListServiceService checkListServiceService,            
            IChecklistActionService checkActionService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {            
            this._userService = userService;
            this._CheckListsService = checkListsService;
            this._WorkingGroupService = workingGroupService;
            this._CheckListServiceService = checkListServiceService;
            this._CheckListActionService = checkActionService;
        }

        public ActionResult Index()
        {
            var model = CreateCheckListInputModel();

            return this.View(model);

        }

        public ActionResult New()
        {
            var model = new CheckListInputModel();

            model.CheckListId = 0;
            model.WGId = null;
            model.CheckListName = "";
            
            var workingGroups = this._WorkingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);
            
            
            var wgs = workingGroups.Select(x => new SelectListItem
            {
                Text = x.WorkingGroupName,
                Value = x.Id.ToString()
            }).ToList();

            wgs.Insert(0,new SelectListItem {Text = "", Value = null, Selected=true});
            
            model.WorkingGroups = wgs;            

            return this.View(model);
            
        }

        [HttpPost]
        public ActionResult New(CheckListInputModel checklist)
        {
            var model = this.SaveCheckList(checklist);

            return this.RedirectToAction("Edit", "CheckList", new { id = model.CheckListId }); 
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var chL = _CheckListsService.GetChecklist(id);
            var model = CreateCheckListInput(chL);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CheckListInputModel checkList)
        {
            var model = this.UpdateCheckList(checkList);
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveAction(int serviceId, int checkListId, string actionName)
        {
            var newAction = new CheckListActionBM(0, serviceId, 1, actionName, DateTime.Now, DateTime.Now);

            _CheckListActionService.SaveCheckListAction(newAction);

            return Json(Url.Action("Edit", "CheckList", new { id = checkListId }));
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var canDelete = HasServices(id);

            if (canDelete)
            {
                this._CheckListsService.DeleteCheckListByID(id);
                return this.RedirectToAction("Index", "CheckList");
            }            

            this.TempData.Add("Error", string.Empty);
            return this.RedirectToAction("Edit", "CheckList", new { id = id });


        }

        [HttpPost]
        public ActionResult DeleteAction(int id)
        {
            var checkListId = _CheckListActionService.GetCheckListIdByAction(id);
            _CheckListActionService.DeleteActionByID(id);

            return this.RedirectToAction("Edit", "CheckList", new { id = checkListId });
        }
               
                      
        private CheckListIndexViewModel IndexViewModel()
        {
            var model = new CheckListIndexViewModel();

            return model;
        }

        private CheckListInputModel CreateCheckListInput(CheckListBM checklist)
        {
            var workingGroups = this._WorkingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);
            var chLServices = this._CheckListServiceService.GetCheckListServices(checklist.Id).ToList();

            var wgs = workingGroups.Select(x => new SelectListItem
            {
                Selected = (x.Id == checklist.WorkingGroupId),
                Text = x.WorkingGroupName,
                Value = x.Id.ToString()
            }).ToList();

            wgs.Insert(0, new SelectListItem { Text = "", Value = null, Selected = true });

            var sL = chLServices.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            sL.Insert(0, new SelectListItem { Text = "", Value = "0", Selected = true });

            var selService = chLServices.FirstOrDefault();
            var newCheckList = new CheckListInputModel
            {
                CheckListId = checklist.Id,
                WGId = checklist.WorkingGroupId,
                CheckListName = checklist.ChecklistName,
                WorkingGroups = wgs,
                Services = new List<CheckListServiceModel>(),
                ServicesList = sL,
                SelectedServiceId = selService?.Id ?? 0
            };

            foreach (var serviceBm in chLServices)
            {
                var service = new CheckListServiceModel
                {
                    IsActive = serviceBm.IsActive,
                    CheckList_Id = serviceBm.CheckListId,
                    ServiceName = serviceBm.Name,
                    Id = serviceBm.Id,
                    ActionsList = new List<CheckListActionsInputModel>()
                };
                var actions = _CheckListActionService.GetActions(serviceBm.Id)
                    .Select(x => new CheckListActionsInputModel
                    {
                        Service_Id = serviceBm.Id,
                        ActionName = x.ActionName,
                        Action_Id = x.Id,
                        IsActive = x.IsActive,
                        CreatedDate = x.CreatedDate,
                        ChangedDate = x.ChangedDate
                    }).ToList();
                if (actions.Count > 0)
                {
                    service.ActionsList.AddRange(actions);
                }
                else
                {
                    var newAction = CreateActionInput(service.Id);
                    service.ActionsList.Add(newAction);
                }
                newCheckList.Services.Add(service);
            }

            return newCheckList;
        }

        // functions for CheckLists
        private CheckListIndexViewModel CreateCheckListInputModel()
        {
            var clistsName = this._CheckListsService.GetChecklists(SessionFacade.CurrentCustomer.Id).ToList();
            var workingGroups = this._WorkingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);
            var checkListsNames = clistsName.Select(x => new SelectListItem
                                    {
                                        Text = x.ChecklistName,
                                        Value = x.Id.ToString()
                                    }).OrderBy(x => x.Text).ToList();
            checkListsNames.Insert(0, new SelectListItem { Text = "", Value = "0", Selected = true });

            var model = new CheckListIndexViewModel
            {
                WorkingGroups = workingGroups.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                ListOfExistances = checkListsNames,
                From = DateTime.Now,
                To = DateTime.Now
            };

            
            return model;
        }

        private CheckListInputModel SaveCheckList(CheckListInputModel checklist)
        {

            if (checklist.CheckListName != null)
            {
                var new_CheckList = new CheckListBM
                (
                    checklist.CheckListId,
                    SessionFacade.CurrentCustomer.Id,
                    checklist.WGId,
                    checklist.CheckListName,
                    DateTime.Now,
                    DateTime.Now
               );

                if (checklist.CheckListId > 0)
                    new_CheckList.Id = checklist.CheckListId;

                this._CheckListsService.SaveCheckList(new_CheckList);

                var workingGroups = this._WorkingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);


                var wgs = workingGroups.Select(x => new SelectListItem
                {
                    Selected = (x.Id == checklist.WGId ? true : false),
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList();

                wgs.Insert(0, new SelectListItem { Text = "", Value = "0", Selected = true });


                var model = new CheckListInputModel()
                {
                    CheckListId = new_CheckList.Id,
                    CheckListName = new_CheckList.ChecklistName,
                    WGId = new_CheckList.WorkingGroupId,
                    WorkingGroups = wgs
                };

                return model;
            }

            return checklist;
        }

        private CheckListInputModel UpdateCheckList(CheckListInputModel checklist)
        {
            var chkLstBm = new CheckListBM(checklist.CheckListId, SessionFacade.CurrentCustomer.Id, checklist.WGId, checklist.CheckListName, DateTime.Now, DateTime.Now);
            _CheckListsService.UpdateCheckList(chkLstBm);

            foreach (var sm in checklist.Services)
            {
                var chkServiceBm = new CheckListServiceBM(SessionFacade.CurrentCustomer.Id, sm.CheckList_Id, sm.Id, sm.IsActive, sm.ServiceName, DateTime.Now, DateTime.Now);
                _CheckListServiceService.UpdateCheckListService(chkServiceBm);

                foreach (var am in sm.ActionsList)
                {
                    var chkActionBm = new CheckListActionBM(am.Action_Id, am.Service_Id, am.IsActive, am.ActionName, DateTime.Now, DateTime.Now);
                    if (chkActionBm.Id > 0)
                    {
                        _CheckListActionService.UpdateAction(chkActionBm);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(chkActionBm.ActionName))
                            _CheckListActionService.SaveCheckListAction(chkActionBm);
                    }
                }
            }
            _CheckListsService.CommitChanges();
            return CreateCheckListInput(chkLstBm);
        }

        private CheckListActionsInputModel CreateActionInput(int serviceId)
        {
            var newAction = new CheckListActionsInputModel()
            {
                Action_Id = 0,
                Service_Id = serviceId,
                ActionName = string.Empty,
                IsActive = 1,
                CreatedDate = DateTime.Now,
                ChangedDate = DateTime.Now
            };

            return newAction;
        }

        [HttpPost]
        public ActionResult AddService(string serviceName, int checkListId)
        {
            var newService = new CheckListServiceBM(SessionFacade.CurrentCustomer.Id, checkListId, 0, 1, serviceName, DateTime.Now, DateTime.Now);

            _CheckListServiceService.SaveCheckListService(newService);

            return Json(Url.Action("Edit", "CheckList", new { id = checkListId }));
        }

        private bool HasServices(int checkListId)
        {
            var checkListServices = _CheckListServiceService.GetCheckListServices(checkListId);
            return checkListServices == null;
        }
    }
}
