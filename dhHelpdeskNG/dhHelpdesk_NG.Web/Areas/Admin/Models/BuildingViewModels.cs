using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
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