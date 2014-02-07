namespace DH.Helpdesk.Dal.Repositories.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Dal.Dal;

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