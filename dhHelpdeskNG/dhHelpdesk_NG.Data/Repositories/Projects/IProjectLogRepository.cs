namespace DH.Helpdesk.Dal.Repositories.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IProjectLogRepository : INewRepository
    {
        void Add(NewProjectLog businessModel);

        void Delete(int id);

        void DeleteByProjectId(int projectId);

        List<ProjectLogOverview> Find(int projectId);
    }
}