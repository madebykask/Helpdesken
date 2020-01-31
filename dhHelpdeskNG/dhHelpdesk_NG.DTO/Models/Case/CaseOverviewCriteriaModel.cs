using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseOverviewCriteriaModel
    {
        public CaseOverviewCriteriaModel()
        {
            
        }

        public bool MyCasesRegistrator { get; set; }
        public bool MyCasesInitiator { get; set; }
        public bool MyCasesFollower { get; set; }
        public bool MyCasesRegarding { get; set; }
        public bool MyCasesUserGroup { get; set; }

        public string UserId { get; set; }
        public string UserEmployeeNumber { get; set; }
        public int? MyCasesInitiatorDepartmentId { get; set; }

        public List<string> GroupMember { get; set; } 
    }
}
