using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Models.StateSecondaries
{
    public class StateSecondaryOutputModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool IsActive { get; set; }
        public bool NoMailToNotifier { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? WorkingGroupId { get; set; }
        public bool IsDefault { get; set; }
        public int? ReminderDays { get; set; }
        public bool RecalculateWatchDate { get; set; }
        public string AlternativeStateSecondaryName { get; set; }
    }
}
