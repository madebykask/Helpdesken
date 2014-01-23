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

        IList<ProjectOverview> GetCustomerProjects(int customerId);

        IList<ProjectOverview> GetCustomerProjects(int customerId, EntityStatus entityStatus, int? projectManagerId, string projectNameLike);
    }

    public interface IProjectLogService
    {
        void AddLog(NewProjectLog project);

        void DeleteLog(int id);

        IList<ProjectLogOverview> GetProjectLogs(int projectId);
    }
}