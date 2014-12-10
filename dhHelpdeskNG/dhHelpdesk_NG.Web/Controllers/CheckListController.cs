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
        private readonly IChecklistsService _CheckListsService;
        private readonly IChecklistServiceService _CheckListServiceService;
        private readonly IChecklistActionService _CheckListActionService;


        public CheckListController(
            IUserService userService,
            IChecklistsService checkListsService,
            IWorkingGroupService workingGroupService,
            IChecklistServiceService checkListServiceService,            
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

            //return View();
        }

        
        //public PartialViewResult getSelectedCheckList(int id)
        //{
        //    var chLService = this._CheckListServiceService.GetChecklistServiceByCheckListID(id, SessionFacade.CurrentCustomer.Id).ToList();

        //    CheckListserviceModel checkListS = null;

        //    foreach (var servicce in chLService)
        //    {
        //        checkListS = new CheckListserviceModel()
        //        {
        //            Customer_Id = SessionFacade.CurrentCustomer.Id,
        //            Service_Id = servicce.Id,
        //            IsActive = servicce.IsActive,
        //            ServiceName = servicce.Name,
        //            CreatedDate = servicce.CreatedDate,
        //            ChangedDate = servicce.ChangedDate
        //        };
        //    }
        //    return this.PartialView("SelectedCheckList", checkListS);          
        //}


        public ActionResult New()
        {
            var model = new ChecklistInputModel();

            model.CheckListId = 0;
            model.WGId = null;
            model.CheckListName = "";
            
            var workingGroups = this._WorkingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);
            
            
            var wgs = workingGroups.Select(x => new SelectListItem
            {
                Text = x.WorkingGroupName,
                Value = x.Id.ToString()
            }).ToList();

            wgs.Insert(0,new SelectListItem {Text = "", Value = "0", Selected=true});
            
            model.WorkingGroups = wgs;

            return this.View(model);
            
        }

        /*public ActionResult Edit(int checkListId)
        {
            var model = new ChecklistInputModel();

            var checkList = _CheckListsService.GetChecklist();
            model.CheckListId = checkListId;
            model.WGId = null;
            model.CheckListName = "";

            var workingGroups = this._WorkingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);


            var wgs = workingGroups.Select(x => new SelectListItem
            {
                Text = x.WorkingGroupName,
                Value = x.Id.ToString()
            }).ToList();

            wgs.Insert(0, new SelectListItem { Text = "", Value = "0", Selected = true });

            model.WorkingGroups = wgs;

            return this.View(model);

        }
         * 
        /*[HttpPost]
        public PartialViewResult AddSericesAndActions(ChecklistInputModel service)
        {
            //if (service.CheckListId == 0)
            //{
            //    this.RedirectToAction("New", service);
            //}

            //var savedCheckList = this._CheckListsService.GetChecklist(SessionFacade.CurrentCustomer.Id ,service.CheckListName );

            var newService = new ChecklistServiceBM(SessionFacade.CurrentCustomer.Id, 0, service.ListOfServices.Service_Id, 1,
                                                     service.ListOfServices.ServiceName , DateTime.Now, DateTime.Now);
        
            this._CheckListServiceService.NewChecklistService(newService);

            var addedServices = this._CheckListServiceService.GetChecklistServiceByCheckListID(0, SessionFacade.CurrentCustomer.Id);
            //var servicesList = this._CheckListServiceService.GetChecklistServices(checklist.CheckListId, SessionFacade.CurrentCustomer.Id);

            var returned = new CheckListserviceModel { Customer_Id = SessionFacade.CurrentCustomer.Id,
                                                       ServiceName = service.ListOfServices.ServiceName,
                                                       ServicesList = addedServices.Select( s =>  new SelectListItem
                                                                        {
                                                                            Text = s.Name,
                                                                            Value = s.Id.ToString()
                                                                        }).ToList(),
                                                        IsActive = 1,
                                                        ChangedDate = DateTime.Now, 
                                                        CreatedDate = DateTime.Now
                                                     };

            return this.PartialView("_NewServices", returned);
             
        }
            */

        [HttpPost]
        public ActionResult New(ChecklistInputModel checklist)
        {
            var model = this.SaveCheckList(checklist);

            return this.View(model);
        }

        
        private ChecklistInputModel SaveCheckList(ChecklistInputModel checklist)
        {
           
           if (checklist.CheckListName != null)
            {
                var new_CheckList = new CheckListBM
                (
                    SessionFacade.CurrentCustomer.Id,                    
                    checklist.WGId,
                    checklist.CheckListName,
                    DateTime.Now,
                    DateTime.Now
               );

              //new_CheckList.Id = checklist.CheckListId;
              this._CheckListsService.SaveCheckList(new_CheckList);

              var returneNewModel = new ChecklistInputModel()
              {
                  CheckListId = new_CheckList.Id,
                  CheckListName = new_CheckList.ChecklistName,
                  WGId = new_CheckList.WorkingGroupId,
                  WorkingGroups = checklist.WorkingGroups                  
              };

              return returneNewModel;
            }


           return checklist;

           
        }
        
        private CheckListIndexViewModel IndexViewModel()
        {
            var model = new CheckListIndexViewModel();

            return model;
        }

        private CheckListIndexViewModel CreateCheckListInputModel()
        {
                        
            //var clistsName = this._CheckListsService.GetChecklists(SessionFacade.CurrentCustomer.Id).ToList();
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
            var model = new CheckListIndexViewModel
            {

                WorkingGroups = workingGroups.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),

                //CheckListsList = null,

                //ListOfExistances = clistsName.Select(x => new SelectListItem
                //{
                //    Text = x.ChecklistName,
                //    Value = x.Id.ToString()
                //}).ToList(),

                From = DateTime.Now,
                To = DateTime.Now,                
           
            };

            
            return model;
        }

    }
}
