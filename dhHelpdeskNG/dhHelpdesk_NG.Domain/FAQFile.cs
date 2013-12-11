using System;

namespace dhHelpdesk_NG.Domain
{
    public class FAQFile : Entity
    {
        public int FAQ_Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual FAQ FAQ { get; set; }
    }
}
