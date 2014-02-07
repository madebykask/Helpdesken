namespace DH.Helpdesk.Dal.Repositories.Projects
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IProjectCollaboratorRepository : INewRepository
    {
        void Add(NewProjectCollaborator businessModel);

        void Add(List<NewProjectCollaborator> businessModels);

        void Add(int projectId, List<int> collaboratorIds);

        void Delete(int id);

        void Delete(List<int> ids);

        void DeleteByProjectId(int projectId);

        List<ProjectCollaboratorOverview> Find(int projectId);
    }
}