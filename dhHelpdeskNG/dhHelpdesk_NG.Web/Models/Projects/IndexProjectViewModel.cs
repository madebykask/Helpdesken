namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Filters.Projects;

    public class IndexProjectViewModel
    {
        public List<ProjectOverview> Projects { get; set; }

        public ProjectFilter Filter { get; set; }

        public List<SelectListItem> Users { get; set; }
    }
}