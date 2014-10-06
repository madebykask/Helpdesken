namespace DH.Helpdesk.Web.Models.Projects
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Web.Infrastructure.Filters.Projects;

    public class IndexProjectViewModel
    {
        public List<ProjectOverview> Projects { get; set; }

        public ProjectFilter Filter { get; set; }

        public List<SelectListItem> Users { get; set; }
    }
}