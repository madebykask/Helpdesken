using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class BuildingController : BaseController
    {
        private readonly IBuildingService _buildingService;
        private readonly ICustomerService _customerService;

        public BuildingController(
            IBuildingService buildingService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _buildingService = buildingService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var buildings = _buildingService.GetBuildings(customer.Id);

            var model = new BuildingIndexViewModel { Buildings = buildings, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var building = new Building { Customer_Id = customer.Id, IsActive = 1 };

            var model = new BuildingInputViewModel { Building = building, Customer = customer };
            return View(model);
           
        }

        [HttpPost]
        public ActionResult New(Building building)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _buildingService.SaveBuilding(building, out errors);

            if (errors.Count == 0)               
                return RedirectToAction("index", "building", new { customerId = building.Customer_Id });

            var customer = _customerService.GetCustomer(building.Customer_Id);
            var model = new BuildingInputViewModel { Building = building, Customer = customer };
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var building = _buildingService.GetBuilding(id);

            if (building == null)                
                return new HttpNotFoundResult("No building found...");

            var customer = _customerService.GetCustomer(building.Customer_Id);
            var model = new BuildingInputViewModel { Building = building, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Building building)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _buildingService.SaveBuilding(building, out errors);

            if (errors.Count == 0)                
                return RedirectToAction("index", "building", new { customerId = building.Customer_Id });

            var customer = _customerService.GetCustomer(building.Customer_Id);
            var model = new BuildingInputViewModel { Building = building, Customer = customer };
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var building = _buildingService.GetBuilding(id);

            if (_buildingService.DeleteBuilding(id) == DeleteMessage.Success)
                return RedirectToAction("index", "building", new { customerId = building.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "building", new { area = "admin", id = building.Id });
            }
        }
    }
}
