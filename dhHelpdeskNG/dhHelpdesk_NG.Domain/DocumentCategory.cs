using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class DocumentCategory : Entity
    {
        public int? ChangedByUser_Id { get; set; }
        public int CreatedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
