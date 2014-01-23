namespace dhHelpdesk_NG.Data.Repositories.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectCollaboratorRepository : INewRepository
    {
        void Add(NewProjectCollaboratorDto businessModel);

        void Delete(int id);

        List<ProjectCollaboratorOverview> Find(int projectId);
    }
}