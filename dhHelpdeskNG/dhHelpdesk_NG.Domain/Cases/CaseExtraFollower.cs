using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.Domain.Cases
{
    public class CaseExtraFollower : Entity
    {
        public string Follower { get; set; }
        public int Case_Id { get; set; }
        public int CreatedByUser_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual Case Case { get; set; }
    }
}
