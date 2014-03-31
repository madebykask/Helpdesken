using System;
using DH.Helpdesk.BusinessData.Models.Common.Output;

namespace DH.Helpdesk.BusinessData.Models.OperationLog.Output
{
    public sealed class OperationLogOverview
    {
        public int Customer_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public string LogText { get; set; }
        public OperationLogCategoryOverview Category { get; set; }
    }
}