using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class ExtraFollower
    {
        public int Id { get; set; }
        public string Follower { get; set; }
        public int CaseId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
