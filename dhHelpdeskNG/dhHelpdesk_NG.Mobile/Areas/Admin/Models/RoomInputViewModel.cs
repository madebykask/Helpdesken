namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class RoomInputViewModel
    {
        public Room Room { get; set; }

        public IList<SelectListItem> Buildings { get; set; }
        public IList<SelectListItem> Floors { get; set; }

        public IList<SFloor> SFloors { get; set; }
    }
    public struct SFloor
    {
        public int Id { get; set; }
        public int Building_Id { get; set; }
        public string Name { get; set; }
    }

}