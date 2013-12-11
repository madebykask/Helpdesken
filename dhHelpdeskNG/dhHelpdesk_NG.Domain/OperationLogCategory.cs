using System;

namespace dhHelpdesk_NG.Domain
{
    public class OperationLogCategory : Entity
    {
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public string OLCName  { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}

