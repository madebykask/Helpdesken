namespace dhHelpdesk_NG.Service
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectService
    {
        void AddProject(NewProject project);

        void DeleteProject(int id);

        void UpdateProject(UpdatedProject project);

        ProjectOverview GetProject(int id);

        List<ProjectOverview> GetCustomerProjects(int customerId);

        List<ProjectOverview> GetCustomerProjects(int customerId, EntityStatus entityStatus, int? projectManagerId, string projectNameLike);

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

        void DeleteFiles(int projectId, List<string> deletedRegistrationFiles);

        byte[] GetFileContent(int id, string fileName);

        bool FileExists(int id, string fileName);

        List<string> FindFileNamesExcludeSpecified(int id, List<string> deletedFileNames);
    }
}