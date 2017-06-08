using System;

namespace DH.Helpdesk.Domain.Cases
{
    using global::System.Collections.Generic;
    

    public class CaseLockEntity : Entity
    {
        public int Case_Id { get; set; }

        public int User_Id { get; set; }

        public Guid LockGUID { get; set; }

        public string BrowserSession { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ExtendedTime { get; set; }

        public virtual User User { get; set; }

        public string ActiveTab { get; set; }
    }
}