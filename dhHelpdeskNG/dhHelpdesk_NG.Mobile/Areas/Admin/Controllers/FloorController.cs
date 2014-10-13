namespace DH.Helpdesk.Mobile.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Areas.Admin.Models;
    using DH.Helpdesk.Mobile.Infrastructure;

    public class FloorController : BaseAdminController
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
            this._buildingService = buildingService;
            this._floorService = floorService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {

            var floor = this._floorService.GetFloors();

            return this.View(floor);
        }

        public ActionResult New()
        {
            var model = this.CreateInputViewModel(new Floor { IsActive = 1 });

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Floor floor)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._floorService.SaveFloor(floor, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "floor", new { area = "admin" });

            var model = this.CreateInputViewModel(floor);

            return this.View(model);
        }
        
        public ActionResult Edit(int id)
        {
            var floor = this._floorService.GetFloor(id);

            if (floor == null)
                return new HttpNotFoundResult("No floor found...");

            var model = this.CreateInputViewModel(floor);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(Floor floor)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._floorService.SaveFloor(floor, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "floor", new { area = "admin" });

            var model = this.CreateInputViewModel(floor);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this._floorService.DeleteFloor(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "floor", new { area = "admin" });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "floor", new { area = "admin", id = id });
            }
        }

        private FloorInputViewModel CreateInputViewModel(Floor floor)
        {
            var model = new FloorInputViewModel
            {
                Floor = floor,
                Buildings = this._buildingService.GetBuildings(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}
