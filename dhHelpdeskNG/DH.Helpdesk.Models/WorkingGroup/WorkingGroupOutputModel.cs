using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Models.WorkingGroup
{
    public class WorkingGroupOutputModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public string WorkingGroupName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? StateSecondaryId { get; set; }
    }
}
