using System;

namespace dhHelpdesk_NG.Domain
{
    public class EmploymentType : Entity
    {
        public int Status { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
