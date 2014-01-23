namespace dhHelpdesk_NG.Service
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectLogService
    {
        void AddLog(NewProjectLog log);

        void DeleteLog(int id);

        IList<ProjectLogOverview> GetProjectLogs(int projectId);
    }
}