namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class RegionIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Region> Regions { get; set; }
    }
}