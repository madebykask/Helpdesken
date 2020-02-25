namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Projects
{
	using System.Collections.Generic;

	using DH.Helpdesk.BusinessData.Models.Projects.Output;
	using DH.Helpdesk.Domain;
	using DH.Helpdesk.Web.Models.Projects;
	using System.Web.Mvc;
	using Services.Services;

	public interface IUpdatedProjectViewModelFactory
    {
        UpdatedProjectViewModel Create(ProjectOverview projectOverview, SelectList users, List<ProjectCollaboratorOverview> collaboratorOverviews, List<ProjectScheduleOverview> schedules, List<ProjectLogOverview> logs, List<Case> cases, IGlobalSettingService globalSettingService);
    }
}