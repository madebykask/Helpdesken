using System;

namespace dhHelpdesk_NG.Domain
{
    public class ProductAreaQuestionVersion : Entity
    {
        public int ChangedByUser_Id { get; set; }
        public int ProductArea_Id { get; set; }
        public int Version { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ValidFromDate { get; set; }

        public virtual ProductArea ProductArea { get; set; }
    }
}
