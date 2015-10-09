using System.Linq;
namespace DH.Helpdesk.BusinessData.Models.Case
{
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Cases;
    using System.Collections.Generic;
    public class CaseDataSet
    {
        public CaseDataSet()
        {
        
        }

        public IList<CaseType> CaseTypeQuery { get; set; }
        public IList<ProductArea> ProductAreaQuery { get; set; }
        public IList<FinishingCause> ClosingReasonQuery { get; set; }
        public IList<Customer> CustomerQuery { get; set; }
        public IList<Region> RegionQuery { get; set; }
        public IList<Department> DepartmentQuery { get; set; }
        public IList<OU> OrganizationUnitQuery { get; set; }
        public IList<User> UserQuery { get; set; }
        public IList<CaseFile> CaseFileQuery { get; set; }
        public IList<System> SystemQuery { get; set; }
        public IList<Urgency> UrgencyQuery { get; set; }
        public IList<Impact> ImpactQuery { get; set; }
        public IList<Category> CategoryQuery { get; set; }
        public IList<Supplier> SupplierQuery { get; set; }
        public IList<WorkingGroupEntity> WorkingGroupQuery { get; set; }
        public IList<Priority> PriorityQuery { get; set; }
        public IList<Status> StatusQuery { get; set; }
        public IList<StateSecondary> StateSecondaryQuery { get; set; }
        public IList<CausingPart> CausingPartQuery { get; set; }        
        public IList<CaseStatistic> CaseStatisticsQuery { get; set; }
        public IList<RegistrationSourceCustomer> RegistrationSourceCustomerQuery { get; set; }
        public IList<Language> LanguageQuery { get; set; }
        public IList<Log> LogQuery { get; set; }
        //public IList<LogFile> LogFileQuery { get; set; }
    }
}
