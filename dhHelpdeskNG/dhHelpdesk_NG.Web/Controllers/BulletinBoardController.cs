using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Models;

namespace dhHelpdesk_NG.Web.Controllers
{
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
            _bulletinBoardService = bulletinBoardService;
            _workingGroupService = workingGroupService;
        }

        public ActionResult Index()
        {
            var model = IndexInputViewModel();

            if (SessionFacade.CurrentBulletinBoardSearch != null)
            {
                BulletinBoardSearch CS = new BulletinBoardSearch();
                CS = SessionFacade.CurrentBulletinBoardSearch;
                model.BulletinBoards = _bulletinBoardService.SearchAndGenerateBulletinBoard(SessionFacade.CurrentCustomer.Id, CS);
                model.SearchBbs = CS.SearchBbs;
            }
            else
                model.BulletinBoards = _bulletinBoardService.GetBulletinBoards(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.ChangedDate).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(BulletinBoardSearch SearchBulletinBoards)
        {
            BulletinBoardSearch CS = new BulletinBoardSearch();
            if (SessionFacade.CurrentBulletinBoardSearch != null)
                CS = SessionFacade.CurrentBulletinBoardSearch;

            CS.SearchBbs = SearchBulletinBoards.SearchBbs;

            var bb = _bulletinBoardService.SearchAndGenerateBulletinBoard(SessionFacade.CurrentCustomer.Id, CS);

            if (SearchBulletinBoards != null)
                SessionFacade.CurrentBulletinBoardSearch = CS;

            var model = IndexInputViewModel();

            model.BulletinBoards = bb;
            model.SearchBbs = CS.SearchBbs;

            return View(model);
        }

        public ActionResult Sort(string FieldName)
        {
            var model = IndexInputViewModel();
            BulletinBoardSearch CS = new BulletinBoardSearch();
            if (SessionFacade.CurrentBulletinBoardSearch!= null)
                CS = SessionFacade.CurrentBulletinBoardSearch;
            CS.Ascending = !CS.Ascending;
            CS.SortBy = FieldName;
            SessionFacade.CurrentBulletinBoardSearch = CS;
            return View(model);
        }

        public ActionResult New()
        {
            var model = CreateInputViewModel(new BulletinBoard { Customer_Id = SessionFacade.CurrentCustomer.Id, ShowDate = DateTime.Now, ShowUntilDate = DateTime.Now });

            return View(model);
        }

        [HttpPost]
        public ActionResult New(BulletinBoard bulletinBoard, int[] WGsSelected)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _bulletinBoardService.SaveBulletinBoard(bulletinBoard, WGsSelected, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "bulletinboard");

            var model = CreateInputViewModel(bulletinBoard);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var bulletinBoard = _bulletinBoardService.GetBulletinBoard(id);

            if (bulletinBoard == null)
                return new HttpNotFoundResult("No bulletin board found...");

            var model = CreateInputViewModel(bulletinBoard);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, BulletinBoard bulletinBoard, int[] WGsSelected)
        {
            var bulletinBoardToSave = _bulletinBoardService.GetBulletinBoard(id);
            UpdateModel(bulletinBoardToSave, "bulletinboard");

            bulletinBoardToSave.ShowOnStartPage = bulletinBoard.ShowOnStartPage;
            bulletinBoardToSave.PublicInformation = bulletinBoard.PublicInformation;

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _bulletinBoardService.SaveBulletinBoard(bulletinBoardToSave, WGsSelected, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "bulletinboard");

            var model = CreateInputViewModel(bulletinBoardToSave);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_bulletinBoardService.DeleteBulletinBoard(id) == DeleteMessage.Success)
                return RedirectToAction("index", "bulletinboard");
            else
            {
                TempData.Add("Error", "");

                return RedirectToAction("edit", "bulletinboard", new { id = id });
            }
        }

        private BulletinBoardIndexViewModel IndexInputViewModel()
        {
            var model = new BulletinBoardIndexViewModel { };  

            return model;
        }

        private BulletinBoardInputViewModel CreateInputViewModel(BulletinBoard bulletinBoard)
        {
            var wgsSelected = bulletinBoard.WGs ?? new List<WorkingGroup>();
            var wgsAvailable = new List<WorkingGroup>();

            foreach (var wg in _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id))
            {
                if (!wgsSelected.Contains(wg))
                    wgsAvailable.Add(wg);
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
