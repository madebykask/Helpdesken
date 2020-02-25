namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Projects.Concrete
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;

	using DH.Helpdesk.BusinessData.Models.Projects.Output;
	using DH.Helpdesk.Domain;
	using DH.Helpdesk.Web.Models.Case;
	using DH.Helpdesk.Web.Models.Projects;
	using Services.Services;

	public class UpdatedProjectViewModelFactory : IUpdatedProjectViewModelFactory
    {
        public UpdatedProjectViewModel Create(
            ProjectOverview projectOverview,
            SelectList users,
            List<ProjectCollaboratorOverview> collaboratorOverviews,
            List<ProjectScheduleOverview> schedules,
            List<ProjectLogOverview> logs,
            List<Case> cases,
			IGlobalSettingService globalSettingService)
        {
            var project = MapProjectOverview(projectOverview);

            // project.ProjectCollaboratorIds = collaboratorOverviews.Select(x => x.UserId.ToString()).ToList();
            project.ProjectCollaboratorIds = collaboratorOverviews.Select(x => x.UserId).ToList();

            var items = users.Select(x => new { Value = x.Value, Name = x.Text });
            var ids = collaboratorOverviews.Select(x => x.UserId).ToList();
            var list = new MultiSelectList(items, "Value", "Name", ids);

            var scheduleEditModel = CreateScheduleEditModel(collaboratorOverviews, schedules);

            var newScheduleEditModel = new NewProjectScheduleEditModel();
            newScheduleEditModel.ProjectScheduleEditModel = new ProjectScheduleEditModel { ProjectId = projectOverview.Id };

			var whiteList = globalSettingService.GetFileUploadWhiteList();
            return new UpdatedProjectViewModel
                       {
                           ProjectEditModel = project,
                           Users = list, // users.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = string.Format("{0} {1}", x.FirstName, x.SurName) }).ToList(),
                           UpdatedProjectScheduleEditModel = scheduleEditModel,
                           ProjectLog = new ProjectLogEditModel { ProjectId = project.Id, ResponsibleUserId = SessionFacade.CurrentUser.Id },
                           ProjectLogs = logs,
                           CaseOverviews = cases.Select(MapCase).ToList(),
                           NewProjectScheduleEditModel = newScheduleEditModel,
                           Guid = project.Id,
						   FileUploadWhiteList = whiteList
                       };
        }

        private static UpdatedProjectScheduleEditModel CreateScheduleEditModel(IEnumerable<ProjectCollaboratorOverview> users, IEnumerable<ProjectScheduleOverview> schedules)
        {
            return new UpdatedProjectScheduleEditModel
                       {
                           Users = users.Select(x => new SelectListItem { Value = x.UserId.ToString(), Text = x.UserName }).ToList(),
                           ProjectScheduleEditModels = schedules.Select(MapProjectScheduleOverview).ToList()
                       };
        }

        private static ProjectEditModel MapProjectOverview(ProjectOverview projectOverview)
        {
            return new ProjectEditModel
            {
                Id = projectOverview.Id,
                Name = projectOverview.Name,
                ProjectManagerId = projectOverview.ProjectManagerId,
                Description = projectOverview.Description,
                IsActive = projectOverview.IsActive == 1,
                StartDate = projectOverview.StartDate,
                EndDate = projectOverview.EndDate
            };
        }

        private static ProjectScheduleEditModel MapProjectScheduleOverview(ProjectScheduleOverview projectOverview)
        {
            DateTime? startDate = null;
            if (projectOverview.StartDate.HasValue)
                startDate = DateTime.SpecifyKind(projectOverview.StartDate.Value, DateTimeKind.Utc);

            DateTime? finishDate = null;
            if (projectOverview.FinishDate.HasValue)
                finishDate = DateTime.SpecifyKind(projectOverview.FinishDate.Value, DateTimeKind.Utc);

            return new ProjectScheduleEditModel
            {
                Id = projectOverview.Id,
                Name = projectOverview.Name,
                Description = projectOverview.Description,
                State = (ScheduleStates)projectOverview.State,
                StartDate = startDate,
                FinishDate = finishDate,
                CaseNumber = projectOverview.CaseNumber,
                Position = projectOverview.Position,
                ProjectId = projectOverview.ProjectId,
                Time = projectOverview.Time,
                UserId = projectOverview.UserId
            };
        }

        private static CaseOverview MapCase(Case caseEntity)
        {
            return new CaseOverview
            {
                Id = caseEntity.Id,
                CaseNumber = caseEntity.CaseNumber.ToString(),
                Caption = caseEntity.Caption,
                RegistrationDate = DateTime.SpecifyKind(caseEntity.RegTime, DateTimeKind.Utc).ToShortDateString(),
                Initiator = caseEntity.PersonsName,
                Administrator = caseEntity.Administrator != null ? caseEntity.Administrator.SurName + " " + caseEntity.Administrator.FirstName : "",
                Department = caseEntity.Department.DepartmentName,
                //WatchDate = caseEntity.WatchDate.HasValue ? DateTime.SpecifyKind(caseEntity.WatchDate.Value, DateTimeKind.Utc).ToShortDateString() : string.Empty,
                //CaseType = caseEntity.CaseType.Name,
                //SubState = caseEntity.StateSecondary.Name,
            };
        }
    }
}