namespace dhHelpdesk_NG.Data.Repositories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectCollaboratorRepository : INewRepository
    {
        void Add(NewProjectCollaborator businessModel);

        void Add(List<NewProjectCollaborator> businessModels);

        void Delete(int id);

        void Delete(List<int> ids);

        void DeleteByProjectId(int projectId);

        List<ProjectCollaboratorOverview> Find(int projectId);
    }
}