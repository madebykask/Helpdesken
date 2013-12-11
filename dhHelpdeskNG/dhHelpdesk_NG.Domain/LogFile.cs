using System;

namespace dhHelpdesk_NG.Domain
{
    public class LogFile : Entity
    {
        public int Log_Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Log Log { get; set; }
    }
}
