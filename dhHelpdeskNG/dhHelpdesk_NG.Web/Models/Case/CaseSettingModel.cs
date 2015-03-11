using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Models.Case
{
    using System;
    using System.Collections.Generic;

    public class CaseColumnsSettingsModel
    {
        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public IList<CaseSettings> UserColumns { get; set; }
        
        public IList<SelectListItem> LineList { get; set; }

        public IList<CaseFieldSettingsWithLanguage> CaseFieldSettingLanguages { get; set; }

        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }
        
    }

    public sealed class CaseSettingModel
    {
        public CaseSettingModel()
        {
            
        }

        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public bool RegionCheck { get; set; }
        public IList<Region> Regions { get; set; }
        public string SelectedRegion { get; set; }

        public bool RegisteredByCheck { get; set; }
        public IList<User> RegisteredBy { get; set; }
        public string SelectedRegisteredBy { get; set; }

        public bool CaseTypeCheck { get; set; }

        public string CaseTypePath { get; set; }

        public int CaseTypeId { get; set; }

        public IList<CaseType> CaseTypes { get; set; }

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

        public DateTime? CaseRegistrationDateStartFilter { get; set; }

        public DateTime? CaseRegistrationDateEndFilter { get; set; }

        public DateTime? CaseWatchDateStartFilter { get; set; }

        public DateTime? CaseWatchDateEndFilter { get; set; }

        public DateTime? CaseClosingDateStartFilter { get; set; }

        public DateTime? CaseClosingDateEndFilter { get; set; }

        public bool CaseRegistrationDateFilterShow { get; set; }

        public bool CaseWatchDateFilterShow { get; set; }

        public bool CaseClosingDateFilterShow { get; set; }

        public bool ClosingReasonCheck { get; set; }

        public IList<FinishingCause> ClosingReasons { get; set; }

        public int ClosingReasonId { get; set; }

        public string ClosingReasonPath { get; set; }

        public CaseColumnsSettingsModel ColumnSettingModel { get; set; }        

        public string InitiatorName { get; set; }
    }
}