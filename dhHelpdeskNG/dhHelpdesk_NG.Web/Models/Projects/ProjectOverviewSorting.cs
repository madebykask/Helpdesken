namespace DH.Helpdesk.Web.Models.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public class ProjectOverviewSorting
    {
        public ProjectOverviewSorting(List<ProjectOverview> projects, SortFieldModel sortFieldModel)
        {
            this.Projects = projects;
            this.SortFieldModel = sortFieldModel;
        }

        [NotNull]
        public List<ProjectOverview> Projects { get; set; }

        public SortFieldModel SortFieldModel { get; set; }
    }
}