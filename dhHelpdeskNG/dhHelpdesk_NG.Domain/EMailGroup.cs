using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class EMailGroup : Entity
    {
        public int? Customer_Id { get; set; }
        public int IsActive { get; set; }
        public string Members { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
