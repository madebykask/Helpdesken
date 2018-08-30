namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class CaseTypeInputViewModel
    {
        public bool CanAddSubCaseType { get; set; }

        public CaseType CaseType { get; set; }

        public Customer Customer { get; set; }

        public IList<SelectListItem> SystemOwners { get; set; }
        public IList<WorkingGroupEntity> WorkingGroups { get; set; }
    }
}