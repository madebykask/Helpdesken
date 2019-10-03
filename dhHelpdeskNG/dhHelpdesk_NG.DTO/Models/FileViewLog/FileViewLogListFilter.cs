using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Shared.Input;

namespace DH.Helpdesk.BusinessData.Models.FileViewLog
{
    public class FileViewLogListFilter
    {
        public int CustomerId { get; set; }
        public List<int> DepartmentsIds { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public int AmountPerPage { get; set; }
        public SortField Sort { get; set; }
    }
}
