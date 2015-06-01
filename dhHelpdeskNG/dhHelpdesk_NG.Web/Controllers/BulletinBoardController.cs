using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Controllers
{    
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models;

    public class BulletinBoardController : BaseController
    {
        private readonly IBulletinBoardService _bulletinBoardService;
        private readonly IWorkingGroupService _workingGroupService;

        public BulletinBoardController(
            IBulletinBoardService bulletinBoardService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._bulletinBoardService = bulletinBoardService;
            this._workingGroupService = workingGroupService;
        }

        public ActionResult Index()
        {
            var model = this.IndexInputViewModel();

            BulletinBoardSearch CS = new BulletinBoardSearch();
            if (SessionFacade.CurrentBulletinBoardSearch != null)
            {                
                CS = SessionFacade.CurrentBulletinBoardSearch;
                model.BulletinBoards = this._bulletinBoardService.SearchAndGenerateBulletinBoard(SessionFacade.CurrentCustomer.Id, CS);
                model.SearchBbs = CS.SearchBbs;
            }
            else
            {
                model.BulletinBoards = this._bulletinBoardService.GetBulletinBoards(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.ChangedDate).ToList();
                CS.SortBy = "ChangedDate";
                CS.Ascending = true;
                SessionFacade.CurrentBulletinBoardSearch = CS;
            }

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Index(BulletinBoardSearch SearchBulletinBoards)
        {
            BulletinBoardSearch CS = new BulletinBoardSearch();
            if (SessionFacade.CurrentBulletinBoardSearch != null)
                CS = SessionFacade.CurrentBulletinBoardSearch;

            CS.SearchBbs = SearchBulletinBoards.SearchBbs;

            var bb = this._bulletinBoardService.SearchAndGenerateBulletinBoard(SessionFacade.CurrentCustomer.Id, CS);

            if (SearchBulletinBoards != null)
                SessionFacade.CurrentBulletinBoardSearch = CS;

            var model = this.IndexInputViewModel();

            model.BulletinBoards = bb;
            model.SearchBbs = CS.SearchBbs;

            return this.View(model);
        }

        //public ActionResult Sort(string FieldName)
        //{
        //    var model = this.IndexInputViewModel();
        //    BulletinBoardSearch CS = new BulletinBoardSearch();
        //    if (SessionFacade.CurrentBulletinBoardSearch!= null)
        //        CS = SessionFacade.CurrentBulletinBoardSearch;
        //    CS.Ascending = !CS.Ascending;
        //    CS.SortBy = FieldName;
        //    SessionFacade.CurrentBulletinBoardSearch = CS;
        //    return this.View(model);
        //}

        public void Sort(string fieldName)
        {
            var model = this.IndexInputViewModel();
            BulletinBoardSearch CS = new BulletinBoardSearch();
            if (SessionFacade.CurrentBulletinBoardSearch != null)
                CS = SessionFacade.CurrentBulletinBoardSearch;
            CS.Ascending = !CS.Ascending;
            CS.SortBy = fieldName;
            SessionFacade.CurrentBulletinBoardSearch = CS;

            //var model = this.IndexInputViewModel();
            //BulletinBoardSearch CS = new BulletinBoardSearch();
            //if (SessionFacade.CurrentBulletinBoardSearch != null)
            //    CS = SessionFacade.CurrentBulletinBoardSearch;
            //CS.Ascending = !CS.Ascending;
            //CS.SortBy = FieldName;
            //SessionFacade.CurrentBulletinBoardSearch = CS;
            //return this.View(model);
        }

        public ActionResult New()
        {
            var model = this.CreateInputViewModel(new BulletinBoard { Customer_Id = SessionFacade.CurrentCustomer.Id, ShowDate = DateTime.Now, ShowUntilDate = DateTime.Now });

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(BulletinBoard bulletinBoard, int[] WGsSelected)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._bulletinBoardService.SaveBulletinBoard(bulletinBoard, WGsSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "bulletinboard");

            var model = this.CreateInputViewModel(bulletinBoard);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var bulletinBoard = this._bulletinBoardService.GetBulletinBoard(id);

            if (bulletinBoard == null)
                return new HttpNotFoundResult("No bulletin board found...");

            var model = this.CreateInputViewModel(bulletinBoard);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, BulletinBoard bulletinBoard, int[] WGsSelected)
        {
            var bulletinBoardToSave = this._bulletinBoardService.GetBulletinBoard(id);
            this.UpdateModel(bulletinBoardToSave, "bulletinboard");

            bulletinBoardToSave.ShowOnStartPage = bulletinBoard.ShowOnStartPage;
            bulletinBoardToSave.PublicInformation = bulletinBoard.PublicInformation;

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._bulletinBoardService.SaveBulletinBoard(bulletinBoardToSave, WGsSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "bulletinboard");

            var model = this.CreateInputViewModel(bulletinBoardToSave);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this._bulletinBoardService.DeleteBulletinBoard(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "bulletinboard");
            else
            {
                this.TempData.Add("Error", "");

                return this.RedirectToAction("edit", "bulletinboard", new { id = id });
            }
        }

        private BulletinBoardIndexViewModel IndexInputViewModel()
        {
            var model = new BulletinBoardIndexViewModel { };  

            return model;
        }

        private BulletinBoardInputViewModel CreateInputViewModel(BulletinBoard bulletinBoard)
        {
            var wgsSelected = bulletinBoard.WGs ?? new List<WorkingGroupEntity>();
            var wgsAvailable = new List<WorkingGroupEntity>();

            var workingGroups = this._workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id);
            var wgsSelectedIds = wgsSelected.Select(g => g.Id).ToArray();

            foreach (var wg in workingGroups)
            {
                if (!wgsSelectedIds.Contains(wg.Id))
                {
                    wgsAvailable.Add(wg);
                }
            }

            var model = new BulletinBoardInputViewModel
            {
                BulletinBoard = bulletinBoard,
                WGsAvailable = wgsAvailable.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                WGsSelected = wgsSelected.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}
