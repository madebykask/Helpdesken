namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

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
            this._buildingService = buildingService;
            this._floorService = floorService;
            this._roomService = roomService;
        }

        public ActionResult Index()
        {
            var rooms = this._roomService.GetRooms();

            return this.View(rooms);
        }

        public ActionResult New()
        {
            var model = this.CreateInputViewModel(new Room { });

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New([Bind(Exclude = "Floor")]Room room)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._roomService.SaveRoom(room, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "room", new { area = "admin" });

            var model = this.CreateInputViewModel(room);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var room = this._roomService.GetRoom(id);

            if (room == null)
                return new HttpNotFoundResult("No room found...");

            var model = this.CreateInputViewModel(room);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Floor")]Room room)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._roomService.SaveRoom(room, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "room", new { area = "admin" });

            var model = this.CreateInputViewModel(room);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this._roomService.DeleteRoom(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "room", new { area = "admin" });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "room", new { area = "admin", id = id });
            }
        }

        private RoomInputViewModel CreateInputViewModel(Room room)
        {
            var floors = this._floorService.GetFloors();

            var model = new RoomInputViewModel
            {
                Room = room,
                Buildings = this._buildingService.GetBuildings(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
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
