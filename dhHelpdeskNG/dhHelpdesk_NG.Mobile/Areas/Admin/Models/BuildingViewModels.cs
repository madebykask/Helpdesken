namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class BuildingIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Building> Buildings { get; set; }
    }

    public class BuildingInputViewModel
    {
        public Customer Customer { get; set; }
        public Building Building { get; set; }
    }
}