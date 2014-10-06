namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Mobile.Models.Projects;

    public interface IUpdatedProjectViewModelFactory
    {
        UpdatedProjectViewModel Create(ProjectOverview projectOverview, List<User> users, List<ProjectCollaboratorOverview> collaboratorOverviews, List<ProjectScheduleOverview> schedules, List<ProjectLogOverview> logs, List<Case> cases);
    }
}