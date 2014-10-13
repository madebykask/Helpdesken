namespace DH.Helpdesk.Mobile.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Areas.Admin.Models;

    public class BuildingController : BaseAdminController
    {
        private readonly IBuildingService _buildingService;
        private readonly ICustomerService _customerService;

        public BuildingController(
            IBuildingService buildingService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._buildingService = buildingService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var buildings = this._buildingService.GetBuildings(customer.Id);

            var model = new BuildingIndexViewModel { Buildings = buildings, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var building = new Building { Customer_Id = customer.Id, IsActive = 1 };

            var model = new BuildingInputViewModel { Building = building, Customer = customer };
            return this.View(model);
           
        }

        [HttpPost]
        public ActionResult New(Building building)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._buildingService.SaveBuilding(building, out errors);

            if (errors.Count == 0)               
                return this.RedirectToAction("index", "building", new { customerId = building.Customer_Id });

            var customer = this._customerService.GetCustomer(building.Customer_Id);
            var model = new BuildingInputViewModel { Building = building, Customer = customer };
            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var building = this._buildingService.GetBuilding(id);

            if (building == null)                
                return new HttpNotFoundResult("No building found...");

            var customer = this._customerService.GetCustomer(building.Customer_Id);
            var model = new BuildingInputViewModel { Building = building, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(Building building)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._buildingService.SaveBuilding(building, out errors);

            if (errors.Count == 0)                
                return this.RedirectToAction("index", "building", new { customerId = building.Customer_Id });

            var customer = this._customerService.GetCustomer(building.Customer_Id);
            var model = new BuildingInputViewModel { Building = building, Customer = customer };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var building = this._buildingService.GetBuilding(id);

            if (this._buildingService.DeleteBuilding(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "building", new { customerId = building.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "building", new { area = "admin", id = building.Id });
            }
        }
    }
}
