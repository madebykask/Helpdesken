namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Infrastructure.Filters.Projects;
    using DH.Helpdesk.Web.Models.Projects;

    public interface IIndexProjectViewModelFactory
    {
        IndexProjectViewModel Create(List<ProjectOverview> overviews, List<User> users, ProjectFilter filter);
    }
}