namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class FloorIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Floor> Floors { get; set; }
    }
}