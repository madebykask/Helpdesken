using System;

namespace dhHelpdesk_NG.Domain
{
    public class CaseFile : Entity
    {
        public int Case_Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Case Case { get; set; }
    }
}
