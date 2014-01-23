namespace dhHelpdesk_NG.Data.Repositories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectLogRepository : INewRepository
    {
        void Add(NewProjectLogDto businessModel);

        void Delete(int id);

        List<ProjectLogOverview> Find(int projectId);
    }
}