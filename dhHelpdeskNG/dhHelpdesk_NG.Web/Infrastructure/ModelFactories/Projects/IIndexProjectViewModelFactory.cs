namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Filters.Projects;
    using dhHelpdesk_NG.Web.Models.Projects;

    public interface IIndexProjectViewModelFactory
    {
        IndexProjectViewModel Create(List<ProjectOverview> overviews, List<User> users, ProjectFilter filter);
    }
}