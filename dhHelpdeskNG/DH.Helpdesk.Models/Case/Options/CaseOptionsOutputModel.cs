using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.Models.Case.Options
{
    public class CaseOptionsOutputModel
    {
        public List<ItemOverview> CustomerRegistrationSources { get; set; }
        public List<ItemOverview> Systems { get; set; }
        public List<ItemOverview> Urgencies { get; set; }
        public List<ItemOverview> Impacts { get; set; }
        public List<ItemOverview> Suppliers { get; set; }
        public List<ItemOverview> Countries { get; set; }
        public List<ItemOverview> Currencies { get; set; }
        public List<ItemOverview> WorkingGroups { get; set; }
        public List<ItemOverview> ResponsibleUsers { get; set; }
        public List<ItemOverview> Performers { get; set; }
        public List<ItemOverview> Priorities { get; set; }
        public List<ItemOverview> Statuses { get; set; }
        public List<ItemOverview> StateSecondaries { get; set; }
        public List<ItemOverview> Projects { get; set; }
        public List<ItemOverview> Problems { get; set; }
        public List<ItemOverview> Changes { get; set; }
        public List<ItemOverview> SolutionsRates { get; set; }
        
    }
}
