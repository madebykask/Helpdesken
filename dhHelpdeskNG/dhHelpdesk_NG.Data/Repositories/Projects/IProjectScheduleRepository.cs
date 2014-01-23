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

        void DeleteByProjectId(int projectId);

        void Update(UpdatedProjectSchedule businessModel);

        void Update(List<UpdatedProjectSchedule> businessModels);

        List<ProjectScheduleOverview> Find(int projectId);
    }
}