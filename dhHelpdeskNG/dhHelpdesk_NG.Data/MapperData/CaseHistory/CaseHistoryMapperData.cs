using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.User.Interfaces;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.MapperData.CaseHistory
{
    public class CaseHistoryMapperData
    {
        public Domain.CaseHistory CaseHistory { get; set; }
        public Category Category { get; set; }
        public DepartmentMapperData Department { get; set; }
        public RegistrationSourceCustomer RegistrationSourceCustomer { get; set; }
        public CaseType CaseType { get; set; }
        public ProductArea ProductArea { get; set; }
        public UserMapperData UserPerformer { get; set; }
        public UserMapperData UserResponsible { get; set; }
        public Priority Priority { get; set; }
        public WorkingGroupEntity WorkingGroup { get; set; }
        public StateSecondary StateSecondary { get; set; }
        public Status Status { get; set; }
        public DepartmentMapperData IsAbout_Department { get; set; }

        public IEnumerable<EmailLogMapperData> EmailLogs { get; set; }
    }
}