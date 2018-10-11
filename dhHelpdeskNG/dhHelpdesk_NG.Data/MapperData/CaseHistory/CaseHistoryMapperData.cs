using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.Problem.Output;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.BusinessData.Models.Projects.Output;
using DH.Helpdesk.BusinessData.Models;

namespace DH.Helpdesk.Dal.MapperData.CaseHistory
{
    public class CaseHistoryMapperData
    {
        public Domain.CaseHistory CaseHistory { get; set; }

        public CategoryOverview Category { get; set; } 
        public DepartmentOverview Department { get; set; }
        public CaseTypeOverview CaseType { get; set; }
        public ProductAreaOverview ProductArea { get; set; }
        public ProjectOverview Project { get; set; }
        public ProblemOverview Problem { get; set; }
        public CausingPartOverview CausingPart { get; set; }
        public UserBasicOvierview UserPerformer { get; set; }
        public UserBasicOvierview UserResponsible { get; set; }
        public PriorityOverview Priority { get; set; }
        public StatusOverview Status { get; set; }
        public StateSecondaryOverview StateSecondary { get; set; }
        public RegistrationSourceCustomerOverview RegistrationSourceCustomer { get; set; }
        public WorkingGroupOverview WorkingGroup { get; set; }
        public DepartmentOverview IsAbout_Department { get; set; }
        public IList<EmailLogsOverview> EmailLogs { get; set; }
        public RegionOverview Region { get; set; }
        public OUOverview OU { get; set; }
        public RegionOverview IsAbout_Region { get; set; }
        public OUOverview IsAbout_OU { get; set; }
    }
}