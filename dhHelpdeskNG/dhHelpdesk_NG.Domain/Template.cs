using System;

namespace dhHelpdesk_NG.Domain
{
    public class Template
    {
        public int Customer_Id { get; set; }
        public int Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
