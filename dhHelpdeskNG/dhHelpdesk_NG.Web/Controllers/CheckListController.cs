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

        
        //
        // GET: /CheckList/

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
            var chL = this._CheckListsService.GetChecklist(id);

            var chLServices = this._CheckListServiceService.GetCheckListServices(id).ToList();

            var model = this.CreatcheckListInput(chL);

            model.Services = new CheckListServiceModel
            {
                CheckList_Id = id,
                Service_Id = 0,
                SId = null,
                IsActive = 1,
                ServiceName = string.Empty
            };

            var sL = chLServices.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            //sL.Insert(0, new SelectListItem { Text = "", Value = "0", Selected = true });
            model.Services.ServicesList = sL;
            model.Services.ActionsList = new List<CheckListActionsInputModel>();

            foreach (var s in chLServices)
            {
                var allActions = this._CheckListActionService.GetActions(s.Id).ToList();
                var actionsInput = allActions.Select(a => new CheckListActionsInputModel
                    (
                    a.Service_Id,
                    a.Id,
                    a.IsActive,
                    a.ActionName,
                    a.CreatedDate,
                    a.ChangedDate
                    )).ToList();

                foreach (var checkListActionsInputModel in actionsInput)
                {
                    model.Services.ActionsList.Add(checkListActionsInputModel);
                }

                if (actionsInput.Count == 0)
                {
                    var newAction = CreatActionInput(s.Id);
                    model.Services.ActionsList.Add(newAction);
                }
            }

            return this.View(model);
        }


        [HttpPost]
        public ActionResult Edit(int id, CheckListInputModel checkList, CheckListServiceModel CLService, CheckListActionsInputModel CLAction)
        {
            var model = this.UpdateCheckList(checkList);

            if (CLService != null)
            {
                CLService.CheckList_Id = checkList.CheckListId;
                model.Services = this.SaveCheckListService(CLService);

                var chLServices = this._CheckListServiceService.GetCheckListServices(id).ToList();

                var sL = chLServices.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

                sL.Insert(0, new SelectListItem { Text = "", Value = "0", Selected = true });

                model.Services.ServicesList = sL;
                model.Services.SId = CLService.Service_Id;


                var allActions = this._CheckListActionService.GetActions(model.Services.Service_Id).ToList();

                var actionsInput = allActions.Select(a => new CheckListActionsInputModel
                     (
                         a.Service_Id,
                         a.Id,
                         a.IsActive,
                         a.ActionName,
                         a.CreatedDate,
                         a.ChangedDate
                     )).ToList();

                model.Services.ActionsList = actionsInput;

                if (allActions.Count == 0)
                {
                    var New_action = CreatActionInput(CLService.Service_Id);
                    //if (New_action.ActionName != null)
                    //    this.SaveAction(selectedServiceId, CLService, New_action);
                    model.Services.ActionsList.Insert(0, New_action);
                }

            }
            return this.View(model);
        }



        [HttpPost]
        public ActionResult SaveAction(int serviceId, int checkListId, string actionName)
        {
            var newAction = new CheckListActionBM(0, serviceId, 1, actionName, DateTime.Now, DateTime.Now);

            _CheckListActionService.SaveCheckListAction(newAction);

            return Json(Url.Action("Edit", "CheckList", new { id = checkListId }));
        }


        //private CheckListActionsInputModel SaveAction(int selectedServiceId, CheckListServiceModel ch_Service, CheckListActionsInputModel cL_Action)
        //{

        //        var new_Action = new CheckListActionBM
        //        (
        //            ch_Service.Service_Id,
        //            cL_Action.IsActive,
        //            ch_Service.ActionInput,
        //            DateTime.Now,
        //            DateTime.Now
        //       );


        //    this._CheckListActionService.SaveCheckListAction(new_Action);


        //    return cL_Action;
        //}


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
            
            this._CheckListActionService.DeleteActionByID(id);
                           
            return this.RedirectToAction("Edit", "CheckList", new { id = id });
        }
               
                      
        private CheckListIndexViewModel IndexViewModel()
        {
            var model = new CheckListIndexViewModel();

            return model;
        }

        private CheckListInputModel CreatcheckListInput(CheckListBM checklist)
        {

            var workingGroups = this._WorkingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);
            var chLServices = this._CheckListServiceService.GetCheckListServices(checklist.Id).ToList();

//            var new_Service = (dynamic)null;
            var checkListActions = new List<CheckListActionBM>();

//            if (chLServices != null)
//            {
                foreach (var s in chLServices)
                {
                    checkListActions = this._CheckListActionService.GetActions(s.Id).ToList();
                }

//                new_Service = chLServices.Select(s => new CheckListServiceModel()).ToList();
//            }
            //else
            //    new_Service = new CheckListServiceModel(checklist.Id, 0, 1, "");

            var wgs = workingGroups.Select(x => new SelectListItem
            {
                Selected = (x.Id == checklist.WorkingGroupId),
                Text = x.WorkingGroupName,
                Value = x.Id.ToString()
            }).ToList();

            wgs.Insert(0, new SelectListItem { Text = "", Value = null, Selected = true });


            var new_CheckList = new CheckListInputModel
            {
                CheckListId = checklist.Id,
                WGId = checklist.WorkingGroupId,
                CheckListName = checklist.ChecklistName,
                WorkingGroups = wgs,
                CheckListActions = checkListActions
                //,
                //Services = new_Service
            };


            return new_CheckList;

        }

        // functions for CheckLists
        private CheckListIndexViewModel CreateCheckListInputModel()
        {
                        
            var clistsName = this._CheckListsService.GetChecklists(SessionFacade.CurrentCustomer.Id).ToList();
            //var checklistDates = this._CheckListService.GetChecklistDates(SessionFacade.CurrentCustomer.Id).ToList();
            var workingGroups = this._WorkingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);

            var Customer_User_WorkingGroups = workingGroups.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList();

            var wgroups = workingGroups.Select(wg => new workingGroupMapper
                    (wg.Id,
                     wg.WorkingGroupName
                    )).ToList();

            /*var inputCheckLists = clistsName.Select(r => new ChecklistInputModel
                      (   r.Id,
                          0,
                          r.ChecklistName,
                          wgroups,
                          new  CheckListserviceModel()                       
                      )).ToList();
            */
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
                
                //CheckListsList = null,

                ListOfExistances = checkListsNames,
                 

                From = DateTime.Now,
                To = DateTime.Now,                
           
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
            var update_CheckList = new CheckListBM
            (
                checklist.CheckListId,
                SessionFacade.CurrentCustomer.Id,
                checklist.WGId,
                checklist.CheckListName,
                DateTime.Now,
                DateTime.Now
           ) { Id = checklist.CheckListId };

            this._CheckListsService.UpdateCheckList(update_CheckList);

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
                CheckListId = update_CheckList.Id,
                CheckListName = update_CheckList.ChecklistName,
                WGId = update_CheckList.WorkingGroupId,
                WorkingGroups = wgs
            };

            return model;

        }


        //Functions for Check Lists Services

        private CheckListsModel CreateCheckListsModel(int checkList_id)
        {
            var model = new CheckListsModel();

            var allServices = this._CheckListServiceService.GetCheckListServices(checkList_id).ToList();

            //var allActions = this._CheckListActionService.


            //var inputModel = allServices.Select(s => new CheckListServiceModel
            //          (
            //              s.CheckListId,
            //              s.Id,
            //              s.Id,
            //              s.IsActive,
            //              s.Name,
            //              allServices
            //          )).ToList();

            //model.InputServices = inputModel;

            //var emptyChoice = new CheckListServiceBM(SessionFacade.CurrentCustomer.Id, 0, 1, "", DateTime.Now, DateTime.Now);
            //allServices.Insert(0, emptyChoice);

            //var newInput = new CheckListServiceModel(0, 0, 0, 1, " ", allServices);
                                                       
            //model.InputServices.Insert(0, newInput);

            //model.InputActions = new CheckListActionsInputModel();

            return model;
        }

        //Functions for Services actions

        private CheckListActionsInputModel CreatActionInput(int service_Id) //, List<CheckListServiceBM> checkListServices
        {

            var new_Action = new CheckListActionsInputModel()
            {
                Action_Id = 0,
                Service_Id = service_Id,
                ActionName = "",
                IsActive = 1,
                CreatedDate = DateTime.Now,
                ChangedDate = DateTime.Now   
            };


            return new_Action;

        }

        //private CheckListsModel CreateCheckListsActionsModel(int checklist_id ,int service_id)
        //{
        //    var model = new CheckListsModel();

        //    var allServices = this._CheckListServiceService.GetCheckListServices(checklist_id).ToList();

        //    var allActions = this._CheckListActionService.GetActions(service_id).ToList();

        //    var sL = allServices.Select(x => new SelectListItem
        //    {
        //        Text = x.Name,
        //        Value = x.Id.ToString()
        //    }).ToList();

        //    sL.Insert(0, new SelectListItem { Text = "", Value = "0", Selected = true });

        //     var actionsInput = allActions.Select(a => new CheckListActionsInputModel
        //              (
        //                  a.Service_Id,
        //                  a.Id,                
        //                  a.IsActive,
        //                  a.ActionName, 
        //                  a.CreatedDate,
        //                  a.ChangedDate
        //              )).ToList();
          

        //    var servicesInput = allServices.Select(s => new CheckListServiceModel
        //              (
        //                  s.CustomerId,
        //                  s.CheckListId,
        //                  s.Id,
        //                  s.IsActive,
        //                  s.Name,
        //                  sL,
        //                  actionsInput
        //              )).ToList();

        //    model.InputServices = servicesInput;

        //    //var emptyChoice = new CheckListServiceBM(SessionFacade.CurrentCustomer.Id, 0, 1, "", DateTime.Now, DateTime.Now);
        //    //allServices.Insert(0, emptyChoice);

        //    //var newInput = new CheckListServiceModel(0, 0, 0, 1, " ", allServices);

        //    //model.InputServices.Insert(0, newInput);

        //     model.InputActions = actionsInput;

        //    return model;
        //}

        private CheckListServiceModel SaveCheckListService(CheckListServiceModel checkListService)
        {

            if (checkListService.ServiceName != null)
            {
                var new_CheckListService = new CheckListServiceBM
                (
                    SessionFacade.CurrentCustomer.Id,
                    checkListService.CheckList_Id,
                    checkListService.Service_Id,
                    1,
                    checkListService.ServiceName,
                    DateTime.Now,
                    DateTime.Now
                );

                if (checkListService.Service_Id > 0)
                    new_CheckListService.Id = checkListService.Service_Id;

                this._CheckListServiceService.SaveCheckListService(new_CheckListService);

                var model = new CheckListServiceModel()
                {
                    CheckList_Id = new_CheckListService.CheckListId,
                    IsActive = new_CheckListService.IsActive,
                    Service_Id = new_CheckListService.Id,
                    ServiceName = new_CheckListService.Name                            
                };

                return model;
            }

            return checkListService;
        }



        [HttpPost]
        public PartialViewResult Add(CheckListInputModel checkList, CheckListServiceModel cL_Service)
        {
            return this.PartialView("_NewServices", cL_Service);     
        }

      

        private Boolean HasServices(int checkListId)
        {
            var checkListServices = this._CheckListServiceService.GetCheckListServices(checkListId);

            if (checkListServices != null)
                return false;
            else
                return true;
        }
    }
}
