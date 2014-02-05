using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Web.Models
{
    public class OperationLogIndexViewModel
    {
        //public string SearchCs { get; set; }
        public IOperationLogSearch OLSearch_Filter { get; set; }

        public OperationLog OperationLog { get; set; }

        public IEnumerable<OperationLog> OperationLogs { get; set; }
        public IList<Customer> Customers { get; set; }
        public IList<OperationObject> OperationObjects { get; set; }
        public IList<OperationLogCategory> OperationLogCategories { get; set; }
        public IList<OperationLogList> OperationLogList { get; set; }
    }

    public class OperationLogInputViewModel
    {
        public int OperationLogHour { get; set; }
        public int OperationLogMinute { get; set; }

        public OperationLog OperationLog { get; set; }

        public IList<SelectListItem> OperationObjects { get; set; }
        public IList<SelectListItem> OperationLogCategories { get; set; }
        public IList<SelectListItem> OperationObjectsAvailable { get; set; }
        public IList<SelectListItem> OperationObjectsSelected { get; set; }
        
        

        public OperationLogInputViewModel() { }
    }
}