using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dhHelpdesk_NG.DTO.DTOs
{
    public class OperationLogList
    {
        public DateTime CreatedDate { get; set; }
        public string OperationObjectName { get; set; }
        public string OperationLogAdmin { get; set; }
        public string OperationLogCategoryName { get; set; }
        public string OperationLogDescription { get; set; }
        public string OperationLogAction { get; set; }
        public int Id { get; set; }
    }
}
