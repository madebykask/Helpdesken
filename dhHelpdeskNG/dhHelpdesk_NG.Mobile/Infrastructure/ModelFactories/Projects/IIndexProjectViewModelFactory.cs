namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Mobile.Infrastructure.Filters.Projects;
    using DH.Helpdesk.Mobile.Models.Projects;

    public interface IIndexProjectViewModelFactory
    {
        IndexProjectViewModel Create(List<ProjectOverview> overviews, List<User> users, ProjectFilter filter);
    }
}