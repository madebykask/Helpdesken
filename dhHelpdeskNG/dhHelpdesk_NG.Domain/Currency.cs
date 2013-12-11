using System;

namespace dhHelpdesk_NG.Domain
{
    public class Currency : Entity
    {
        public string Code { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
