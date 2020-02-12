namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using BusinessData.Models.Case.Output;

    public class ContractCategoryInputViewModel
    {
        public ContractCategory ContractCategory { get; set; }
        public Customer Customer { get; set; }

        public IList<CaseTypeOverview> CaseTypes { get; set; }
        public int CaseType_Id { get; set; }
        public string ParentPath_CaseType { get; set; }

        public IList<SelectListItem> StateSecondary { get; set; }
    }
}