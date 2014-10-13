namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class ContractCategoryInputViewModel
    {
        public ContractCategory ContractCategory { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> CaseType { get; set; }
        public IList<SelectListItem> StateSecondary { get; set; }
    }
}