using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Models.Statuses
{
    public class StatusOutputModel
    {
        public int CustomerId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public int? WorkingGroupId { get; set; }
        public int? StateSecondaryId { get; set; }
        public string Name { get; set; }
    }
}
