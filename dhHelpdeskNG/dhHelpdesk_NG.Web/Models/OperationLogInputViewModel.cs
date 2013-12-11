using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Web.Models
{
    public class OperationLogIndexViewModel
    {
        public string SearchBbs { get; set; }

        public OperationLog OperationLog { get; set; }

        public IEnumerable<OperationLog> OperationLogs { get; set; }
        public IList<Customer> Customers { get; set; }
        public IList<OperationObject> OperationObjects { get; set; }
        public IList<OperationLogCategory> OperationLogCategories { get; set; }
        public IList<OperationLogList> OperationLogList { get; set; }
    }

    public class OperationLogInputViewModel
    {
        public OperationLog OperationLog { get; set; }

        public IList<SelectListItem> OperationObjectsAvailable { get; set; }
        public IList<SelectListItem> OperationObjectsSelected { get; set; }

        public OperationLogInputViewModel() { }
    }
}