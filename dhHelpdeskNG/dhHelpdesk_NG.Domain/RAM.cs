using System;

namespace dhHelpdesk_NG.Domain
{
    public class RAM : Entity
    {
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
