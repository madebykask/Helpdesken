﻿namespace DH.Helpdesk.Web.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.Shared;

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

        public IList<OperationLogEMailLog> OperationLogEmailLog { get; set; }
        public IList<SelectListItem> SMSWorkingGroupSelected { get; set; }
        public IList<SelectListItem> SMSWorkingGroupAvailable { get; set; }
        public IList<SelectListItem> AdministratorsAvailable { get; set; }
        public IList<SelectListItem> AdministratorsSelected { get; set; }
        public IList<SelectListItem> SystemResponsiblesAvailable { get; set; }
        public IList<SelectListItem> SystemResponsiblesSelected { get; set; }

        public SendToDialogModel SendToDialogModel { get; set; }
        public OperationLogList OperationLogList { get; set; }

        public OperationLogInputViewModel() { }

        public SelectList ResponsibleUsersAvailable { get; set; }

        public Setting CustomerSettings { get; set; }

        public int OperationObjectShow { get; set; }
    }
}