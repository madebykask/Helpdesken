using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")] 
    public class FloorController : BaseController
    {
        private readonly IBuildingService _buildingService;
        private readonly IFloorService _floorService;
        private readonly ICustomerService _customerService;

        public FloorController(
            IBuildingService buildingService,
            IFloorService floorService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _buildingService = buildingService;
            _floorService = floorService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {

            var floor = _floorService.GetFloors();

            return View(floor);
        }

        public ActionResult New()
        {
            var model = CreateInputViewModel(new Floor { IsActive = 1 });

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Floor floor)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _floorService.SaveFloor(floor, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "floor", new { area = "admin" });

            var model = CreateInputViewModel(floor);

            return View(model);
        }
        
        public ActionResult Edit(int id)
        {
            var floor = _floorService.GetFloor(id);

            if (floor == null)
                return new HttpNotFoundResult("No floor found...");

            var model = CreateInputViewModel(floor);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Floor floor)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _floorService.SaveFloor(floor, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "floor", new { area = "admin" });

            var model = CreateInputViewModel(floor);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_floorService.DeleteFloor(id) == DeleteMessage.Success)
                return RedirectToAction("index", "floor", new { area = "admin" });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "floor", new { area = "admin", id = id });
            }
        }

        private FloorInputViewModel CreateInputViewModel(Floor floor)
        {
            var model = new FloorInputViewModel
            {
                Floor = floor,
                Buildings = _buildingService.GetBuildings(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}
