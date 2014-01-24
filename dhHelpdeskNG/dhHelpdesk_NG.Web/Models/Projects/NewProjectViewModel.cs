namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class NewProjectViewModel
    {
        public ProjectEditModel Project { get; set; }

        public List<SelectListItem> Users { get; set; }
    }
}