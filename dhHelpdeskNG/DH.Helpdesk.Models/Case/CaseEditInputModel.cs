using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Models.Case
{
    public class CaseEditInputModel
    {
        public int? CaseId { get; set; }
        public int? ResponsibleUserId { get; set; }
        public int? PerformerId { get; set; }
        public int? WorkingGroupId { get; set; }
        public int? StateSecondaryId { get; set; }
    }
}
