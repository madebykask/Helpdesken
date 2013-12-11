using System;

namespace dhHelpdesk_NG.Domain
{
    public class Processor : Entity
    {
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
