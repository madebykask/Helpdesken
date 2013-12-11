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
    public class RoomController : BaseController
    {
        private readonly IBuildingService _buildingService;
        private readonly IFloorService _floorService;
        private readonly IRoomService _roomService;

        public RoomController(
            IBuildingService buildingService,
            IFloorService floorService,
            IRoomService roomService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _buildingService = buildingService;
            _floorService = floorService;
            _roomService = roomService;
        }

        public ActionResult Index()
        {
            var rooms = _roomService.GetRooms();

            return View(rooms);
        }

        public ActionResult New()
        {
            var model = CreateInputViewModel(new Room { });

            return View(model);
        }

        [HttpPost]
        public ActionResult New([Bind(Exclude = "Floor")]Room room)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _roomService.SaveRoom(room, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "room", new { area = "admin" });

            var model = CreateInputViewModel(room);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var room = _roomService.GetRoom(id);

            if (room == null)
                return new HttpNotFoundResult("No room found...");

            var model = CreateInputViewModel(room);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Floor")]Room room)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _roomService.SaveRoom(room, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "room", new { area = "admin" });

            var model = CreateInputViewModel(room);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_roomService.DeleteRoom(id) == DeleteMessage.Success)
                return RedirectToAction("index", "room", new { area = "admin" });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "room", new { area = "admin", id = id });
            }
        }

        private RoomInputViewModel CreateInputViewModel(Room room)
        {
            var floors = _floorService.GetFloors();

            var model = new RoomInputViewModel
            {
                Room = room,
                Buildings = _buildingService.GetBuildings(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Floors = floors.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                SFloors = floors.Select(x => new SFloor
                {
                    Id = x.Id,
                    Building_Id = x.Building_Id,
                    Name = x.Name
                }).ToList()
            };

            return model;
        }
    }
}
