using System;

namespace dhHelpdesk_NG.Domain
{
    public class OrderLog : Entity
    {
        public int Order_Id { get; set; }
        public int User_Id { get; set; }
        public string LogNote { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Order Order { get; set; }
        public virtual User User { get; set; }
    }
}
