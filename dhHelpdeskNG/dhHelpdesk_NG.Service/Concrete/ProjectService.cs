namespace dhHelpdesk_NG.Service.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Data.Repositories.Projects;
    using dhHelpdesk_NG.Domain.Projects;

    public interface IProjectService
    {
        IList<Project> GetProjects(int customerId);

        Project GetProject(int id);

        DeleteMessage DeleteProject(int id);

        void SaveProject(Project project, out IDictionary<string, string> errors);
        void Commit();
    }

    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork)
        {
            this._projectRepository = projectRepository;
            this._unitOfWork = unitOfWork;;
        }

        public IList<Project> GetProjects(int customerId)
        {
            return this._projectRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Project GetProject(int id)
        {
            return this._projectRepository.GetById(id);
        }

        public DeleteMessage DeleteProject(int id)
        {
            var project = this._projectRepository.GetById(id);

            if (project != null)
            {
                try
                {
                    this._projectRepository.Delete(project);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveProject(Project project, out IDictionary<string, string> errors)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(project.Description))
                errors.Add("Project.Description", "Du måste ange en beskrivning");

            if (string.IsNullOrEmpty(project.Name))
                errors.Add("Project.Name", "Du måste ange ett projekt");

            if (project.Id == 0)
                this._projectRepository.Add(project);
            else
                this._projectRepository.Update(project);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
