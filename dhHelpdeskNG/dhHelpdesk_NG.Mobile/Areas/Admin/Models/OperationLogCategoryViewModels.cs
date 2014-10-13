namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class OperationLogCategoryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<OperationLogCategory> OperationLogCategories { get; set; }
    }

    public class OperationLogCategoryInputViewModel
    {
        public Customer Customer { get; set; }
        public OperationLogCategory OperationLogCategory { get; set; }
    }
}