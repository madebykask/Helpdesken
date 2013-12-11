using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
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