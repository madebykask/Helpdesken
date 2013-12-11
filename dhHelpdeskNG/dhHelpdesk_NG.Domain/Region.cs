using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class Region : Entity
    {
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public int IsDefault { get; set; }
        public string Name { get; set; }
        public string SearchKey { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}
