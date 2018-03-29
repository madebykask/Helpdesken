using System;

namespace DH.Helpdesk.Domain.GDPR
{
    public class GDPROperationsAudit : Entity
    {
        public int User_Id { get; set; }
        public int? Customer_Id { get; set; }
        public string Operation { get; set; }
        public string Parameters { get; set; }
        public string Result { get; set; }
        public string Url { get; set; }
        public string Application { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
        public virtual Customer Customer { get; set; }
    }
}