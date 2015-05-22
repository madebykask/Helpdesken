namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Dal.Enums;

    public interface IProjectService
    {
        void AddProject(NewProject project);

        void DeleteProject(int id);

        void UpdateProject(UpdatedProject project);

        ProjectOverview GetProject(int id);

        List<ProjectOverview> GetCustomerProjects(int customerId);

        List<ProjectOverview> GetCustomerProjects(
            int customerId,
            EntityStatus entityStatus,
            int? projectManagerId,
            string projectNameLike,
            SortField sortField);

        void AddSchedule(NewProjectSchedule schedule);

        void DeleteSchedule(int id);

        void UpdateSchedule(UpdatedProjectSchedule schedule);

        void UpdateSchedule(List<UpdatedProjectSchedule> schedules);

        List<ProjectScheduleOverview> GetProjectSchedules(int projectId);

        void AddLog(NewProjectLog log);

        void DeleteLog(int id);

        List<ProjectLogOverview> GetProjectLogs(int projectId);

        void AddCollaborator(NewProjectCollaborator collaborator);

        void AddCollaborator(List<NewProjectCollaborator> collaborators);

        void AddCollaborator(int projectId, List<int> collaboratorIds);

        void DeleteCollaborator(int collaboratorId);

        void DeleteCollaborator(List<int> collaboratorIds);

        List<ProjectCollaboratorOverview> GetProjectCollaborators(int projectId);

        void AddFiles(List<NewProjectFile> files);

        void DeleteFiles(int projectId, string basePath, List<string> deletedRegistrationFiles);

        byte[] GetFileContent(int id, string basePath, string fileName);

        bool FileExists(int id, string fileName);

        List<string> FindFileNamesExcludeSpecified(int id, List<string> deletedFileNames);
    }
}