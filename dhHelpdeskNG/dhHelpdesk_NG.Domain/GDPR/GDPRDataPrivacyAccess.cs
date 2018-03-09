using System;

namespace DH.Helpdesk.Domain.GDPR
{
    public class GDPRDataPrivacyAccess : Entity
    {
        public int User_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
    }
}