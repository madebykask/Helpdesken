using System.Web.Mvc;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Models.Case
{
    using System.Collections.Generic;

    public sealed class CaseSettingModel
    {
        public CaseSettingModel()
        {
            
        }

        public bool RegionCheck { get; set; }
        public IList<Region> Regions { get; set; }
        public string SelectedRegion { get; set; }

        public bool RegisteredByCheck { get; set; }
        public IList<User> RegisteredBy { get; set; }
        public string SelectedRegisteredBy { get; set; }

        public bool CaseTypeCheck { get; set; }

        public bool ProductAreaCheck { get; set; }
        public string ProductAreaPath { get; set; }
        public int ProductAreaId { get; set; }
        public IList<ProductArea> ProductAreas { get; set; }

        public bool WorkingGroupCheck { get; set; }
        public IList<WorkingGroupEntity> WorkingGroups { get; set; }
        public string SelectedWorkingGroup { get; set; }

        public bool ResponsibleCheck { get; set; }

        public bool AdministratorCheck { get; set; }
        public IList<User> Administrators { get; set; }
        public string SelectedAdministrator { get; set; }

        public bool PriorityCheck { get; set; }
        public IList<Priority> Priorities { get; set; }
        public string SelectedPriority { get; set; }

        public bool StateCheck { get; set; }

        public bool SubStateCheck { get; set; }
        public IList<StateSecondary> SubStates { get; set; }
        public string SelectedSubState { get; set; }

    }
}
    
