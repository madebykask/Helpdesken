using System;

namespace dhHelpdesk_NG.Domain
{
    class FAQCat : Entity
    {
        public string Name { get; set; }
        public int Customer_Id { get; set; }
        public int Parent_FAQCat_Id { get; set; }
        public int PublicFAQCat { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChangedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
