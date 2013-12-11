using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    using dhHelpdesk_NG.Domain.Notifiers;

    public class ComputerUserGroup : Entity
    {
        public int Customer_Id { get; set; }
        public int? Department_Id { get; set; }
        public int IsDefault { get; set; }
        public int ShowOnStartPage { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OU> OUs { get; set; }
        public virtual ICollection<ComputerUser> ComputerUsers { get; set; }
    }
}
