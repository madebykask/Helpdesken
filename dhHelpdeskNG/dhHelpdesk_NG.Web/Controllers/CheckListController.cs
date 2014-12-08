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

            var clistsName = this._CheckListsService.GetChecklists(SessionFacade.CurrentCustomer.Id).ToList();
            var inputCheckLists = clistsName.Select(r => new ChecklistInputModel
                      (r.Id,
                        0,
                          r.ChecklistName,
                          wgroups,
                          r.CreatedDate,
                          r.ChangedDate,
                          null
                      )).ToList();

            return this.View(new ChecklistInputModel()
            { ChangedDate = DateTime.Now, CreatedDate = DateTime.Now,
              WorkingGroups = Customer_User_WorkingGroups,
              ListOfServices = new CheckListserviceModel()
            });
        }

        [HttpPost]
        public ActionResult New(ChecklistInputModel checklist)
        {
             

            if (this.ModelState.IsValid)
            {
                var new_CheckList = new CheckListBM
                (
                    SessionFacade.CurrentCustomer.Id,
                    checklist.CheckListId,
                    checklist.WGId,
                    checklist.CheckListName,
                    checklist.CreatedDate,
                    DateTime.Now
               );

                this._CheckListsService.NewChecklist(new_CheckList);                

                return this.RedirectToAction("index", "checklist");


            }

            return this.View(checklist);
        }

        private CheckListIndexViewModel IndexViewModel()
        {
            var model = new CheckListIndexViewModel();

            return model;
        }

        private CheckListIndexViewModel CreateCheckListInputModel()
        {

            var allCheckLists = this._CheckListServiceService.GetChecklistServices(SessionFacade.CurrentCustomer.Id).ToList();
            
            
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

            var inputCheckLists = clistsName.Select(r => new ChecklistInputModel
                      (   r.Id,
                          0,
                          r.ChecklistName,
                          wgroups,
                          r.CreatedDate,
                          r.ChangedDate,
                          null                        
                      )).ToList();

            var model = new CheckListIndexViewModel
            {

                WorkingGroups = workingGroups.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),

                CheckListsList = inputCheckLists,

                ListOfExistances = clistsName.Select(x => new SelectListItem
                {
                    Text = x.ChecklistName,
                    Value = x.Id.ToString()
                }).ToList(),

                From = DateTime.Now,
                To = DateTime.Now,
           
            };

            
            return model;
        }

    }
}
