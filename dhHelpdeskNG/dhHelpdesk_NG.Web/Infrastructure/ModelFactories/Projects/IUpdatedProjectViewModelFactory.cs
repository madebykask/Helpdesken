namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;
    using dhHelpdesk_NG.Web.Models.Projects;

    public interface IUpdatedProjectViewModelFactory
    {
        UpdatedProjectViewModel Create(ProjectOverview projectOverview, List<User> users, List<ProjectCollaboratorOverview> collaboratorOverviews, List<ProjectScheduleOverview> schedules, List<ProjectLogOverview> logs, List<Case> cases);
    }
}