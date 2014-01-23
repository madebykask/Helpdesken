namespace dhHelpdesk_NG.Service
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public interface IProjectCollaboratorService
    {
        void AddCollaborator(NewProjectCollaborator collaborator);

        void AddCollaborator(List<NewProjectCollaborator> collaborators);

        void DeleteCollaborator(int collaboratorId);

        void DeleteCollaborator(List<int> collaboratorIds);

        List<ProjectCollaboratorOverview> GetProjectCollaborators(int projectId);
    }
}