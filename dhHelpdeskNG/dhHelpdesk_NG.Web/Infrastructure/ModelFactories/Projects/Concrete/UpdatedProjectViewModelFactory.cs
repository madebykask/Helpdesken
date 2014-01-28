namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Projects.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;
    using dhHelpdesk_NG.Web.Models;
    using dhHelpdesk_NG.Web.Models.Projects;

    public class UpdatedProjectViewModelFactory : IUpdatedProjectViewModelFactory
    {
        public UpdatedProjectViewModel Create(
            ProjectOverview projectOverview,
            List<User> users,
            List<ProjectCollaboratorOverview> collaboratorOverviews,
            List<ProjectScheduleOverview> schedules,
            List<ProjectLogOverview> logs,
            List<Case> cases)
        {
            var project = MapProjectOverview(projectOverview);

            // project.ProjectCollaboratorIds = collaboratorOverviews.Select(x => x.UserId.ToString()).ToList();
            project.ProjectCollaboratorIds = collaboratorOverviews.Select(x => x.UserId).ToList();

            var items = users.Select(x => new { Value = x.Id, Name = string.Format("{0} {1}", x.FirstName, x.SurName) });
            var ids = collaboratorOverviews.Select(x => x.UserId).ToList();
            var list = new MultiSelectList(items, "Value", "Name", ids);

            var scheduleEditModel = CreateScheduleEditModel(collaboratorOverviews, schedules);

            return new UpdatedProjectViewModel
                       {
                           ProjectEditModel = project,
                           Users = list, // users.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = string.Format("{0} {1}", x.FirstName, x.SurName) }).ToList(),
                           UpdatedProjectScheduleEditModel = scheduleEditModel,
                           ProjectLog = new ProjectLogEditModel { ProjectId = project.Id, ResponsibleUserId = SessionFacade.CurrentUser.Id },
                           ProjectLogs = logs,
                           CaseOverviews = cases.Select(MapCase).ToList(),
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
                IsActive = projectOverview.IsActive,
                EndDate = projectOverview.EndDate.HasValue ? DateTime.SpecifyKind(projectOverview.EndDate.Value, DateTimeKind.Utc).ToShortDateString() : string.Empty
            };
        }

        private static ProjectScheduleEditModel MapProjectScheduleOverview(ProjectScheduleOverview projectOverview)
        {
            return new ProjectScheduleEditModel
            {
                Id = projectOverview.Id,
                Name = projectOverview.Name,
                Description = projectOverview.Description,
                State = (ScheduleStates)projectOverview.State,
                StartDate = projectOverview.StartDate.HasValue ? DateTime.SpecifyKind(projectOverview.StartDate.Value, DateTimeKind.Utc).ToShortDateString() : string.Empty,
                FinishDate = projectOverview.FinishDate.HasValue ? DateTime.SpecifyKind(projectOverview.FinishDate.Value, DateTimeKind.Utc).ToShortDateString() : string.Empty,
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
                WatchDate = caseEntity.WatchDate.HasValue ? DateTime.SpecifyKind(caseEntity.WatchDate.Value, DateTimeKind.Utc).ToShortDateString() : string.Empty,
            };
        }
    }
}