namespace dhHelpdesk_NG.Service.Concrete
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Repositories.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectCollaboratorService : IProjectCollaboratorService
    {
        private readonly IProjectCollaboratorRepository projectCollaboratorRepository;

        public ProjectCollaboratorService(IProjectCollaboratorRepository projectCollaboratorRepository)
        {
            this.projectCollaboratorRepository = projectCollaboratorRepository;
        }

        public void AddCollaborator(NewProjectCollaborator collaborator)
        {
            throw new System.NotImplementedException();
        }

        public void AddCollaborator(List<NewProjectCollaborator> collaborators)
        {
            this.projectCollaboratorRepository.Add(collaborators);
            this.projectCollaboratorRepository.Commit();
        }

        public void DeleteCollaborator(int collaboratorId)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCollaborator(List<int> collaboratorIds)
        {
            this.projectCollaboratorRepository.Delete(collaboratorIds);
            this.projectCollaboratorRepository.Commit();
        }

        public List<ProjectCollaboratorOverview> GetProjectCollaborators(int projectId)
        {
            var collaborators = this.projectCollaboratorRepository.Find(projectId);
            return collaborators;
        }
    }
}