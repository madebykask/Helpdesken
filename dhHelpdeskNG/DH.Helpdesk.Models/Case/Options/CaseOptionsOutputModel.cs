using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.Models.Case.Options
{
    public class CaseOptionsOutputModel
    {
        public IList<ItemOverview> CustomerRegistrationSources { get; set; }
        public IList<ItemOverview> Systems { get; set; }
        public IList<ItemOverview> Urgencies { get; set; }
        public IList<ItemOverview> Impacts { get; set; }
        public IList<ItemOverview> Suppliers { get; set; }
        public IList<ItemOverview> Countries { get; set; }
        public IList<ItemOverview> Currencies { get; set; }
        public IList<ItemOverview> WorkingGroups { get; set; }
        public IList<ItemOverview> ResponsibleUsers { get; set; }
        public IList<ItemOverview> Performers { get; set; }
        public IList<ItemOverview> Priorities { get; set; }
        public IList<ItemOverview> Statuses { get; set; }
        public IList<ItemOverview> StateSecondaries { get; set; }
        public IList<ItemOverview> Projects { get; set; }
        public IList<ItemOverview> CausingParts { get; set; }
        public IList<ItemOverview> Problems { get; set; }
        public IList<ItemOverview> Changes { get; set; }
        public IList<ItemOverview> SolutionsRates { get; set; }
    }
}
