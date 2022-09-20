using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.Case.MergedCase
{
    public class MergedParentInfo
    {
        /// <summary>
        /// Id of the parent case
        /// </summary>
        public int ParentId { get; set; }

        public decimal CaseNumber { get; set; }

        public UserNamesStruct CaseAdministrator { get; set; }

        public DateTime? FinishingDate { get; set; }
    }
}
