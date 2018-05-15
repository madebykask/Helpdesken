using System;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Contract;

namespace DH.Helpdesk.Web.Models.Contract
{
    public class ContractHistoryFieldsDiff
    {
        public DateTime Modified { get; set; }
        public string RegisteredBy { get; set; }
        public List<string> Emails { get; set; }
        public List<FieldDifference> FieldsDiff { get; set; }
    }
}