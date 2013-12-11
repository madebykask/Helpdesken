using System;

namespace dhHelpdesk_NG.Domain
{
    public class ChangeCategory : Entity
    {
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

