using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
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