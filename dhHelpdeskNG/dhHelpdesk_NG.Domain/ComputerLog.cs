using System;

namespace dhHelpdesk_NG.Domain
{
    public class ComputerLog : Entity
    {
        public int Computer_Id { get; set; }
        public int CreatedByUser_Id { get; set; }
        public string ComputerLogCategory { get; set; }
        public string ComputerLogText { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Computer Computer { get; set; }
        public virtual User CreatedByUser { get; set; }
    }
}
