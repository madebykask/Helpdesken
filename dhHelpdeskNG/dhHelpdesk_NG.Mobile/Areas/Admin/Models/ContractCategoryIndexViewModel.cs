namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class ContractCategoryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ContractCategory> ContractCategories { get; set; }
    }
}