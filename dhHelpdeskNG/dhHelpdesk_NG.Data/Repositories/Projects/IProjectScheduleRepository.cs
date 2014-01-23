namespace dhHelpdesk_NG.Data.Repositories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectScheduleRepository : INewRepository
    {
        void Add(NewProjectSchedule businessModel);

        void Delete(int id);

        void Update(UpdatedProjectSchedule businessModel);

        List<ProjectScheduleOverview> Find(int projectId);
    }
}